using UnityEngine;

public class Status
{
    [SerializeField] private StatusDataSO statusData;
    [SerializeField] private StatusDataSO statusUpData;

    public void SetFuelAmount()
    {
        statusData.FuelAmount += statusUpData.FuelAmount;
    }
    public void SetCorrosion()
    {
        statusData.Corrosion += statusUpData.Corrosion;
    }
    public void SetHullRestorationRate()
    {
        statusData.HullRestorationRate += statusUpData.HullRestorationRate;
    }
    public void SetMotorRestorationRate(int value)
    {
        statusData.MotorRestorationRate += statusUpData.MotorRestorationRate;
    }
    public void SetEngineRestorationRate(int value)
    {
        statusData.EngineRestorationRate += statusUpData.EngineRestorationRate;
    }
    public void SetRadarRestorationRate(int value)
    {
        statusData.RadarRestorationRate += statusUpData.RadarRestorationRate;
    }
    public void SetRadarOutputAmount(int value)
    {
        statusData.RadarOutputAmount += statusUpData.RadarOutputAmount;
    }
}
