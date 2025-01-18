using UnityEngine;

public class Status
{
    [SerializeField] private StatusDataSO statusData;

    public void SetFuelAmount(int value)
    {
        statusData.FuelAmount += value;
    }
    public void SetCorrosion(int value)
    {
        statusData.Corrosion += value;
    }
    public void SetHullRestorationRate(int value)
    {
        statusData.HullRestorationRate += value;
    }
    public void SetMotorRestorationRate(int value)
    {
        statusData.MotorRestorationRate += value;
    }
    public void SetEngineRestorationRate(int value)
    {
        statusData.EngineRestorationRate += value;
    }
    public void SetRadarRestorationRate(int value)
    {
        statusData.RadarRestorationRate += value;
    }
    public void SetRadarOutputAmount(int value)
    {
        statusData.RadarOutputAmount += value;
    }
}
