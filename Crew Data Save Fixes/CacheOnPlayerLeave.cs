using HarmonyLib;
using System.Collections.Generic;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLServer), "RemovePlayer")]
    internal class CacheOnPlayerLeave
    {
        static void CachePlayerData(PLPlayer inPlayer) //Talents, Items
        {
            if (inPlayer.TeamID != 0 || !(PLServer.Instance.LatestSaveGameData != null && PLServer.Instance.LatestSaveGameData.ClassData[inPlayer.GetClassID()] != null))//stop if not player team
            {
                return;
            }

            int classID = inPlayer.GetClassID();
            PLPlayer player = PLServer.Instance.GetCachedFriendlyPlayerOfClass(classID);
            if (player != null && classID > -1 && classID < 5)
            {
                PLServer.Instance.LatestSaveGameData.ClassData[classID].Talents = player.Talents;
                PLServer.Instance.LatestSaveGameData.ClassData[classID].TalentPointsAvailable = player.TalentPointsAvailable;

                List<PawnItemDataBlock> pawnItemData = new List<PawnItemDataBlock>();
                for (int i = 0; i < player.MyInventory.AllItems.Count; i++)
                {
                    PLPawnItem item = player.MyInventory.AllItems[i];
                    pawnItemData.Add(new PawnItemDataBlock()
                    {
                        ItemType = item.PawnItemType,
                        SubType = item.SubType,
                        Level = item.Level,
                        OptionalEquipID = item.EquipID
                    });
                }
                PLServer.Instance.LatestSaveGameData.ClassData[classID].PawnInventory = pawnItemData;
            }
        }
        static void Prefix(PLPlayer inPlayer)
        {
            CachePlayerData(inPlayer);
        }
    }
}
