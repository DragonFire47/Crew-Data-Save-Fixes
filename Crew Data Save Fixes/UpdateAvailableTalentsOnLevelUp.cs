using HarmonyLib;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLServer), "OnCrewLevelUp")]
    internal class UpdateAvailableTalentsOnLevelUp
    {
        //Increases all CDB TalentPointsAvailable on crew level up.
        static void Postfix(PLServer __instance)
        {
            foreach (ClassDataBlock CDB in __instance.LatestSaveGameData.ClassData)
            {
                CDB.TalentPointsAvailable += 2;
            }
        }
    }
}
