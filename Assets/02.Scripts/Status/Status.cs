using UnityEngine;

public class Status : MonoBehaviour
{
    public StatusDataSO statusData;
    public StatusDataSO statusUpData;

    public void SetFuelAmount(bool IsIncrese)
    {
        if(IsIncrese)
        {
            statusData.FuelAmount += statusUpData.FuelAmount;
        }
        else
        {
            statusData.FuelAmount -= statusUpData.FuelAmount;
        }
    }
    public void SetCorrosion(bool IsIncrese)
    {
        if (IsIncrese)
        {
            statusData.Corrosion += statusUpData.Corrosion;
        }
        else
        {
            statusData.Corrosion -= statusUpData.Corrosion;
        }
    }
    public void SetHullRestorationRate(bool IsIncrese)
    {
        if (IsIncrese)
        {
            statusData.HullRestorationRate += statusUpData.HullRestorationRate;
        }
        else
        {
            statusData.HullRestorationRate -= statusUpData.HullRestorationRate;
        }
    }
    public void SetMotorRestorationRate(bool IsIncrese)
    {
        if (IsIncrese)
        {
            statusData.MotorRestorationRate += statusUpData.MotorRestorationRate;
        }
        else
        {
            statusData.MotorRestorationRate -= statusUpData.MotorRestorationRate;
        }
    }
    public void SetEngineRestorationRate(bool IsIncrese)
    {
        if (IsIncrese)
        {
            statusData.EngineRestorationRate += statusUpData.EngineRestorationRate;
        }
        else
        {
            statusData.EngineRestorationRate -= statusUpData.EngineRestorationRate;
        }
    }
    public void SetRadarRestorationRate(bool IsIncrese)
    {
        if (IsIncrese)
        {
            statusData.RadarRestorationRate += statusUpData.RadarRestorationRate;
        }
        else
        {
            statusData.RadarRestorationRate -= statusUpData.RadarRestorationRate;
        }
    }
    public void SetRadarOutputAmount(bool IsIncrese)
    {
        if (IsIncrese)
        {
            statusData.RadarOutputAmount += statusUpData.RadarOutputAmount;
        }
        else
        {
            statusData.RadarOutputAmount -= statusUpData.RadarOutputAmount;
        }
    }
}
