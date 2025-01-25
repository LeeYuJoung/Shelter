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
    public void SetMotorRestorationRate()
    {
        statusData.MotorRestorationRate += statusUpData.MotorRestorationRate;
    }
    public void SetEngineRestorationRate()
    {
        statusData.EngineRestorationRate += statusUpData.EngineRestorationRate;
    }
    public void SetRadarRestorationRate()
    {
        statusData.RadarRestorationRate += statusUpData.RadarRestorationRate;
    }
    public void SetRadarOutputAmount()
    {
        statusData.RadarOutputAmount += statusUpData.RadarOutputAmount;
    }
}
