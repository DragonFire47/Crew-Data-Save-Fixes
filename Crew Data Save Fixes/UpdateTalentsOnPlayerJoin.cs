using HarmonyLib;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLServer), "GetCDBForPlayer")]
    internal class UpdateTalentsOnPlayerJoin
    {
        static void Postfix(PLPlayer inPlayer)
        {
            if(inPlayer.TeamID == 0)
            {
                SavesPatch.CacheSpareTalentPoints(inPlayer.GetClassID());
            }
        }
    }
}
