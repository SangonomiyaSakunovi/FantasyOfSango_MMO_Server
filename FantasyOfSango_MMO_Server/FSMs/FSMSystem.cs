using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.FSMs
{
    public class FSMSystem
    {
        public NPCCode NPCCode { get; private set; }
        public ClientPeer TargetClienPeer { get; private set; }

        private Dictionary<FSMStateCode, FSMState> fSMStateDict = new Dictionary<FSMStateCode, FSMState>();
        private FSMStateCode currentfSMStateCode;
        private FSMState currentfSMState;

        public FSMSystem(NPCCode npcCode)
        {
            NPCCode = npcCode;
        }

        public void UpdateFSM(NPCGameObject npc)
        {
            currentfSMState.Reason(npc);
            currentfSMState.Act(npc);
        }

        public void AddFSMState(FSMState fSMState)
        {
            if (currentfSMState == null)
            {
                currentfSMState = fSMState;
                currentfSMStateCode = fSMState.StateCode;
            }
            if (fSMStateDict.ContainsKey(fSMState.StateCode))
            {
                return;
            }
            fSMStateDict.Add(fSMState.StateCode, fSMState);
        }

        public void DeletFSMState(FSMStateCode fSMStateCode)
        {
            if (!fSMStateDict.ContainsKey(fSMStateCode))
            {
                return;
            }
            fSMStateDict.Remove(fSMStateCode);
        }

        public void OnFSMTransition(FSMTransitionCode fSMTransitionCode)
        {
            FSMStateCode newFSMStateCode = currentfSMState.GetFSMStateCode(fSMTransitionCode);
            if (newFSMStateCode == FSMStateCode.Null)
            {
                return;
            }
            if (!fSMStateDict.ContainsKey(newFSMStateCode))
            {
                return;
            }
            currentfSMState.DoBeforeEntering();
            currentfSMState = fSMStateDict[newFSMStateCode];
            currentfSMStateCode = newFSMStateCode;
        }

        public void SetTargetClientPeer(ClientPeer targetPeer)
        {
            TargetClienPeer = targetPeer;
        }
        public void RemoveTargetClientPeer()
        {
            TargetClienPeer = null;
        }
    }
}
