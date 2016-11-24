require 'modkit'

-- Base Lua file

local baseBanditHealth = 70
local baseBanditAttackPower = 3.5

function Mod:onInit(...)
  print(baseBanditAttackPower)
  print(Game:calcHypotenuse(3, 2))
end

function handler(o, a)
	print('tick!', o, a);
end

Hooks.onGameTick.add(handler);
Hooks.doTest();

-- look into delegate C#
