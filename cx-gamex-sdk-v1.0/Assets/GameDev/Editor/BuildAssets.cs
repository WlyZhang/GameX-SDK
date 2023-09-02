using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

public class BuildAssets
{
    /// <summary>
    /// 文件名称列表
    /// </summary>

    /// <summary>
    /// Resources 文件夹中的 【.lua.txt】文件名列表
    /// </summary>
    private static List<string> resList = new List<string>();

    /// <summary>
    /// 需要打包Lua脚本列表
    /// </summary>
    private static List<Object> fileList = new List<Object>();

    /// <summary>
    /// 开发模式脚本路径
    /// </summary>
    private static string developPath;

    private static List<string> fileNameList = new List<string>();

    /// <summary>
    /// 用于版本号信息
    /// </summary>
    private static List<Object> abFileList = new List<Object>();

    /// <summary>
    /// 版本信息字典
    /// </summary>
    private static Dictionary<string, object> versionDic = new Dictionary<string, object>();

    /// <summary>
    /// 版本信息
    /// </summary>
    private static string versionInfo;

    /// <summary>
    /// 版本信息生成路径
    /// </summary>
    private static string versionPath = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/AssetBundles/files.txt";

    /// <summary>
    /// Win测试版本路径
    /// </summary>
    private static string winBuildPath = $"{Application.dataPath}/../Debug/{Game.DevelopApp}_Data/StreamingAssets";

    /// <summary>
    /// 热更新模式脚本路径
    /// </summary>
    private static string hotUpdatePath;

    /// <summary>
    /// 打包Bundle路径
    /// </summary>
    private static string buildPath;

    /// <summary>
    /// 打包Bundle平台
    /// </summary>
    private static BuildTarget buildTarget;

    /// <summary>
    /// 当前运行平台
    /// </summary>
    private static string platform = string.Empty;



    //================================= 以上是属性 ================================================




    [MenuItem("HotUpdate/Builder/Windows Platform")]
    public static void BuildWindows()
    {
        platform = "Windows";
        buildTarget = BuildTarget.StandaloneWindows64;
        BuildToAssetBundle();
    }

    [MenuItem("HotUpdate/Builder/Mac Platform")]
    public static void BuildMac()
    {
        platform = "Mac";
        buildTarget = BuildTarget.StandaloneOSX;
        BuildToAssetBundle();
    }

    [MenuItem("HotUpdate/Builder/iOS Platform")]
    public static void BuildIOS()
    {
        platform = "iOS";
        buildTarget = BuildTarget.iOS;
        BuildToAssetBundle();
    }

    [MenuItem("HotUpdate/Builder/Android Platform")]
    public static void BuildAndroid()
    {
        platform = "Android";
        buildTarget = BuildTarget.Android;
        BuildToAssetBundle();
    }

    [MenuItem("HotUpdate/Clean Bundle")]
    public static void DeleteCache()
    {
        PlayerPrefs.DeleteAll();

        ClearList();

        string cachePath = $"{Application.persistentDataPath}/{Game.ModuleName}/{platform}/";
        string luaPath = $"{Application.dataPath}/LuaProject/{Game.ModuleName}/{platform}/";
        string streamingPath = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/";

        ClearCache(cachePath);
        ClearCache(luaPath);
        ClearCache(streamingPath);


        //winBuild 目录清理
        ClearCache($"{winBuildPath}/{Game.ModuleName}");

        AssetDatabase.Refresh();
    }

