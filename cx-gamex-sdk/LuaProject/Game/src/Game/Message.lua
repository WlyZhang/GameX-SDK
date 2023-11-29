
Message = {}

--分发消息
function Message:switch(args)
    
    local data = JSON:decode(args)
    Message:changeModel(data)
end

--添加或更改模型
function Message:changeModel(data)
    
    local modelName = data.name
    local id = tonumber(data.id)
    --local root = Game:addView(modelName,id,true)
end

return Message