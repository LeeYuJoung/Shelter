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
        Idel,       // 기본
        Move,       // 이동
        PickUpMove, // 들고 이동
        Work,       // 작업
        Drop,       // 놓다
        Breaking    // 휴식
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

    // 사운드 종류
    public enum SoundType
    {
        Walk = 0,              // 로봇 걷는 소리
        PickUp = 1,            // 로봇이 쓰레기 잡는 소리
        Drop = 2,              // 로봇 쓰레기 내려놓는 소리
        IncineratorWork = 3,   // 소각장에 쓰레기 들어가는 소리
        MachineError = 4,      // 기계 에러 발생 소리
        UIClick = 5,           // UI 클릭 소리
        UIGaze = 6,            // UI 게이지 소리
        Upgrade = 7            // 업그레이드 성공
    }
}