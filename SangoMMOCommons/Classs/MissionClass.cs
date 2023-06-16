using SangoMMOCommons.Enums;
using System;

//Developer : SangonomiyaSakunovi

namespace SangoMMOCommons.Classs
{
    [Serializable]
    public class MissionUpdateReq
    {
        public string MissionId { get; set; }
        public MissionTypeCode MissionTypeCode { get; set; }
        public MissionUpdateTypeCode MissionUpdateTypeCode { get; set; }
    }

    [Serializable]
    public class MissionUpdateRsp
    {
        public string MissionId { get; set; }
        public bool IsCompleteSuccess { get; set; }
        public MissionTypeCode MissionTypeCode { get; set; }
        public MissionUpdateTypeCode MissionUpdateTypeCode { get; set; }
    }
}
