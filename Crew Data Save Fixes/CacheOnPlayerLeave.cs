using HarmonyLib;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLServer), "RemovePlayer")]
    internal class CacheOnPlayerLeave
    {
        static void Prefix(PLPlayer player)
        {
            if (player.TeamID == 0)
            {
                SavesPatch.CachePlayerData(player.GetClassID());
                SavesPatch.CacheSpareTalentPoints(player.GetClassID());
            }
        }
    }
}
