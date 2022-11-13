using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using static PulsarModLoader.Patches.HarmonyHelpers;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLSaveGameIO), "SaveToFile")]
    internal class SavesPatch
    {
        public static void CachePlayerData(int classID) //Talents, Items
        {
            PLPlayer player = PLServer.Instance.GetCachedFriendlyPlayerOfClass(classID);
            if (player != null && PLServer.Instance.LatestSaveGameData != null)
            {
                PLServer.Instance.LatestSaveGameData.ClassData[classID].Talents = player.Talents;

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
        public static void CacheSpareTalentPoints(int classID) //spent talents
        {
            if (PLServer.Instance.LatestSaveGameData == null)
            {
                return;
            }
            int spentTalents = 0;
            int maxTalentCount = PLServer.Instance.LatestSaveGameData.ClassData[classID].Talents.Length;
            for (int i = 0; i < maxTalentCount; i++)
            {
                spentTalents += PLServer.Instance.LatestSaveGameData.ClassData[classID].Talents[i];
            }
            PLServer.Instance.LatestSaveGameData.ClassData[classID].TalentPointsAvailable = ((PLServer.Instance.CurrentCrewLevel - 1) * 2) - spentTalents;
        }

        static void PatchMethod(BinaryWriter binaryWriter, int currentClass)
		{
            CacheSpareTalentPoints(currentClass);

            binaryWriter.Write(true);
            binaryWriter.Write(PLServer.Instance.LatestSaveGameData.ClassData[currentClass].TalentPointsAvailable);
            binaryWriter.Write(PLServer.Instance.ClassInfos[currentClass].SurvivalBonusCounter);

            int talentcount = PLServer.Instance.LatestSaveGameData.ClassData[currentClass].Talents.Length;
            binaryWriter.Write(talentcount);
            for(int i = 0; i < talentcount; i++)
            {
                binaryWriter.Write(PLServer.Instance.LatestSaveGameData.ClassData[currentClass].Talents[i]);
            }

            int itemcount = PLServer.Instance.LatestSaveGameData.ClassData[currentClass].PawnInventory.Count;
            binaryWriter.Write(itemcount);
            for (int i = 0; i < itemcount; i++)
            {
                PawnItemDataBlock item = PLServer.Instance.LatestSaveGameData.ClassData[currentClass].PawnInventory[i];
                binaryWriter.Write((int)item.ItemType);
                binaryWriter.Write(item.SubType);
                binaryWriter.Write(item.Level);
                binaryWriter.Write(item.OptionalEquipID);
            }
		}

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BinaryWriter), "Write")),
            };

            List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldloc_S, (byte)31),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(SavesPatch), "PatchMethod")),
            };
            return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE);
        }
    }
}
