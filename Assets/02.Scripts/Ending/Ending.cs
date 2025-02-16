using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEditor.SearchService;
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
    private string path;
    private EndingType type;
    private EndingInfo saveData = new EndingInfo();
    void Start()
    {
        type = EndingType.None;
        path = Path.Combine(Application.dataPath, "10.Files", "EndingData.json");
        JsonLoad();
    }

    public void ExcuteEnding(StatusDataSO statusData)
    {
        type =  ClassfyEnding(statusData);
        JsonSave();
        //SceneManager.LoadScene(type.ToString()); //씬이동
    }

    public EndingType ClassfyEnding(StatusDataSO statusData)
    {
        EndingType type = EndingType.Dead;

        if (statusData.RadarOutputAmount == 100)
        {
            type = EndingType.EncounterWithAnAlien;
        }
        else if (statusData.FuelAmount >= 60 && statusData.Corrosion >= 60 && statusData.HullRestorationRate >= 60
            && statusData.MotorRestorationRate >= 60 && statusData.EngineRestorationRate >= 60 && statusData.RadarRestorationRate >= 60
            && statusData.RadarOutputAmount >= 60)
        {
            type = EndingType.MarsMigration;
        }
        else if (statusData.MotorRestorationRate <= 60)
        {
            type = EndingType.PlanetLanding;
        }
        else if(statusData.RadarOutputAmount <= 60)
        {
            type = EndingType.SpaceLost;
        }
        else if(statusData.Corrosion >= 30 && statusData.HullRestorationRate <= 40)
        {
            type = EndingType.SpaceshipExplosion;
        }
        else if(statusData.FuelAmount <= 50)
        {
            type = EndingType.FuelExhaustion;
        }
        else if(statusData.FuelAmount <= 40 && statusData.Corrosion <= 40 && statusData.HullRestorationRate <= 40
            && statusData.MotorRestorationRate <= 40 && statusData.EngineRestorationRate <= 40 && statusData.RadarRestorationRate <= 40
            && statusData.RadarOutputAmount <= 40)
        {
            type = EndingType.Dead;
        }
        return type;
    }

    public void JsonLoad()
    {
        //파일이 존재하면
        if (!File.Exists(path))
        {
            JsonSave();
        }
        else
        {
            //불러오기
            string JsonData = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<EndingInfo>(JsonData);

            if (saveData != null) //saveData에 뭐가 있으면
            {
                //데이터 어디다 저장할까
            }
        }
    }

    public void JsonSave()
    {
        saveData.endingTypes.Add(type);

        //저장하기
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}
