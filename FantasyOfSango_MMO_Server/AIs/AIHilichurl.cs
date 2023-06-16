using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.FSMs;
using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Constants;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.AIs
{
    public class AIHilichurl : BaseAI
    {
        private Vector3Position path1 = new Vector3Position(-41.90256f, 6.667f, 123.9594f);
        private Vector3Position path2 = new Vector3Position(-71.56f, 12.49f, 150.52f);
        private Vector3Position path3 = new Vector3Position(-72.574f, 10.009f, 133.421f);

        public AIHilichurl(NPCGameObject obj) : this(obj, new FSMSystem(obj.NPCCode), new ElementSystem(obj.NPCCode)) { }

        public AIHilichurl(NPCGameObject @object, FSMSystem fsm, ElementSystem ele) : base(@object, fsm, ele) { }

        public override void InitFSM()
        {
            List<Vector3Position> pathList = new List<Vector3Position> { path1, path2, path3 };
            FSMState patrolState = new NPCPatrolState(fSMSystem, EnemyConstant.HilichurlPatrolToChaseDis, pathList, EnemyConstant.HilichurlPatrolSpeed, -1);
            FSMState chaseState = new NPCChaseState(fSMSystem, EnemyConstant.HilichurlChaseToPatrolDis, EnemyConstant.HilichurlChaseToAttackDis, EnemyConstant.HilichurlChaseSpeed);
            FSMState hilichurlAttackState = new HilichurlAttackState(fSMSystem, EnemyConstant.HilichurlAttackToChaseDis);
            fSMSystem.AddFSMState(patrolState);
            fSMSystem.AddFSMState(chaseState);
            fSMSystem.AddFSMState(hilichurlAttackState);
            patrolState.AddFSMTransition(FSMTransitionCode.NoticePlayer, FSMStateCode.Chase);
            chaseState.AddFSMTransition(FSMTransitionCode.LostPlayer, FSMStateCode.Patrol);
            chaseState.AddFSMTransition(FSMTransitionCode.ApproachPlayer, FSMStateCode.HilichurlAttack);
            hilichurlAttackState.AddFSMTransition(FSMTransitionCode.AwayPlayer, FSMStateCode.Chase);
        }

        public override void UpdateAI()
        {
            fSMSystem.UpdateFSM(this.NPCobj);
            elementSystem.Update();
        }

        public override void SetDamaged(AvaterCode avater, SkillCode skill, Vector3Position attakerPos)
        {
            //Just for test!!!
            if (skill == SkillCode.ElementAttack)
            {
                ElementApplication elementApplicationCache = null;
                if (avater == AvaterCode.SangonomiyaKokomi)   //KokomiAttack with Hydro
                {
                    elementApplicationCache = new ElementApplication(ElementTypeCode.Hydro, 2);
                }
                else if (avater == AvaterCode.Yoimiya)   //YoimiyaAttack with Pyro
                {
                    elementApplicationCache = new ElementApplication(ElementTypeCode.Pyro, 2);
                }
                else if (avater == AvaterCode.Ayaka)
                {
                    elementApplicationCache = new ElementApplication(ElementTypeCode.Cryo, 2);
                }
                this.elementSystem.ElementReaction(FightTypeCode.PVE, skill, elementApplicationCache, attakerPos, NPCobj._id);
            }
        }
    }
}
