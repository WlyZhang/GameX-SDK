
Game = {}
Game.modelList = {}

--添加 View
function Game:addCommandView(path, name, index)

    if Game[name] == nil then
        AssetLoadManager.Load("asset",path,name,function (obj)

            local root = unity.GameObject.Instantiate(obj)
            Game[name] = root

            print("加载网络资源")

            --测试
            root.transform.position = unity.Vector3(index*100,0,0)
    
            local cam = root.transform:Find("Camera"):GetComponent("Camera")
            Game:displayChanged(index, cam)
        end)
    else
        
        local root = unity.GameObject.Instantiate(Game[name])

        print("加载本地缓存资源")

        --测试
        root.transform.position = unity.Vector3(index*100,0,0)

        local cam = root.transform:Find("Camera"):GetComponent("Camera")
        Game:displayChanged(index, cam)
    end
end


--添加 View
function Game:addView(name, id, isInitPath)

    local root = nil

    if Game.modelList[id] == nil then

        root = ModelManager:CreateModel(name, isInitPath)
        Game.lastId = id
        Game.modelList[id] = root
        print("111")

    else
        if Game.lastId == id then

            unity.GameObject.Destroy(Game.modelList[id])

            root = ModelManager:CreateModel(name, isInitPath)
            Game.lastId = id
            Game.modelList[id] = root

        end
    end

    --测试
    root.transform.position = unity.Vector3(id*100,0,0)

    local cam = root.transform:Find("Camera"):GetComponent("Camera")
    Game:displayChanged(id, cam)

    return root
end


--获取场景View
function Game:getView(camName, index)

    local cam = unity.GameObject.Find(camName):GetComponent("Camera")
    Game:displayChanged(index, cam)
end


--更改 View 渲染
function Game:displayChanged(index, cam)

    local camList = unity.Camera.allCameras

    for i=0,camList.Length-1 do
        if camList[i] == cam then
            camList[i].depth = 10
        else
            camList[i].depth = 0
        end
    end

    print(cam.gameObject.name.." --> "..unity.Display.displays.Length)
    if index < unity.Display.displays.Length then
        unity.Display.displays[index]:SetRenderingResolution(256, 256);
        cam:SetTargetBuffers(unity.Display.displays[index].colorBuffer, unity.Display.displays[index].depthBuffer);
    end
end

return Game