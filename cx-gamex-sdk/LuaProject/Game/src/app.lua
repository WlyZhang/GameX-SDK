
local initStr = _G.InitManager
local InitManager = load(initStr)()

Utils:setScreen(true)


--检测版本
VersionData:GetNewVersion(1,function (res)
    FirstPanel:Create()
end)
