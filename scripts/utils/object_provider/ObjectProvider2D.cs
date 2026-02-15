using Godot;

namespace Scripts.Utils;

[GlobalClass]
public partial class ObjectProvider2D : BaseObjectProvider<CanvasItem, NodePool2D>
{
    [Export] private CanvasItem[] _objects;
    
    protected override CanvasItem[] Objects => _objects;
    
    protected override NodePool2D CreateNodePool(CanvasItem node, Node container, int poolSize) =>
        new NodePool2D(node, container, poolSize);
}
