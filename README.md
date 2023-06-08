# Crew Data Save Fixes

This mod was developed to fix bugs related to crew save data in Pulsar: Lost Colony. Currently Talents, Survival Bonus Health, and Player Inventory Loadouts are saved per class IF a player is currently playing as the class. When a game save is loaded, this data is cached and applied to new players as they join, and it does not update.  

There are issues where joining players get the health, talents, and inventory loadouts from the last time the game was saved then loaded. Often the crew has leveled up, but the joining player still has the total talent points from many levels in the past. They also get the bonus health of the last player, which could be a good thing (already maxed), but often is undesirable as the player had negative health. If the player successfully fixes these issues then leaves (or loses connection), upon rejoining they are back to ground zero.

## Information:
For Game Version 1.2.04  
For PML Version 0.11.1  
Mod Version 1.1.0  
Developed by: Dragon
Host/Client Requirements: Client

Support the developer: https://www.patreon.com/DragonFire47


## Features:
- Prevents the survival bonus counter from being overridden by cached save data.
- Caches talent points on player leave.
- Caches unspent talent points.
- Caches inventory loadouts on player leave.
- Saves all of the above, even without players.


## To install:  
- Have PulsarModLoader installed  
- Go to \PULSARLostColony\Mods  
- Add the .dll included with this package
