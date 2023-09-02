
LoadManager = {}

--初始化路径
function LoadManager:Init(streamPath, cachePath)
    if streamPath ~= nil and cachePath ~= nil then
        print("打包加载路径：-->    "..streamPath)
        print("缓存加载路径：-->    "..cachePath)

        LoadManager.tempPath = streamPath
        LoadManager.tempCachePath = cachePath

        LoadManager.path = streamPath
        LoadManager.cachePath = cachePath   
    else
        error("资源加载【主】路径为nil")
    end
end

--选择加载文件夹
function LoadManager:SwitchDir(localPath)
    if localPath ~= nil then

        LoadManager.path = LoadManager.tempPath
        LoadManager.cachePath = LoadManager.tempCachePath

        LoadManager.path = LoadManager.path..localPath
        LoadManager.cachePath = LoadManager.cachePath..localPath
    else
        error("资源加载【Switch】路径为nil")
    end
end

--加载资源
function LoadManager:LoadAsset(assetName,isInitPath)
    
    local path = nil
    if isInitPath then
        path = LoadManager.path..assetName
    else
        path = LoadManager.cachePath..assetName
    end

    print(path)
    local ab = unity.AssetBundle.LoadFromFile(path)
    local go = ab:LoadAsset(assetName)

    return go
end

--加载场景
function LoadManager:LoadScene(sceneName,isInitPath)
    LoadManager:SwitchDir("/Scene/")

    local path = nil
    if isInitPath then
        path = LoadManager.path..sceneName
        print("加载 StreamingAssets 路径资源")
    else
        path = LoadManager.cachePath..sceneName
        print("加载 沙盒 路径资源")
    end

    local ab = unity.AssetBundle.LoadFromFile(path)
    unity.SceneManagement.SceneManager.LoadScene(sceneName,unity.SceneManagement.LoadSceneMode.Additive)
end

--加载音频
function LoadManager:LoadAudio(audioName,isInitPath)

    local path = nil
    if isInitPath then
        path = LoadManager.path..audioName
        print("加载 StreamingAssets 路径资源")
    else
        path = LoadManager.cachePath..audioName
        print("加载 沙盒 路径资源")
    end

    local ab = unity.AssetBundle.LoadFromFile(path)
    local clip = ab:LoadAsset(audioName)
    print(clip.name)

    return clip
end

--加载场景（测试模式）
function LoadManager:TryLoadScene(sceneName,isInitPath,isTest)
    if isTest then
        unity.SceneManagement.SceneManager.LoadScene(sceneName,unity.SceneManagement.LoadSceneMode.Additive)
    else
        LoadManager:LoadScene(sceneName,isInitPath)
    end
end

--拷贝文件夹
function LoadManager:CopyFolder(sourceFolder, destFolder)
    AssetLoadManager.CopyFolder(sourceFolder, destFolder)
end

--删除缓存文件夹
function LoadManager:DeleteCacheFolder()
    AssetLoadManager.DeleteCacheFolder()
end

return LoadManager