using HarmonyLib;

namespace CrewDataSaveFixes
{
    [HarmonyPatch(typeof(PLGlobal), "EnterNewGame")]
    class GlobalEnterNewGamePatch
    {
        static void Postfix()
        {
            //Should fix all null ClassDatas.
            Global.InitSaveGameData();

            //Loads SBCs on game start
            for (int i = 0; i < 5; i++)
            {
                PLServer.Instance.ClassInfos[i].SurvivalBonusCounter = PLServer.Instance.LatestSaveGameData.ClassData[i].SurvivalBonusCounter;
            }
        }
    }
}
