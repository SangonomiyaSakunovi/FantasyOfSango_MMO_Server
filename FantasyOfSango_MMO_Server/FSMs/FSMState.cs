using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.FSMs
{
    public abstract class FSMState
    {
        protected FSMStateCode _stateCode;

        public FSMStateCode StateCode { get { return _stateCode; } }
        protected Dictionary<FSMTransitionCode, FSMStateCode> fSMMappingDict = new Dictionary<FSMTransitionCode, FSMStateCode>();

        protected FSMSystem fSMSystem;

        public FSMState(FSMSystem system)
        {
            this.fSMSystem = system;
        }

        public void AddFSMTransition(FSMTransitionCode fSMTransitionCode, FSMStateCode fSMStateCode)
        {
            if (fSMMappingDict.ContainsKey(fSMTransitionCode))
            {
                return;
            }
            fSMMappingDict.Add(fSMTransitionCode, fSMStateCode);
        }

        public void DeletFSMTransition(FSMTransitionCode fSMTransitionCode)
        {
            if (!fSMMappingDict.ContainsKey(fSMTransitionCode))
            {
                return;
            }
            fSMMappingDict.Remove(fSMTransitionCode);
        }

        public FSMStateCode GetFSMStateCode(FSMTransitionCode fSMTransitionCode)
        {
            if (fSMMappingDict.ContainsKey(fSMTransitionCode))
            {
                return fSMMappingDict[fSMTransitionCode];
            }
            else
            {
                return FSMStateCode.Null;
            }
        }

        public virtual void DoBeforeEntering() { }
        public virtual void DoAfterExiting() { }
        public abstract void Act(NPCGameObject npc);
        public abstract void Reason(NPCGameObject npc);
    }
}
