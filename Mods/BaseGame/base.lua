require 'modkit'
require 'buildings'
require 'animals'

-- Base Lua file
-- Below stuff is for proof of concept testing on how this mod works, check console output in game

local baseBanditHealth = 50
local baseBanditAttackPower = 3.5

-- defineEnemy { name = "Bandit", health = baseBanditHealth, attackPower = baseBanditAttackPower, }

function handler(o, a)
	print('handled!', o, a);
end
Hooks.onTest.add(handler);

function Mod:onInit(...)
  print(baseBanditAttackPower)
  print(Game:calcHypotenuse(3, 2))
  Game.multiplier = 4;
end
