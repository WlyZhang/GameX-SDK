
Utils = {}

--跳转游戏
function Utils:ToGame(gameName)

    CS.Game.ToGame(gameName)
end

--是否开启测试模式
function Utils:enableTest(isTest)

    Utils.isTest = isTest
    CS.Platform.enableTest = isTest
    
    UIManager:Init(isTest)------参数控制加载 【本地==>>true】 或 【网络==>>false】资源
    ModelManager:Init(isTest)
    AudioManager:Init(isTest)
    VideoManager:Init(isTest)
end


--发送消息到 Navite 层

function Utils:sendToNavite(msg)

    if Utils.isTest == true then

        CS.Platform.SendToNaviteTest(msg)
    else
        CS.Platform.SendToNavite(msg)
    end
end


--分割字符串
function Utils:split(input, delimiter)
    
    local arr = CS.GameUtils.Split(input,delimiter)
    return arr
end

--判断字符串是否为空
function Utils:isNull(data)
    return CS.GameUtils.IsNull(data)
end

--判断是否包含字符串
function Utils:isContains(data,str)
    return CS.GameUtils.IsContains(data,str)
end

--设置屏幕转向【true竖屏】【false横屏】
function Utils:setScreen(isPortrait)
    CS.GameUtils.SetScreen(isPortrait)
end


--注册游戏体独立脚本（生命周期）
function Utils:registerObject(go)

    if go ~= nil and go:GetComponent("ObjectBehaviour") == nil then
        go:AddComponent(typeof(CS.ObjectBehaviour))
    else
        print("不能重复注册脚本")
    end
end

--缓存文本文件
function Utils:createTextFile(path,name,info)
    AssetLoadManager:CreateTextFile(path,name,info)
end

--创建Player角色
function Utils:createPlayer()
    ModelManager:CreateModel("Player",false)
end

--发送消息到服务器
function Utils:sendToServer(type,url,key,json,callback)
    Http.Instance:Message(type,url,key,json,callback)
end

--发送消息到Token服务器
function Utils:sendToToken(url,userId,userToken,key,json,callback)
    Http.Instance:MessageToken(url,userId,userToken,key,json,callback)
end

--发送消息到WWWForm服务器
function Utils:sendToForm(url,wwwform,callback)
    Http.Instance:MessageForm(url,wwwform,callback)
end


return Utils;