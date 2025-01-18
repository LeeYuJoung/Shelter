using UnityEngine;

[CreateAssetMenu(fileName = "StatusDataSO", menuName = "Scriptable Objects/StatusDataSO")]
public class StatusDataSO : ScriptableObject
{
    public int FuelAmount; //���ᷮ
    public int Corrosion; //�νĵ�
    public int HullRestorationRate; //��ü ������
    public int MotorRestorationRate; //���±� ������
    public int EngineRestorationRate; //���� ������
    public int RadarRestorationRate; //���̴� ������
    public int RadarOutputAmount; //���̴� ��·�
    
}
