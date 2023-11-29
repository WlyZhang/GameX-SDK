
UIType = {}

function UIType:Register()

    --开始面板
    UIManager:Register("FirstPanel", FirstPanel)

    --登录注册
    UIManager:Register("LoginPanel", LoginPanel)

    --游戏面板
    UIManager:Register("GamePanel", GamePanel)

    --消息弹窗
    UIManager:Register("MessagePanel", MessagePanel)

end


return UIType