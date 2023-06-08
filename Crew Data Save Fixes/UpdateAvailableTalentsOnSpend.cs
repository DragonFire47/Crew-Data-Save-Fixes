using HarmonyLib;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLPlayer), "ServerRankTalent")]
    internal class UpdateAvailableTalentsOnSpend
    {
        static void Prefix(PLPlayer __instance, int inTalentID)
        {
            if (inTalentID >= 0 && inTalentID < __instance.Talents.Length && PLServer.Instance.GetCachedFriendlyPlayerOfClass(__instance.GetClassID()) == __instance)
            {
                TalentInfo talentInfoForTalentType = PLGlobal.GetTalentInfoForTalentType((ETalents)inTalentID);
                if (talentInfoForTalentType != null && __instance.Talents[inTalentID] < talentInfoForTalentType.MaxRank && __instance.TalentPointsAvailable > 0)
                {
                    PLServer.Instance.LatestSaveGameData.ClassData[__instance.GetClassID()].TalentPointsAvailable -= 1;
                }
            }
        }
    }
}
