using HarmonyLib;
using PulsarModLoader.Utilities;
using System;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLServer), "OnCrewLevelUp")]
    internal class UpdateAvailableTalentsOnLevelUp
    {
        static void Postfix(PLServer __instance)
        {
            if (__instance.LatestSaveGameData == null)
            {
                return;
            }
            try
            {
                foreach (ClassDataBlock CDB in __instance.LatestSaveGameData.ClassData)
                {
                    if (CDB != null)
                    {
                        CDB.TalentPointsAvailable += 2;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Info("Failed to add points to classDataBlocks.\n" + ex.Message);
            }
        }
    }
}
