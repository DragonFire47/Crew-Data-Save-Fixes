using PulsarModLoader;

namespace CrewDataSaveFixes
{
    public class Mod : PulsarMod
    {
        public override string Version => "1.1.0";

        public override string Author => "Dragon";

        public override string Name => "Crew Data Save Fixes";

        public override string LongDescription => "Fixes issues related to class data saving.\nSurvival Bonus Counter is no longer overridden by cached save data.\nTalent Points are cached on player leave.\nLeft over talent points get cached.\nInventory Loadouts get cached on player leave.\nAll of the above get saved, even without players.";

        public override string HarmonyIdentifier()
        {
            return $"{Author}.{Name}";
        }
    }
}
/*
//All Saved during game save
//Caches Created on PLGLobal.EnterNewGame
//
//Talents
//Loaded in UpdateNewPlayerTalentsAndInventoryFromSaveData
//
//Patches
// - Saves on player leave
// - Subtracts from saved on spend
// - Adds to Saved on level up
//
//
//
//Survival Bonus Counter
//Loaded in UpdateNewPlayerTalentsAndInventoryFromSaveData.
//Note - Gets increased and decreased properly, but is reloaded every time a player joins.
//
//Patches 
// - Loads during PLGLobal.EnterNewGame
// - Saves SBCs before they get overwritten.
//
//Inventory
//Loaded in UpdateNewPlayerTalentsAndInventoryFromSaveData
//
//Patches
// - Saves on player leave
// - Set to default inventory if count = 0. PlayerDefaultInventoryItemsPatch, SurvivalBonusCounterCacheUpdatePatch
//
//
*/