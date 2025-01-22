namespace EnumTypes
{
    // 로봇 종류
    public enum RobotType
    {
        Collector, // 수집 로봇
        Sweeper    // 청소 로봇
    }

    // 로봇 상태
    public enum RobotState
    {
        Idel,     // 기본
        Move,     // 이동
        Work,     // 작업
        Drop,     // 놓다
        Breaking  // 휴식
    }

    // 우주선 기계 종류
    public enum MachineType
    {
        Incinerator,  // 쓰레기 소각장
        GasTank,      // 연료통
        Motor,        // 동력기
        engine,       // 엔진
        Radar         // 레이더
    }
}