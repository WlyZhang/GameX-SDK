using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AssetLoadManager
{
    public static AssetLoadManager Instance;

    private static string CachePath;

    public static void Create()
    {
        Instance = new AssetLoadManager();

        CachePath = $"{Application.persistentDataPath}/{Game.ModuleName}/AssetBundles";
    }

    /// <summary>
    /// 加载
    /// </summary>
    /// <param name="type">加载类型</param>
    /// <param name="path">加载路径</param>
    /// <param name="assetName">加载资源名</param>
    /// <param name="callback">回调函数</param>
    /// <param name="isAdditive">场景是否叠加</param>
    public static async void Load(string type, string path, string assetName, Action<UnityEngine.Object> callback = null, bool isAdditive = false)
    {
        if (type.Equals("model"))
        {
            UnityEngine.Object obj = await Instance.LoadAssetAsync(path, assetName);

            if (callback != null)
            {
                callback(obj);
            }
        }
        else if (type.Equals("scene"))
        {
            if (isAdditive)
            {
                await Instance.LoadSceneAsync(path, assetName, LoadSceneMode.Additive);
            }
            else
            {
                await Instance.LoadSceneAsync(path, assetName, LoadSceneMode.Single);
            }

            if(callback != null)
            {
                callback(null);
            }
        }
        else if (type.Equals("texture"))
        {
            UnityEngine.Texture texture = await Instance.LoadTextureAsync(path, assetName);

            if (callback != null)
            {
                callback(texture);
            }
        }
        else if (type.Equals("audio"))
        {
            UnityEngine.AudioClip clip = await Instance.LoadAudioAsync(path, assetName);

            if (callback != null)
            {
                callback(clip);
            }
        }
        else if (type.Equals("video"))
        {
            UnityEngine.Video.VideoClip clip = await Instance.LoadVideoAsync(path, assetName);

            if (callback != null)
            {
                callback(clip);
            }
        }
        else if (type.Equals("sprite"))
        {
            byte[] bytes = await Instance.LoadAssetAsyncWithByte(path);

            if (callback != null)
            {
                Texture2D t2d = new Texture2D(1024, 1024);
                bool boo = t2d.LoadImage(bytes);

                if(boo)
                {
                    Sprite sprite = Sprite.Create(t2d,new Rect(0,0,t2d.width,t2d.height),new Vector2(t2d.width/2,t2d.height/2));
                    callback(sprite);
                }
            }
        }
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="path">AssetPath路径</param>
    /// <param name="assetName">资源名称</param>
    /// <param name="loadSceneMode">加载场景模式</param>
    /// <returns></returns>
    public async Task LoadSceneAsync(string path, string assetName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive, bool canUpdate = true)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{path}");

        request.SendWebRequest();

        while(!request.downloadHandler.isDone)
        {
            await Task.Yield();
        }

        byte[] results = request.downloadHandler.data;

        if(canUpdate)
        {
            CreateFile($"{CachePath}/Scene/{assetName}", results, results.Length);
        }

        AssetBundle ab = AssetBundle.LoadFromMemory(results);

        SceneManager.LoadScene(assetName, loadSceneMode);
    }

    /// <summary>
    /// 异步加载模型
    /// </summary>
    /// <param name="path">AssetPath路径</param>
    /// <param name="assetName">资源名称</param>
    /// <returns></returns>
    public async Task<GameObject> LoadAssetAsync(string path, string assetName, bool canUpdate = true)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{path}");

        request.SendWebRequest();

        while (!request.downloadHandler.isDone)
        {
            await Task.Yield();
        }

        byte[] results = request.downloadHandler.data;

        if(canUpdate)
        {
            CreateFile($"{CachePath}/Model/{assetName}", results, results.Length);
        }

        AssetBundle ab = AssetBundle.LoadFromMemory(results);

        GameObject go = ab.LoadAsset<GameObject>(assetName);

        return go;
    }

    /// <summary>
    /// 异步加载图片
    /// </summary>
    /// <param name="path"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public async Task<Texture> LoadTextureAsync(string path,string assetName, bool canUpdate = true)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{path}");

        request.SendWebRequest();

        while (!request.downloadHandler.isDone)
        {
            await Task.Yield();
        }

        byte[] results = request.downloadHandler.data;

        if(canUpdate)
        {
            CreateFile($"{CachePath}/Texture/{assetName}", results, results.Length);
        }

        AssetBundle ab = AssetBundle.LoadFromMemory(results);

        Texture texture = ab.LoadAsset<Texture>(assetName);

        return texture;
    }

    /// <summary>
    /// 异步加载音频
    /// </summary>
    /// <param name="path"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public async Task<AudioClip> LoadAudioAsync(string path, string assetName, bool canUpdate = true)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{path}");

        request.SendWebRequest();

        while (!request.downloadHandler.isDone)
        {
            await Task.Yield();
        }

        byte[] results = request.downloadHandler.data;

        if(canUpdate)
        {
            CreateFile($"{CachePath}/Audio/{assetName}", results, results.Length);
        }

        AssetBundle ab = AssetBundle.LoadFromMemory(results);

        AudioClip clip = ab.LoadAsset<AudioClip>(assetName);

        return clip;
    }

    /// <summary>
    /// 异步加载视频
    /// </summary>
    /// <param name="path"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public async Task<VideoClip> LoadVideoAsync(string path, string assetName, bool canUpdate = true)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{path}");

        request.SendWebRequest();

        while (!request.downloadHandler.isDone)
        {
            await Task.Yield();
        }

        byte[] results = request.downloadHandler.data;

        if(canUpdate)
        {
            CreateFile($"{CachePath}/Video/{assetName}", results, results.Length);
        }

        AssetBundle ab = AssetBundle.LoadFromMemory(results);

        VideoClip clip = ab.LoadAsset<VideoClip>(assetName);

        return clip;
    }

    /// <summary>
    /// 异步加载模型
    /// </summary>
    /// <param name="path"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public async Task<byte[]> LoadAssetAsyncWithByte(string path)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{path}");

        request.SendWebRequest();

        while (!request.downloadHandler.isDone)
        {
            await Task.Yield();
        }

        byte[] results = request.downloadHandler.data;

        return results;
    }

    /// <summary>
    /// 下载服务器资源缓存本地沙盒
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<bool> LoadAssetAsyncCache(string path, string pathId)
    {
        path = path.Replace("\\", "/");
        pathId = pathId.Replace("\\", "/");

        UnityWebRequest request = UnityWebRequest.Get($"{path}");

        request.SendWebRequest();

        while (!request.downloadHandler.isDone)
        {
            await Task.Yield();
        }

        byte[] results = request.downloadHandler.data;

        string[] strArr = pathId.Split('/');
        string name = strArr[strArr.Length - 1];
        string assetPath = CachePath;

        for (int i = 0; i < strArr.Length-1 ; i++)
        {
            assetPath += $"/{strArr[i]}";
        }

        CreateFile(assetPath, name, results, results.Length);

        Debug.Log($"<color=yellow>{pathId} download complete</color>");

        return request.downloadHandler.isDone;
    }

    /// <summary>
    /// 加载版本信息
    /// </summary>
    /// <param name="path"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public async Task<string> LoadVersionAsync(string path)
    {
        path = path.Replace("\\", "/");

        UnityWebRequest request = UnityWebRequest.Get($"{path}");

        request.SendWebRequest();

        if (!request.downloadHandler.isDone)
        {
            await Task.Delay(1000);
        }

        byte[] results = request.downloadHandler.data;

        string info = request.downloadHandler.text.Trim();

        return info;
    }

    /// <summary>
    /// 缓存文件
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="name">仅文件名（不包含局部路径）</param>
    /// <param name="info">流数据</param>
    /// <param name="length">流数据长度</param>
    public void CreateFile(string path, string name, byte[] info, int length)
    {
#if !UNITY_WEBPLAYER

        string assetPath = $"{path}/{name}";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //文件流信息
        Stream sw;
        FileInfo t = new FileInfo(assetPath);

        t.Delete();

        sw = t.Create();
        //以行的形式写入信息  
        //sw.WriteLine(info);  
        sw.Write(info, 0, length);
        //关闭流  
        sw.Close();
        //销毁流  
        sw.Dispose();
#endif
    }

    /// <summary>
    /// 缓存文件
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="name">仅文件名（不包含局部路径）</param>
    /// <param name="info">流数据</param>
    /// <param name="length">流数据长度</param>
    public void CreateFile(string path, byte[] info, int length)
    {
#if !UNITY_WEBPLAYER

        string[] strArr = path.Split('/');
        string name = strArr[strArr.Length - 1];

        string assetPath = $"{path}/{name}";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //文件流信息
        Stream sw;
        FileInfo t = new FileInfo(assetPath);

        t.Delete();

        sw = t.Create();
        //以行的形式写入信息  
        //sw.WriteLine(info);  
        sw.Write(info, 0, length);
        //关闭流  
        sw.Close();
        //销毁流  
        sw.Dispose();
#endif
    }

    /// <summary>
    /// 缓存文本文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="info"></param>
    public void CreateTextFile(string path, string name, string info)
    {
#if !UNITY_WEBPLAYER

        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        if(File.Exists($"{path}/{name}"))
        {
            File.Delete($"{path}/{name}");
        }

        File.WriteAllText($"{path}/{name}", info);

#endif
    }

    /// <summary>
    /// 清理GC
    /// </summary>
    public static void Unload(bool isUnloadAll)
    {
        AssetBundle.UnloadAllAssetBundles(isUnloadAll);
        Resources.UnloadUnusedAssets();
        GC.Collect();
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
    /// 删除缓存文件夹
    /// </summary>
    public static void DeleteCacheFolder()
    {
        string path = Application.persistentDataPath;

        if(Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
}