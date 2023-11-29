
UIManager = {}

--初始化
function UIManager:Init(isLoadLocal)

    UIManager.isLoadLocal = isLoadLocal
    UIManager.allPage = {}
    UIManager.runPage = {}
    UIManager.typePage = {}
    UIManager.canvas=unity.GameObject.Find("Canvas").transform
    unity.GameObject.DontDestroyOnLoad(UIManager.canvas.gameObject)

    UIType:Register()
end

function UIManager:Register(panelName, o)
    UIManager.typePage[panelName] = o
end

--获取类型
function UIManager:GetType(panelName)

    if UIManager.typePage[panelName] ~= nil then
        return UIManager.typePage[panelName]
    else
        error("没有 "..panelName.." 这个类型")
        return nil
    end
end

--获取面板
function UIManager:GetPanel(panelName)

    if UIManager.runPage[panelName] ~= nil then
        return UIManager.runPage[panelName]
    else
        error("没有 "..panelName.." 这个面板")
        return nil
    end
end

--打开面板
function UIManager:Open(panelName)

    local isHas = false

    if UIManager.runPage[panelName] ~= nil then
        isHas = true
    end

    --[[for key, value in pairs(UIManager.runPage) do
        if key == panelName then
            isHas = true
            break
        end
    end]]--

    if isHas then
        UIManager.runPage[panelName]:SetActive(true)
    else
        local prefab = UIManager:Load(panelName)
        local panel = unity.GameObject.Instantiate(prefab,UIManager.canvas)
        UIManager.runPage[panelName] = panel
    end

    return UIManager.runPage[panelName]
end

--关闭面板
function UIManager:Close(panelName)

    if UIManager.runPage[panelName] ~= nil then
        UIManager.runPage[panelName]:SetActive(false)
    else
        error(panelName.." 预设为空(nil)")
    end
end

--互斥面板
function UIManager:OpenAndCloseOther(panelName)

    local openPanel = nil

    for key, value in pairs(UIManager.runPage) do
        UIManager:Close(key)
    end

    openPanel = UIManager:Open(panelName)
    return openPanel
end

--加载面板
function UIManager:Load(panelName)

    LoadManager:SwitchDir("/uipanel/")

    if UIManager.allPage[panelName] == nil then

        local panelPrefab = nil
        --是否加载本地资源--------------------------------------
        if UIManager.isLoadLocal then
            panelPrefab = unity.Resources.Load("UI/"..panelName)
            print("<color=yellow>".."加载本地 UI 资源: "..panelPrefab.name.."</color>")
        else
            panelPrefab = LoadManager:LoadAsset(panelName)
            print("<color=yellow>".."加载网络 UI 资源: "..panelPrefab.name.."</color>")
        end
        -------------------------------------------------------

        if panelPrefab ~= nil then
            UIManager.allPage[panelName] = panelPrefab
            --print("加载 "..panelName.." 完成")
            return panelPrefab
        else
            print(panelName.." 预设无法加载")
            return nil
        end
    else
       return UIManager.allPage[panelName]
    end
end

--销毁面板
function UIManager:Destroy(panelName)
    for key, value in pairs(UIManager.runPage) do
        if key == panelName then
            table.remove(UIManager.runPage, UIManager.runPage[panelName].index)
            unity.GameObject.Destroy(value)
        end
    end

    for key, value in pairs(UIManager.allPage) do
        if key == panelName then
            table.remove(UIManager.allPage, UIManager.allPage[panelName].index)
        end
    end
end

return UIManager