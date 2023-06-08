using HarmonyLib;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLServer), "GetCDBForPlayer")]
    class SurvivalBonusCounterCachePatch
    {
        static void Postfix(PLPlayer inPlayer, ref ClassDataBlock __result)
        {
            if (__result != null)
            {
                //Sets SurvivalBonusCounter of ClassData before ClassData is written to ServerClassInfo
                __result.SurvivalBonusCounter = PLServer.Instance.ClassInfos[inPlayer.GetClassID()].SurvivalBonusCounter;

                //Runs AttemptedToMove Setter if empty, which flips the bool when set rather than setting it due to a patch. Bool == true => bool == false on the next vanilla set. It will then be treated as starting inv.
                if (__result.PawnInventory.Count == 0)
                {
                    inPlayer.AttemptedToMoveDefaultItemsFromLocker = true;
                }
            }
        }
    }
}
