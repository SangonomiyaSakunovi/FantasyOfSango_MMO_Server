using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.FSMs
{
    public class NPCPatrolState : FSMState
    {
        private List<Vector3Position> PathList = new List<Vector3Position>();
        private float PatrolSpeed;
        private float PatrolToChaseDis;
        private float NPCToPatrolTargetDis;
        private int index = 0;
        public NPCPatrolState(FSMSystem fSMSystem, float patrolToChaseDis, List<Vector3Position> path, float speed, float npcToPatrolTargetDis) : base(fSMSystem)
        {
            _stateCode = FSMStateCode.Patrol;
            PathList = path;
            PatrolSpeed = speed;
            PatrolToChaseDis = patrolToChaseDis;
            NPCToPatrolTargetDis = npcToPatrolTargetDis;
        }
        public override void Act(NPCGameObject npc)
        {

        }

        public override void Reason(NPCGameObject npc)
        {
            ClientPeer targetPeer = NPCSystem.Instance.FindOnlineClientPeerAsTarget(fSMSystem.NPCCode, PatrolToChaseDis);
            if (targetPeer != null)
            {
                fSMSystem.SetTargetClientPeer(targetPeer);
                fSMSystem.OnFSMTransition(FSMTransitionCode.NoticePlayer);
            }
        }
    }

    public class NPCChaseState : FSMState
    {
        private float ChaseSpeed;
        private float ChaseToPatrolDis;
        private float ChaseToAttackDis;
        public NPCChaseState(FSMSystem fSMSystem, float chaseToPatrolDis, float chaseToAttackDis, float speed) : base(fSMSystem)
        {
            _stateCode = FSMStateCode.Chase;
            ChaseSpeed = speed;
            ChaseToPatrolDis = chaseToPatrolDis;
            ChaseToAttackDis = chaseToAttackDis;
        }
        public override void Act(NPCGameObject npc)
        {

        }

        public override void Reason(NPCGameObject npc)
        {
            if (NPCSystem.Instance.IsApproachOnlineClientPeerTarget(fSMSystem.NPCCode, fSMSystem.TargetClienPeer, ChaseToAttackDis))
            {
                fSMSystem.OnFSMTransition(FSMTransitionCode.ApproachPlayer);
            }
            if (NPCSystem.Instance.IsLostOnlineClientPeerTarget(fSMSystem.NPCCode, fSMSystem.TargetClienPeer, ChaseToPatrolDis))
            {
                fSMSystem.RemoveTargetClientPeer();
                fSMSystem.OnFSMTransition(FSMTransitionCode.LostPlayer);
            }
        }
    }

    public class HilichurlAttackState : FSMState
    {
        private float AttackToChaseDis;

        public HilichurlAttackState(FSMSystem fSMSystem, float attackToChaseDis) : base(fSMSystem)
        {
            _stateCode = FSMStateCode.HilichurlAttack;
            AttackToChaseDis = attackToChaseDis;
        }
        public override void Act(NPCGameObject npc)
        {

        }

        public override void Reason(NPCGameObject npc)
        {
            if (NPCSystem.Instance.IsAwayOnlienClientPeerTarget(fSMSystem.NPCCode, fSMSystem.TargetClienPeer, AttackToChaseDis))
            {
                fSMSystem.OnFSMTransition(FSMTransitionCode.AwayPlayer);
            }
        }
    }
}
