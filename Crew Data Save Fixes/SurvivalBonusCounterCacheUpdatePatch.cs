using HarmonyLib;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLServer), "Update")]
    internal class SurvivalBonusCounterCacheUpdatePatch
    {
        static void Postfix(PLServer __instance)
        {
            if (__instance.LatestSaveGameData != null)
            {
                foreach (PLServerClassInfo SCI in __instance.ClassInfos)
                {
                    if (__instance.LatestSaveGameData.ClassData[SCI.ClassID] != null)
                    {
                        __instance.LatestSaveGameData.ClassData[SCI.ClassID].SurvivalBonusCounter = SCI.SurvivalBonusCounter;
                    }
                }
            }
        }
    }
}
