using FantasyOfSango_MMO_Server.FSMs;
using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Bases
{
    public abstract class BaseAI
    {
        protected readonly NPCGameObject NPCobj;

        protected readonly FSMSystem fSMSystem;

        protected readonly ElementSystem elementSystem;

        public BaseAI(NPCGameObject @object, FSMSystem fsmsystem, ElementSystem elementsystem)
        {
            NPCobj = @object;
            fSMSystem = fsmsystem;
            elementSystem = elementsystem;
            InitFSM();
        }

        public abstract void UpdateAI();

        public abstract void InitFSM();

        public abstract void SetDamaged(AvaterCode avater, SkillCode skill, Vector3Position attakerPos);

        public NPCGameObject GetNPCGameObjectInfo() => NPCobj;
    }
}
