namespace EnumTypes
{
    // �κ� ����
    public enum RobotType
    {
        Collector, // ���� �κ�
        Sweeper    // û�� �κ�
    }

    // �κ� ����
    public enum RobotState
    {
        Idel,     // �⺻
        Move,     // �̵�
        Work,     // �۾�
        breaking  // �޽�
    }

    // ���ּ� ��� ����
    public enum MachineType
    {
        Incinerator,  // ������ �Ұ���
        GasTank,      // ������
        Motor,        // ���±�
        engine,       // ����
        Radar         // ���̴�
    }
}