    [MenuItem("HotUpdate/Clean Bundle All")]
    public static void DeleteCacheAll()
    {
        PlayerPrefs.DeleteAll();

        ClearList();

        string cachePath = $"{Application.persistentDataPath}/";
        string luaPath = $"{Application.dataPath}/LuaProject/";
        string streamingPath = $"{Application.streamingAssetsPath}/";

        ClearCache(cachePath);
        ClearCache(luaPath);
        ClearCache(streamingPath);


        //winBuild 目录清理
        ClearCache($"{winBuildPath}/");

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 清理列表
    /// </summary>
    public static void ClearList()
    {
        resList.Clear();
        fileList.Clear();
        fileNameList.Clear();
        abFileList.Clear();
        versionDic.Clear();
    }

    //[MenuItem("Tools/热更新/拷贝资源到StreamingAssets文件夹")]
    public static void CopyToStreamingAssets()
    {
        string lua_ab = "lua";
        string sourcePath = $"{Application.dataPath}/../AssetBundles/StandaloneWindows64/src/";
        string copyPath = $"{Application.streamingAssetsPath}/src/";

        ClearCache(copyPath);

        if (!Directory.Exists(copyPath))
        {
            Directory.CreateDirectory(copyPath);
        }

        if (File.Exists($"{sourcePath}{lua_ab}"))
        {
            File.Copy($"{sourcePath}{lua_ab}", $"{copyPath}{lua_ab}", true);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError($"【{sourcePath}{lua_ab}】路径没有源文件");
        }

        Debug.Log("<color=yellow>已拷贝资源到StreamingAssets文件夹!</color>");
    }


    /// <summary>
    /// 打包Lua脚本到AssetBundle
    /// </summary>
    private static async void BuildToAssetBundle()
    {
        ClearList();

        versionInfo = string.Empty;

        AssetLoadManager.Create();

        string luaPath = $"{Application.dataPath}/LuaProject/{Game.ModuleName}/{platform}/src/Resources/";

        ClearCache(luaPath);

        string streamPath = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/";

        ClearCache(streamPath);

        string winPath = $"{winBuildPath}/{Game.ModuleName}/{platform}/";

        ClearCache(winPath);

        await CopyLuaScriptsToFolder();

        Debug.Log("<color=yellow>已整理Lua脚本完成!</color>");

        //设置BundleName
        await ReNameBundle("src/lua");

        //刷新编辑器
        AssetDatabase.Refresh();

        await GetAssetAll($"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/AssetBundles/");

        //生成版本信息
        versionPath = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/AssetBundles/files.txt";
        CreateTextFile(versionPath, versionInfo.Trim());

        //拷贝StreamingAssets资源到Win测试版本路径
        CopyFolder(streamPath, winPath);

        //刷新编辑器
        AssetDatabase.Refresh();

        Debug.Log("<color=green>热更新资源打包完成!</color>");





















        //打开外部GameX测试程序
        System.Diagnostics.Process.Start($"{Application.dataPath}/../Debug/{Game.DevelopApp}.exe");

    }

    private static async Task GetAssetAll(string dirPath)
    {
        string[] dirList = Directory.GetDirectories(dirPath);

        if (dirList.Length > 0)
        {
            //遍历子文件夹
            for (int i = 0; i < dirList.Length; i++)
            {
                //获取下一层文件夹
                await GetAssetAll(dirList[i]);
            }

            //添加子文件夹文件
            await GetAsset(dirPath);
        }
        else
        {
            //添加本文件夹文件
            await GetAsset(dirPath);
        }

    }

    /// <summary>
    /// 添加文件夹脚本文件
    /// </summary>
    /// <param name="dirPath"></param>
    private static async Task GetAsset(string dirPath)
    {
        string[] dirlist = Directory.GetFiles(dirPath);
        if (dirlist.Length > 0)
        {
            for (int i = 0; i < dirlist.Length; i++)
            {
                dirlist[i] = dirlist[i].Replace("\\", "/");

                //过滤文件
                if(dirlist[i].Contains(".meta")|| dirlist[i].Contains(".manifest"))
                {
                    continue;
                }



                Debug.Log(dirlist[i]);


                string tempPath = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/AssetBundles/";
                string name = dirlist[i].Replace(tempPath, "");
                byte[] byteArr = await AssetLoadManager.Instance.LoadAssetAsyncWithByte(dirlist[i]);

                string md5 = CreateMD5(byteArr);
                AddSingleVersion(name, md5);
            }

            versionInfo = JsonConvert.SerializeObject(versionDic);
        }
    }

    /// <summary>
    /// 复制Lua脚本到打包文件夹
    /// </summary>
    private static async Task CopyLuaScriptsToFolder()
    {
        ClearList();

        developPath = $"{Application.dataPath}/../LuaProject/{Game.ModuleName}/src/";
        hotUpdatePath = $"{Application.dataPath}/LuaProject/{Game.ModuleName}/{platform}/src/Resources/";
        buildPath = $"Assets/LuaProject/{Game.ModuleName}/{platform}/src/Resources/";

        if (!Directory.Exists(hotUpdatePath))
        {
            Directory.CreateDirectory(hotUpdatePath);
        }

        if (!string.IsNullOrEmpty(developPath))
        {
            await GetScriptAll(developPath);
        }

        //复制Lua脚本
        await CopyLuaScripts();

        //刷新编辑器
        AssetDatabase.Refresh();

    }

    /// <summary>
    /// 获取文件夹全部脚本文件
    /// </summary>
    /// <param name="dirPath"></param>
    private static async Task GetScriptAll(string dirPath)
    {
        string[] dirList = Directory.GetDirectories(dirPath);

        if (dirList.Length > 0)
        {
            //遍历子文件夹
            for (int i = 0; i < dirList.Length; i++)
            {
                //获取下一层文件夹
                await GetScriptAll(dirList[i]);
            }

            //添加子文件夹文件
            AddScript(dirPath);
        }
        else
        {
            //添加本文件夹文件
            AddScript(dirPath);
        }
    }

    /// <summary>
    /// 添加文件夹脚本文件
    /// </summary>
    /// <param name="dirPath"></param>
    private static void AddScript(string dirPath)
    {
        string[] dirlist = Directory.GetFiles(dirPath);
        if (dirlist.Length > 0)
        {
            for (int i = 0; i < dirlist.Length; i++)
            {
                dirlist[i] = dirlist[i].Replace("\\", "/");

                fileNameList.Add(dirlist[i]);
            }
        }
    }

    /// <summary>
    /// 复制Lua脚本到打包文件夹
    /// </summary>
    private static async Task CopyLuaScripts()
    {
        if (fileNameList.Count > 0)
        {
            await Task.Yield();

            for (int i = 0; i < fileNameList.Count; i++)
            {
                string luaPath = fileNameList[i].Replace(developPath, hotUpdatePath);

                string[] strArr = luaPath.Split('/');
                string[] splitArr = strArr[strArr.Length - 1].Split('.');
                string key = splitArr[0];

                string dirPath = luaPath.Replace($"{key}.lua", "");
                if(!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string fileName = $"{luaPath}.txt";
                File.Copy(fileNameList[i], fileName, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换

                string tempPath = $"{Application.dataPath}/";
                string midPath = luaPath.Replace(tempPath, "Assets/");

                Debug.Log(midPath);

                resList.Add($"{midPath}.txt");
            }
        }
    }

    /// <summary>
    /// 设置Bundle名称
    /// </summary>
    /// <param name="dir"></param>
    private static async Task ReNameBundle(string dir)
    {
        await Task.Yield();

        for (int i = 0; i<resList.Count; i++)
        {
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(resList[i]);

            fileList.Add(obj);

            AssetImporter imp = AssetImporter.GetAtPath(resList[i]);

            imp.assetBundleName = dir.Trim();

            imp.SaveAndReimport();
        }

        AssetDatabase.Refresh();


        //打包Bundle
        BuildBundles();
    }

    /// <summary>
    /// 打包Bundle
    /// </summary>
    private static void BuildBundles()
    {
        string assetBundlesLibrary = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/AssetBundles";

        if(buildTarget == BuildTarget.Android)
        {
            //assetBundlesLibrary += "/Android";
            if (!Directory.Exists(assetBundlesLibrary))//判断本地目录是否已经存在目录名
            {
                Directory.CreateDirectory(assetBundlesLibrary);
            }

            BuildPipeline.BuildAssetBundles(assetBundlesLibrary, BuildAssetBundleOptions.None,
                BuildTarget.Android);//在此目录下创建AssetBundle
        }
        else if(buildTarget == BuildTarget.iOS)
        {
            //assetBundlesLibrary += "/iOS";
            if (!Directory.Exists(assetBundlesLibrary))//判断本地目录是否已经存在目录名
            {
                Directory.CreateDirectory(assetBundlesLibrary);
            }

            BuildPipeline.BuildAssetBundles(assetBundlesLibrary, BuildAssetBundleOptions.None,
                BuildTarget.iOS);//在此目录下创建AssetBundle
        }
        else if(buildTarget == BuildTarget.StandaloneWindows64)
        {
            //assetBundlesLibrary += "/Windows/";

            if (!Directory.Exists(assetBundlesLibrary))//判断本地目录是否已经存在目录名
            {
                Directory.CreateDirectory(assetBundlesLibrary);
            }

            BuildPipeline.BuildAssetBundles(assetBundlesLibrary, BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows64);//在此目录下创建AssetBundle
        }
        else if(buildTarget == BuildTarget.StandaloneOSX)
        {
            //assetBundlesLibrary += "/Mac/";
            ClearCache($"{assetBundlesLibrary}src/");

            if (!Directory.Exists(assetBundlesLibrary))//判断本地目录是否已经存在目录名
            {
                Directory.CreateDirectory(assetBundlesLibrary);
            }

            BuildPipeline.BuildAssetBundles(assetBundlesLibrary, BuildAssetBundleOptions.None,
                BuildTarget.StandaloneOSX);//在此目录下创建AssetBundle
        }

        buildTarget = BuildTarget.NoTarget;
        //拷贝资源到StreamingAssets文件夹
        //CopyToStreamingAssets();
    }

    /// <summary>
    /// 清理缓存
    /// </summary>
    private static void ClearCache(string cachePath)
    {
        if (Directory.Exists(cachePath))
        {
            Directory.Delete(cachePath, true);
            Debug.Log($"<color=yellow>{cachePath} 缓存已清理</color>");
        }
        else
        {
            Debug.Log($"{cachePath} 无缓存目录");
        }
    }

    /// <summary>
    /// 复制文件夹及文件
    /// </summary>
    /// <param name="sourceFolder">原文件路径</param>
    /// <param name="destFolder">目标文件路径</param>
    /// <returns></returns>
    public static int CopyFolder(string sourceFolder, string destFolder)
    {
        try
        {
            //如果目标路径不存在,则创建目标路径
            if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                System.IO.File.Copy(file, dest);//复制文件
            }
            //得到原文件根目录下的所有文件夹
            string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(destFolder, name);
                CopyFolder(folder, dest);//构建目标路径,递归复制文件
            }
            return 1;
        }
        catch
        {
            return -1;
        }
    }

    /// <summary>
    /// 添加单条版本信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="md5"></param>
    private static void AddSingleVersion(string name,string md5)
    {
        if(!versionDic.ContainsKey(name))
        {
            versionDic.Add(name, md5);
        }
    }

    /// <summary>
    /// 计算md5
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    private static string CreateMD5(byte[] buffer)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] md5Bytes = md5.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; i++)
            {
                sb.Append(md5Bytes[i].ToString("x2"));//X2时，生成字母大写MD5
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// 创建版本信息
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="info"></param>
    private static void CreateTextFile(string path, string info)
    {
#if !UNITY_WEBPLAYER
        //文件流信息  
        StreamWriter sw;
        FileInfo t = new FileInfo(path);
        t.Delete();
        sw = t.CreateText();
        //以行的形式写入信息  
        sw.WriteLine(info);
        //关闭流  
        sw.Close();
        //销毁流  
        sw.Dispose();
#endif
    }
}