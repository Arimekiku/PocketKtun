using System;

namespace Scripts.Utils.Loaders;

public class JsonLoader<TTarget> : Loader<TTarget> where TTarget : class
{
    protected readonly string _path;
    protected readonly string _fileName;

    public JsonLoader(string path, string fileName)
    {
        _path = path;
        _fileName = fileName;
    }

    protected override TTarget LoadTarget()
    {
        var target = LoadFromJson();

        if (target == null)
            throw new
                InvalidOperationException($"Failed to load object of type {typeof(TTarget).Name} from file '{_fileName}' at path '{_path}'.");

        return target;
    }

    protected virtual TTarget LoadFromJson() => JsonUtils.LoadFromFile<TTarget>(_path, _fileName);
}