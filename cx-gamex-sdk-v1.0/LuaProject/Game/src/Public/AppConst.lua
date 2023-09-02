
AppConst = {}

AppConst.platform = CS.AppConst.platform

--游戏模块名称
AppConst.moduleName = CS.Game.ModuleName

--StreamingAssets路径
AppConst.streamPath = unity.Application.streamingAssetsPath.."/"..AppConst.moduleName.."/"..AppConst.platform.."/AssetBundles"

--沙盒缓存路径
AppConst.cachePath = unity.Application.persistentDataPath.."/"..AppConst.moduleName.."/"..AppConst.platform.."/AssetBundles"

--资源服务器路径
AppConst.commandPath = "http://192.168.1.104/"..AppConst.moduleName.."/"..AppConst.platform.."/AssetBundles"





return AppConst