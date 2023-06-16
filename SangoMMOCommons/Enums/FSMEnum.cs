//Developer : SangonomiyaSakunovi

namespace SangoMMOCommons.Enums
{
    public enum FSMStateCode
    {
        Null,
        Patrol,
        Chase,
        HilichurlAttack,
    }

    public enum FSMTransitionCode
    {
        Null,
        NoticePlayer,
        LostPlayer,
        ApproachPlayer,
        AwayPlayer
    }
}
