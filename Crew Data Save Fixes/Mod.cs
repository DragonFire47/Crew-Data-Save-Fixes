using PulsarModLoader;

namespace CrewDataSaveFixes
{
    public class Mod : PulsarMod
    {
        public override string Version => "1.0.0";

        public override string Author => "Dragon";

        public override string Name => "Crew Data Save Fixes";

        public override string LongDescription => "Fixes issues related to class data saving.\nSurvival Bonus Counter is no longer overridden by cached save data.\nTalent Points are cached on player leave.\nLeft over talent points get cached.\nInventory Loadouts get cached on player leave.\nAll of the above get saved, even without players.";

        public override string HarmonyIdentifier()
        {
            return $"{Author}.{Name}";
        }
    }
}
