
Class = {}

function Class:New(o)
    o = o or {}
    setmetatable(o, {__index = Class})
    return o
end


return Class