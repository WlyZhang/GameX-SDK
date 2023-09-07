
ModelManager = {}

--初始化
function ModelManager:Init(isLoadLocal)
    ModelManager.isLoadLocal = isLoadLocal
    ModelManager.allModels = {}

    if isLoadLocal then
        print("准备加载 Resources 资源")
    else
        print("准备加载 Custom 资源")
    end
end

--创建模型
function ModelManager:CreateModel(modelName,isInitPath)

    LoadManager:SwitchDir("/model/")

    local prefab = ModelManager:Load(modelName, isInitPath)
    local model = unity.GameObject.Instantiate(prefab)
    model.transform.parent = unity.GameObject.Find(AppConst.moduleName).transform

    return model
end

--加载模型
function ModelManager:Load(modelName, isInitPath)

    if ModelManager.allModels[modelName] == nil then

        local modelPrefab = nil
        --是否加载本地资源--------------------------------------
        if ModelManager.isLoadLocal then
            modelPrefab = unity.Resources.Load("Model/"..modelName)
            print("<color=yellow>".."加载 Resources Model 资源: "..modelPrefab.name.."</color>")
        else
            modelPrefab = LoadManager:LoadAsset(modelName, isInitPath)
            print("<color=yellow>".."加载 Custom 路径 Model 资源: "..modelPrefab.name.."</color>")
        end
        -------------------------------------------------------

        if modelPrefab ~= nil then
            ModelManager.allModels[modelName] = modelPrefab
            --print("加载 "..panelName.." 完成")
            return modelPrefab
        else
            print(modelName.." 预设无法加载")
            return nil
        end
    else
       return ModelManager.allModels[modelName]
    end
end


--销毁模型
function ModelManager:Destroy(modelName)
    for key, value in pairs(ModelManager.allModels) do
        if key == modelName then
            table.remove(ModelManager.allModels, ModelManager.allModels[modelName].index)
        end
    end
end

return ModelManager