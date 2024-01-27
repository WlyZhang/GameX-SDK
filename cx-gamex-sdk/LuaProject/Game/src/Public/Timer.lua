Timer ={
	timerTasks={};--延时任务
	timerLoopTasks={};--延时循环任务
	taskIndex=1;--任务编号
};
--设置延时循环函数 id,初次延时,循环后间隔延时,循环时间,循环函数,结束时回调函数
function Timer:SetDelayLoopFunction(delayTime, delayTime1, loopTime, funcLoop,funcOnFinish)
	if self.timerLoopTasks[self.taskIndex] == nil then
		self.timerLoopTasks[self.taskIndex] = {};
	end
	self.timerLoopTasks[self.taskIndex].startTime=CS.UnityEngine.Time.time+delayTime;--触发时机
	self.timerLoopTasks[self.taskIndex].loopDelay=delayTime1;--每次循环的间隔
	self.timerLoopTasks[self.taskIndex].overTime=CS.UnityEngine.Time.time+loopTime+delayTime;--循环持续时间
	self.timerLoopTasks[self.taskIndex].funcLoop=funcLoop;--循环函数
	self.timerLoopTasks[self.taskIndex].funcOnFinish=funcOnFinish;--结束时回调函数
	self.taskIndex=self.taskIndex+1;
	return self.taskIndex-1;
end
--延时函数 id，延时时间，触发方法
function Timer:SetDelayFunction(delayTime, func)
	if self.timerTasks[self.taskIndex] == nil then
		self.timerTasks[self.taskIndex] = {};
	end
	self.timerTasks[self.taskIndex].func = func;
	self.timerTasks[self.taskIndex].startTime = CS.UnityEngine.Time.time + delayTime;
	self.taskIndex=self.taskIndex+1;
	return self.taskIndex-1;
end
--更新
function Timer:Update()
	for k, v in pairs(self.timerTasks) do
		--满足延时条件 触发
		if v.startTime < CS.UnityEngine.Time.time then
			if v.func~=nil then
				v.func();--延时方法
			end
			self.timerTasks[k]=nil;--回收任务
		end
	end
	for k,v in pairs(self.timerLoopTasks) do
		--过期任务
		if v.overTime < CS.UnityEngine.Time.time then
			if v.funcOnFinish~=nil then
				v.funcOnFinish();--循环方法结束回调
			end
			self.timerLoopTasks[k]=nil;--回收任务
		--有效任务
		else
			--满足触发条件
			if v.startTime < CS.UnityEngine.Time.time then
				if v.funcLoop~=nil then
					v.funcLoop();--循环方法
				end
				v.startTime=v.startTime+v.loopDelay;--设置下一次触发时间
			end
		end
	end
end
--撤销任务
function Timer:Remove(id)
	for k,v in pairs(self.timerTasks) do
		if k==id then
			self.timerTasks[id]=nil;
			return;
		end
	end
	for k,v in pairs(self.timerLoopTasks) do
		if k==id then
			self.timerLoopTasks[id]=nil;
			return;
		end
	end
end
--清理
function Timer:Clear()
	for k, v in pairs(self.timerTasks) do
		self.timerTasks[k] = nil;
	end
	for k, v in pairs(self.timerLoopTasks) do
		self.timerLoopTasks[k] = nil;
	end
	self.taskIndex=1;
end

return Timer