using Scripts.Utils;
using System;
using System.Linq;
using System.Reflection;

namespace Scripts.DIContainer;

internal class Resolver : IResolver
{
    private readonly MemberMap _membersMap;
    private readonly IContextProvider _contextProvider;
    
    public Resolver(IContextProvider contextProvider)
    {
        _membersMap = new MemberMap();
        _contextProvider = contextProvider;
    }

    public bool Inject<TInstance>(TInstance instance) where TInstance : class => Inject(instance as object);

    public TContract Resolve<TContract>() where TContract : class => Resolve(typeof(TContract)) as TContract;
    
    private bool Inject(object instance)
    {
        var instanceType = instance.GetType();

        if (!_membersMap.ClassIsRegistered(instanceType))
            return false;

        InjectFields(instance);
        InjectProperties(instance);
        InjectMethods(instance);
        
        return true;
    }
    
    private object Resolve(Type type, object injectedInstance = null)
    {
        var context = _contextProvider.GetContext(injectedInstance);
        
        var bind = context.GetBind(type, injectedInstance?.GetType());

        ExceptionsUtils.ThrowIfNull(bind, $"Bind not found for {type}. Cant resolve dependence.");

        if (bind.BindInstanceCreator.LiveScope == LiveScope.AsSingle)
        {
            if (bind.BindInstanceCreator.SingleInstance != null) 
                return bind.BindInstanceCreator.SingleInstance;
                    
            if (bind.BindInstanceCreator.ConstructMethod != null)
                return bind.BindInstanceCreator.ConstructMethod;

            bind.BindInstanceCreator.SingleInstance = CreateNewInstance(bind.RealizationType);
            return bind.BindInstanceCreator.SingleInstance;
        }

        if (bind.BindInstanceCreator.LiveScope == LiveScope.AsTransient)
            return CreateNewInstance(bind.RealizationType);

        throw new Exception($"Bind instance creator for {bind.RealizationType} is not supported.");
    }
    
    private void InjectFields(object instance)
    {
        var injectMembers = _membersMap.GetMembers(instance.GetType(), MemberTypes.Field);

        if (injectMembers.IsNullOrEmpty())
            return;

        foreach (var injectMember in injectMembers)
        {
            var injectField = injectMember as InjectField;
            injectField!.Inject(instance, Resolve(injectField.ParameterType, instance));
        }
    }

    private void InjectProperties(object instance)
    {
        var injectMembers = _membersMap.GetMembers(instance.GetType(), MemberTypes.Property);

        if (injectMembers.IsNullOrEmpty())
            return;

        foreach (var injectMember in injectMembers)
        {
            var injectProperty = injectMember as InjectProperty;
            injectProperty!.Inject(instance, Resolve(injectProperty.ParameterType, instance));
        }
    }

    private void InjectMethods(object instance)
    {
        var injectMembers = _membersMap.GetMembers(instance.GetType(), MemberTypes.Method);

        if (injectMembers.IsNullOrEmpty())
            return;

        foreach (var member in injectMembers)
        {
            var injectMethod = member as InjectMethod;

            var parameters = injectMethod!.ParameterType;
            var dependencies = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; ++i)
                dependencies[i] = Resolve(parameters[i].ParameterType, instance);

            injectMethod.Inject(instance, dependencies);
        }
    }

    private object CreateNewInstance(Type instanceType)
    {
        var constructors = instanceType.GetConstructors(BindingFlags.Public |  BindingFlags.NonPublic | BindingFlags.Instance);
        var constructor = constructors.FirstOrDefault(c => c.IsAttributeDefined<InjectAttribute>()) ??
                          constructors.FirstOrDefault(c => c.GetParameters() == Array.Empty<ParameterInfo>());

        if (constructor == null)
            throw new ArgumentException($"Class {instanceType} does not have a public empty constructor or public constructor marked with InjectAttribute");

        var constructorParameters = constructor.GetParameters();
        var dependencies = new object[constructorParameters.Length];

        for (var i = 0; i < constructorParameters.Length; ++i)
            dependencies[i] = Resolve(constructorParameters[i].ParameterType);

        return Activator.CreateInstance(instanceType, dependencies);
    }
}