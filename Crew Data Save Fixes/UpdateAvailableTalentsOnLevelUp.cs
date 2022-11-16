using HarmonyLib;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLServer), "OnCrewLevelUp")]
    internal class UpdateAvailableTalentsOnLevelUp
    {
        static void Postfix(PLServer __instance)
        {
            if(__instance.LatestSaveGameData == null)
            {
                return;
            }
            foreach (ClassDataBlock CDB in __instance.LatestSaveGameData.ClassData)
            {
                if (CDB != null)
                {
                    CDB.TalentPointsAvailable += 2;
                }
            }
        }
    }
}
