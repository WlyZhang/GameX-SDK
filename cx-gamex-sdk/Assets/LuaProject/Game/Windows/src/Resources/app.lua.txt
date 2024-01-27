
local initStr = _G.InitManager
local InitManager = load(initStr)()

Utils:setScreen(false)


Speaker.Speak("你好，欢迎来到辰星互动！")

ModelManager:CreateModel("Cube",false)