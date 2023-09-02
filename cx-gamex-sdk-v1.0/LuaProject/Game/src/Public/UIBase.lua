
UIBase = {}

function UIBase:New(o)
    o = o or {}
    setmetatable(o, {__index = UIBase})
    return o
end

function UIBase:GetType(panelName)
    return UIManager:GetType(panelName)
end

function UIBase:GetPanel(panelName)
    return UIManager:GetPanel(panelName)
end

function UIBase:Open(panelName)
    UIManager:Open(panelName)
end

function UIBase:Close(panelName)
    UIManager:Close(panelName)
end

function UIBase:OpenAndCloseOther(panelName)
    UIManager:OpenAndCloseOther(panelName)
end

return UIBase