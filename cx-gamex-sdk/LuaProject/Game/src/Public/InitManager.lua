
--注册实例化脚本类型
function RT(reType)
    local str = _G[reType]
    local reTable = load(str)()

    return reTable
end


InitManager = {}

--默认命名空间
unity = CS.UnityEngine



--C# 管理器
AssetLoadManager = CS.AssetLoadManager
AssetLoadManager.Create()


--Http 网络模块封装
Http = CS.Http
Http.Create()


--语音指令
--Speech = CS.Speech  --语音指令需先设置【SetKeys】在使用【Create】
--Speech.Instance:SetKeys("你好","开始")
--Speech.Instance:Create()




--Lua 管理器
JSON = RT("JSON")
EventDispatcher = RT("EventDispatcher"):getInstance()
Utils = RT("Utils")
Timer = RT("Timer")
LoadManager = RT("LoadManager")
UIBase = RT("UIBase")
UIType = RT("UIType")
UIManager = RT("UIManager")
ModelManager = RT("ModelManager")
AudioManager = RT("AudioManager")
VideoManager = RT("VideoManager")
AppConst = RT("AppConst")



--Lua 管理器设置
LoadManager:Init(AppConst.cachePath,AppConst.cachePath)
Utils:enableTest(false)------是否开启全局测试模式



--数据模型



--Lua UI界面模块





return InitManager
