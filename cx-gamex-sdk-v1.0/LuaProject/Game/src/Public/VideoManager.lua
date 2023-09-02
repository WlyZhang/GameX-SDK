
VideoManager = {}

--初始化
function VideoManager:Init(isLoadLocal)
    VideoManager.isLoadLocal = isLoadLocal
    VideoManager.allVideos = {}

    if isLoadLocal then
        print("准备加载 Resources 资源")
    else
        print("准备加载 Custom 资源")
    end
end



--获取视频
function VideoManager:GetVideo(audioName)
    
    local player = unity.GameObject(audioName):AddComponent(typeof(unity.Video.VideoPlayer))
    player.transform.parent = unity.GameObject.Find(AppConst.moduleName).transform
    
    player.source = unity.Video.VideoSource.VideoClip;
    player.aspectRatio = unity.Video.VideoAspectRatio.Stretch;
    local clip = VideoManager:CreateVideo(audioName,true)
    local rt = unity.RenderTexture(1920,1080,0)
    player.clip = clip
    player.targetTexture = rt;
    player:Play()

    return player
end


--创建视频
function VideoManager:CreateVideo(videoName,isInitPath)

    LoadManager:SwitchDir("/Video/")

    local clip = VideoManager:Load(videoName, isInitPath)
    return clip
end


--加载视频
function VideoManager:Load(videoName, isInitPath)

    if VideoManager.allVideos[videoName] == nil then

        local modelPrefab = nil
        --是否加载本地资源--------------------------------------
        if VideoManager.isLoadLocal then
            modelPrefab = unity.Resources.Load("Video/"..videoName)
            print("<color=yellow>".."加载 Resources Audio 资源: "..modelPrefab.name.."</color>")
        else
            modelPrefab = LoadManager:LoadAsset(videoName, isInitPath)
            print("<color=yellow>".."加载 Custom 路径 Audio 资源: "..modelPrefab.name.."</color>")
        end
        -------------------------------------------------------

        if modelPrefab ~= nil then
            VideoManager.allVideos[videoName] = modelPrefab
            --print("加载 "..panelName.." 完成")
            return modelPrefab
        else
            print(videoName.." 预设无法加载")
            return nil
        end
    else
       return VideoManager.allVideos[videoName]
    end
end

--销毁视频
function VideoManager:Destroy(videoName)
    for key, value in pairs(VideoManager.allVideos) do
        if key == videoName then
            table.remove(VideoManager.allVideos, VideoManager.allVideos[videoName].index)
        end
    end
end

return VideoManager