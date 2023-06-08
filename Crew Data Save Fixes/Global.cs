using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;

namespace CrewDataSaveFixes
{
    internal class Global
    {
        public static void InitSaveGameData()
        {
            if (PLServer.Instance.LatestSaveGameData == null)
            {
                InitLatestSaveGameData();
            }
            else
            {
                InitCDBs();
            }
        }

        static void InitLatestSaveGameData()
        {
            SaveGameData SGD = new SaveGameData();
            SGD.ClassData = new ClassDataBlock[5];

            for (int i = 0; i < 5; i++)
            {
                SGD.ClassData[i] = InitCDB();
            }

            PLServer.Instance.LatestSaveGameData = SGD;
        }

        static void InitCDBs()
        {
            for (int i = 0; i < 5; i++)
            {
                if (PLServer.Instance.LatestSaveGameData.ClassData[i] == null)
                {
                    PLServer.Instance.LatestSaveGameData.ClassData[i] = InitCDB();
                }
            }
        }

        static ClassDataBlock InitCDB()
        {
            ClassDataBlock CDB = new ClassDataBlock();
            CDB.TalentPointsAvailable = 0;
            CDB.SurvivalBonusCounter = 0;
            CDB.Talents = new ObscuredInt[64];
            CDB.PawnInventory = new List<PawnItemDataBlock>();

            return CDB;
        }
    }
}
