
AudioManager = {}

--初始化
function AudioManager:Init(isLoadLocal)
    AudioManager.isLoadLocal = isLoadLocal
    AudioManager.allAudios = {}

    if isLoadLocal then
        print("准备加载 Resources 资源")
    else
        print("准备加载 Custom 资源")
    end
end

--播放音频
function AudioManager:Play(audioName,loop)
    
    local audio = unity.GameObject(audioName):AddComponent(typeof(unity.AudioSource))
    audio.transform.parent = unity.GameObject.Find(AppConst.moduleName).transform

    --CreateAudio(audioName,bool)这里的【bool】指向加载文件夹的路径
    local clip = AudioManager:CreateAudio(audioName,false)
    audio.loop = loop
    audio.clip = clip
    audio:Play()

    return audio
end


--创建音频
function AudioManager:CreateAudio(audioName,isInitPath)

    LoadManager:SwitchDir("/Audio/")

    local clip = AudioManager:Load(audioName, isInitPath)
    return clip
end


--加载音频
function AudioManager:Load(audioName, isInitPath)

    if AudioManager.allAudios[audioName] == nil then

        local modelPrefab = nil
        --是否加载本地资源--------------------------------------
        if AudioManager.isLoadLocal then
            modelPrefab = unity.Resources.Load("Audio/"..audioName)
            print("<color=yellow>".."加载 Resources Audio 资源: "..modelPrefab.name.."</color>")
        else
            modelPrefab = LoadManager:LoadAsset(audioName, isInitPath)
            print("<color=yellow>".."加载 Custom 路径 Audio 资源: "..modelPrefab.name.."</color>")
        end
        -------------------------------------------------------

        if modelPrefab ~= nil then
            AudioManager.allAudios[audioName] = modelPrefab
            --print("加载 "..panelName.." 完成")
            return modelPrefab
        else
            print(audioName.." 预设无法加载")
            return nil
        end
    else
       return AudioManager.allAudios[audioName]
    end
end

--销毁音频
function AudioManager:Destroy(audioName)
    for key, value in pairs(AudioManager.allAudios) do
        if key == audioName then
            table.remove(AudioManager.allAudios, AudioManager.allAudios[audioName].index)
        end
    end
end

return AudioManager