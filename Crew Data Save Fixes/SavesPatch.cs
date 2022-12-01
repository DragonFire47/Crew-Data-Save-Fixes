using HarmonyLib;
using PulsarModLoader.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using static PulsarModLoader.Patches.HarmonyHelpers;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLSaveGameIO), "SaveToFile")]
    internal class SavesPatch
    {
        //Write Cached Player Data instead of writing nothing.
        static void PatchMethod(BinaryWriter binaryWriter, int currentClass)
        {
            if (PLServer.Instance.LatestSaveGameData == null)
            {
                binaryWriter.Write(false);
                return;
            }

            binaryWriter.Write(true);
            binaryWriter.Write(PLServer.Instance.LatestSaveGameData.ClassData[currentClass].TalentPointsAvailable);
            binaryWriter.Write(PLServer.Instance.ClassInfos[currentClass].SurvivalBonusCounter);

            int talentcount = PLServer.Instance.LatestSaveGameData.ClassData[currentClass].Talents.Length;
            binaryWriter.Write(talentcount);
            for (int i = 0; i < talentcount; i++)
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
            List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldloc_S, (byte)31),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(SavesPatch), "PatchMethod")),
            };
            var Instructions = instructions.ToList();
            for (int i = 0; i < Instructions.Count; i++)
            {
                if (Instructions[i].opcode == OpCodes.Ldc_I4_0)
                {
                    if (Instructions[i + 1].opcode == OpCodes.Callvirt && (MethodInfo)Instructions[i + 1].operand == AccessTools.Method(typeof(BinaryWriter), "Write", new Type[] { typeof(bool) }))
                    {
                        if (Instructions[i+2].opcode == OpCodes.Ldloc_S && Instructions[i + 3].opcode == OpCodes.Ldc_I4_1 && Instructions[i + 4].opcode == OpCodes.Add)
                        {
                            Instructions.RemoveRange(i, 2);
                            Instructions.InsertRange(i, patchSequence.Select(c => c.FullClone()));
                            break;
                        }
                    }
                }
            }
            return Instructions.AsEnumerable();
            /*List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BinaryWriter), "Write", new Type[] { typeof(bool) })),
            };

            List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldloc_S, (byte)31),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(SavesPatch), "PatchMethod")),
            };
            return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, default, true);*/
        }
    }
}
