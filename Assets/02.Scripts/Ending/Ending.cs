using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EndingType
{ 
    None,
    Dead,
    FuelExhaustion,
    SpaceshipExplosion,
    SpaceLost,
    PlanetLanding,
    MarsMigration,
    EncounterWithAnAlien
}

[Serializable]
public class EndingInfo
{
    public List<EndingType> endingTypes = new List<EndingType>(); 
}

public class Ending : MonoBehaviour
{
    public static EndingInfo SaveData = new EndingInfo();
    public static EndingType Type = EndingType.None;

    private string path;

    void Awake()
    {
        Type = EndingType.None;
        path = Path.Combine(Application.persistentDataPath, "EndingData.json");
        JsonLoad();
    }

    public void ExcuteEnding(StatusDataSO statusData)
    {
        Type =  ClassfyEnding(statusData);
        JsonSave();
        SceneManager.LoadScene(3); //씬이동
    }

    public EndingType ClassfyEnding(StatusDataSO statusData)
    {
        EndingType type = EndingType.Dead;

        //if (statusData.RadarOutputAmount == 100)
        //{
        //    type = EndingType.EncounterWithAnAlien;
        //}
        //else if (statusData.FuelAmount >= 60 && statusData.Corrosion >= 60 && statusData.HullRestorationRate >= 60
        //    && statusData.MotorRestorationRate >= 60 && statusData.EngineRestorationRate >= 60 && statusData.RadarRestorationRate >= 60
        //    && statusData.RadarOutputAmount >= 60)
        //{
        //    type = EndingType.MarsMigration;
        //}
        //else if (statusData.MotorRestorationRate <= 60)
        //{
        //    type = EndingType.PlanetLanding;
        //}
        //else if (statusData.RadarOutputAmount <= 60)
        //{
        //    type = EndingType.SpaceLost;
        //}
        //else if (statusData.Corrosion >= 30 && statusData.HullRestorationRate <= 40)
        //{
        //    type = EndingType.SpaceshipExplosion;
        //}
        //else if (statusData.FuelAmount <= 50)
        //{
        //    type = EndingType.FuelExhaustion;
        //}
        //else if (statusData.FuelAmount <= 40 && statusData.Corrosion <= 40 && statusData.HullRestorationRate <= 40
        //    && statusData.MotorRestorationRate <= 40 && statusData.EngineRestorationRate <= 40 && statusData.RadarRestorationRate <= 40
        //    && statusData.RadarOutputAmount <= 40)
        //{
        //    type = EndingType.Dead;
        //}

        if (statusData.FuelAmount <= 50 && statusData.HullRestorationRate <= 50
    && statusData.MotorRestorationRate <= 50 && statusData.EngineRestorationRate <= 50 && statusData.RadarRestorationRate <= 50
    && statusData.RadarOutputAmount <= 50)
        {
            // Ending 01
            type = EndingType.Dead;
        }
        else if (statusData.FuelAmount >= 70 && statusData.HullRestorationRate >= 70
    && statusData.MotorRestorationRate >= 70 && statusData.EngineRestorationRate >= 70 && statusData.RadarRestorationRate >= 70
    && statusData.RadarOutputAmount >= 70)
        {
            // Ending 06
            type = EndingType.MarsMigration;
        }
        else if (statusData.RadarOutputAmount == 100)
        {
            // Ending 07
            type = EndingType.EncounterWithAnAlien;
        }
        else
        {
            int[] statusValue = new int[4] { statusData.FuelAmount, statusData.HullRestorationRate, statusData.RadarOutputAmount, statusData.MotorRestorationRate };
            int min = statusValue[0];
            int minIndex = 0;

            for(int i= 0; i < statusValue.Length; i++)
            {
                if(statusValue[i] < min)
                {
                    min = statusValue[i];
                    minIndex = i;
                }
            }

            switch (minIndex)
            {
                case 0:
                    // Ending 02
                    type = EndingType.FuelExhaustion;
                    break;
                case 1:
                    // Ending 03
                    type = EndingType.SpaceshipExplosion;
                    break;
                case 2:
                    // Ending 04
                    type = EndingType.SpaceLost;
                    break;
                case 3:
                    // Ending 05
                    type = EndingType.PlanetLanding;
                    break;
            }
        }

        return type;
    }

    public void JsonLoad()
    {
        //파일이 존재하지않으면
        if (!File.Exists(path))
        {
            JsonSave();
        }
        else //파일이 존재하면
        {
            //불러오기
            string JsonData = File.ReadAllText(path);
            SaveData = JsonUtility.FromJson<EndingInfo>(JsonData);

            if (SaveData != null) //saveData에 뭐가 있으면
            {
                //해당 내용을 저장할 변수들


            }
        }
    }

    public void JsonSave()
    {
        foreach(EndingType endingType in SaveData.endingTypes)
        {
            if(endingType == Type)
            {
                return;
            }
        }

        SaveData.endingTypes.Add(Type);

        //저장하기
        string json = JsonUtility.ToJson(SaveData, true);
        File.WriteAllText(path, json);
    }
}
