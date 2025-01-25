using UnityEngine;

[CreateAssetMenu(fileName = "StatusDataSO", menuName = "Scriptable Objects/StatusDataSO")]
public class StatusDataSO : ScriptableObject
{
    public int FuelAmount; //연료량
    public int Corrosion; //부식도
    public int HullRestorationRate; //선체 복구도
    public int MotorRestorationRate; //동력기 복구도
    public int EngineRestorationRate; //엔진 복구도
    public int RadarRestorationRate; //레이더 복구도
    public int RadarOutputAmount; //레이더 출력량 
}
