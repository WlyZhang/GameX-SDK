
Class = {}

function Class:New(obj)
    local o={}
    obj.__index=obj
    setmetatable(o,obj)
    return o
end


return Class