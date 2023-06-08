using HarmonyLib;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLPlayer), "set_AttemptedToMoveDefaultItemsFromLocker")]
    internal class PlayerDefaultInventoryItemsPatch
    {
        //Changes DefaultItemsMoved bool setter to a flipper.
        static bool Prefix(ref bool ___m_AttemptedToMoveDefaultItemsFromLocker)
        {
            ___m_AttemptedToMoveDefaultItemsFromLocker = !___m_AttemptedToMoveDefaultItemsFromLocker;
            return false;
        }
    }
}
