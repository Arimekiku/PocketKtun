namespace Scripts.Utils.Loaders;

public class JsonCompressedLoader<TTarget> : JsonLoader<TTarget> where TTarget : class
{
    public JsonCompressedLoader(string path, string fileName) : base(path, fileName)
    {
    }

    protected override TTarget LoadFromJson() => JsonUtils.LoadCompressedFile<TTarget>(_path, _fileName);
}