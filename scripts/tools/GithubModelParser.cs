using Godot;
using System.Collections.Generic;
using System.Text;

[Tool]
public partial class GithubModelParser : EditorPlugin
{
    private const string GITHUB_USER = "YOUR_GITHUB_USERNAME";
    private const string GITHUB_REPO = "YOUR_REPO_NAME";
    private const string BRANCH = "main";

    private const string MANIFEST_URL =
        "https://raw.githubusercontent.com/" +
        GITHUB_USER + "/" +
        GITHUB_REPO + "/" +
        BRANCH + "/manifest.json";

    private const string LOCAL_BASE_PATH = "res://Models/";

    private HttpRequest _http;
    private Queue<ManifestFile> _downloadQueue = new();
    private bool _isDownloading = false;

    public override void _EnterTree()
    {
        AddToolMenuItem("Sync Model Repository", Callable.From(StartSync));
    }

    public override void _ExitTree()
    {
        RemoveToolMenuItem("Sync Model Repository");
    }

    private void StartSync()
    {
        GD.Print("Starting model repo sync...");

        _http = new HttpRequest();
        AddChild(_http);
        _http.RequestCompleted += OnManifestDownloaded;

        var headers = BuildHeaders();
        _http.Request(MANIFEST_URL, headers);
    }

    private void OnManifestDownloaded(
        long result,
        long responseCode,
        string[] headers,
        byte[] body)
    {
        if (responseCode != 200)
        {
            GD.PushError("Failed to download manifest.json");
            return;
        }

        var jsonText = Encoding.UTF8.GetString(body);
        var json = Json.ParseString(jsonText);

        if (json.VariantType != Variant.Type.Dictionary)
        {
            GD.PushError("Invalid manifest format");
            return;
        }

        var dict = json.AsGodotDictionary();
        var files = dict["files"].AsGodotArray();

        foreach (var item in files)
        {
            var fileDict = item.AsGodotDictionary();
            var path = fileDict["path"].AsString();
            var size = (long)fileDict["size"];

            var localPath = LOCAL_BASE_PATH + path;
            if (FileAccess.FileExists(localPath))
                continue;
            
            GD.Print("Missing: ", path);
            _downloadQueue.Enqueue(new ManifestFile(path, size));
        }

        if (_downloadQueue.Count == 0)
        {
            GD.Print("All models are already synced");
            Cleanup();
            return;
        }

        DownloadNext();
    }

    private void DownloadNext()
    {
        if (_downloadQueue.Count == 0)
        {
            FinishSync();
            return;
        }

        var file = _downloadQueue.Dequeue();
        var url =
            "https://raw.githubusercontent.com/" +
            GITHUB_USER + "/" +
            GITHUB_REPO + "/" +
            BRANCH + "/" +
            file.Path;

        GD.Print("Downloading: ", file.Path);

        _http.RequestCompleted -= OnManifestDownloaded;
        _http.RequestCompleted += (r, c, h, b) =>
            OnFileDownloaded(file, r, c, h, b);

        _http.Request(url, BuildHeaders());
    }

    private void OnFileDownloaded(
        ManifestFile file,
        long result,
        long responseCode,
        string[] headers,
        byte[] body)
    {
        if (responseCode != 200)
        {
            GD.PushError("Failed to download: " + file.Path);
            DownloadNext();
            return;
        }

        var fullPath = LOCAL_BASE_PATH + file.Path;
        EnsureDirectoryExists(fullPath);

        using var f = FileAccess.Open(fullPath, FileAccess.ModeFlags.Write);
        f.StoreBuffer(body);
        f.Close();

        GD.Print("Saved: ", fullPath);

        DownloadNext();
    }

    private void FinishSync()
    {
        GD.Print("Model sync complete");

        EditorInterface.Singleton
            .GetResourceFilesystem()
            .Scan();

        Cleanup();
    }

    private void Cleanup()
    {
        if (_http == null)
            return;
        
        _http.QueueFree();
        _http = null;
    }

    private string[] BuildHeaders()
    {
        var headers = new List<string>
        {
            "User-Agent: Godot"
        };

        return headers.ToArray();
    }

    private void EnsureDirectoryExists(string filePath)
    {
        var dirPath = filePath.GetBaseDir();

        if (!DirAccess.DirExistsAbsolute(dirPath))
        {
            DirAccess.MakeDirRecursiveAbsolute(dirPath);
        }
    }

    private struct ManifestFile
    {
        public string Path;

        public ManifestFile(string path, long size)
        {
            Path = path;
        }
    }
}