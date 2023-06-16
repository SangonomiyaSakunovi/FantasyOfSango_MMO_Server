using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Systems
{
    public class OnlineAttackSystem : BaseSystem
    {
        public static OnlineAttackSystem Instance = null;

        private int PerAttackExperience = 1;

        public override void InitSystem()
        {
            base.InitSystem();
            Instance = this;
        }

        public AttackResult GetAttackResult(AttackDamage attackDamage)
        {
            AttackResult attackResult;
            if (attackDamage.FightTypeCode == FightTypeCode.PVP)
            {
                if (attackDamage.SkillCode == SkillCode.Attack)
                {
                    attackResult = SetAttackDamage(attackDamage);
                }
                else if (attackDamage.SkillCode == SkillCode.ElementAttack)
                {
                    attackResult = SetElementAttackDamage(attackDamage);
                }
                else if (attackDamage.SkillCode == SkillCode.ElementBurst)
                {
                    attackResult = SetElementBurstDamage(attackDamage);
                }
                else
                {
                    attackResult = null;
                }
            }
            if (attackDamage.FightTypeCode == FightTypeCode.PVE)
            {
                if (attackDamage.SkillCode == SkillCode.ElementAttack)
                {
                    attackResult = SetElementAttackDamagePVE(attackDamage);
                    return attackResult;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        private AttackResult SetAttackDamage(AttackDamage attackDamage)
        {
            ClientPeer attackerPeer = OnlineAccountCache.Instance.GetOnlinePlayerClientPeerByAccount(attackDamage.AttackerAccount);
            ClientPeer damagerPeer = OnlineAccountCache.Instance.GetOnlinePlayerClientPeerByAccount(attackDamage.DamagerAccount);
            AvaterInfo attackerAvaterInfo = attackerPeer.AvaterInfo;
            AvaterInfo damagerAvaterInfo = damagerPeer.AvaterInfo;
            int attackerIndex = attackerPeer.CurrentAvaterIndex;
            int damagerIndex = damagerPeer.CurrentAvaterIndex;
            int attackerAttack = attackerAvaterInfo.AttributeInfoList[attackerIndex].Attack;
            int damagerDefence = damagerAvaterInfo.AttributeInfoList[damagerIndex].Defence;
            int attackDamageNum = attackerAttack - damagerDefence;
            damagerAvaterInfo.AttributeInfoList[damagerIndex].HP -= attackDamageNum;
            AttackResult attackResult = new AttackResult
            {
                AttackerAccount = attackDamage.AttackerAccount,
                DamagerAccount = attackDamage.DamagerAccount,
                DamageNumber = attackDamageNum,
                AttackerAvaterInfo = attackerAvaterInfo,
                DamagerAvaterInfo = damagerAvaterInfo
            };
            attackerPeer.SetAvaterInfo(attackerAvaterInfo);
            damagerPeer.SetAvaterInfo(damagerAvaterInfo);
            return attackResult;
        }

        private AttackResult SetElementAttackDamage(AttackDamage attackDamage)
        {
            ClientPeer attackerPeer = OnlineAccountCache.Instance.GetOnlinePlayerClientPeerByAccount(attackDamage.AttackerAccount);
            ClientPeer damagerPeer = OnlineAccountCache.Instance.GetOnlinePlayerClientPeerByAccount(attackDamage.DamagerAccount);
            AvaterInfo attackerAvaterInfo = attackerPeer.AvaterInfo;
            AvaterInfo damagerAvaterInfo = damagerPeer.AvaterInfo;
            int attackerIndex = attackerPeer.CurrentAvaterIndex;
            int damagerIndex = damagerPeer.CurrentAvaterIndex;
            //Exam if that damageRequest from Kokomi, she can give a healer
            if (attackerAvaterInfo.AttributeInfoList[attackerIndex].Avater == AvaterCode.SangonomiyaKokomi)
            {
                int kokomiHealer = attackerAvaterInfo.AttributeInfoList[attackerIndex].Attack * 2;
                damagerAvaterInfo.AttributeInfoList[damagerIndex].HP += kokomiHealer;
                AttackResult attackResultCache = new AttackResult
                {
                    AttackerAccount = attackDamage.AttackerAccount,
                    DamagerAccount = attackDamage.DamagerAccount,
                    DamageNumber = -kokomiHealer,
                    AttackerAvaterInfo = attackerAvaterInfo,
                    DamagerAvaterInfo = damagerAvaterInfo
                };
                attackerPeer.SetAvaterInfo(attackerAvaterInfo);
                damagerPeer.SetAvaterInfo(damagerAvaterInfo);
                return attackResultCache;
            }
            return null;
        }

        private AttackResult SetElementBurstDamage(AttackDamage attackDamage)
        {
            return null;
        }

        private AttackResult SetElementAttackDamagePVE(AttackDamage attackDamage)
        {
            ClientPeer attakerPeer = OnlineAccountCache.Instance.GetOnlinePlayerClientPeerByAccount(attackDamage.AttackerAccount);
            AvaterInfo attackerAvaterInfo = attakerPeer.AvaterInfo;
            int attackerIndex = attakerPeer.CurrentAvaterIndex;

            NPCCode code = (NPCCode)Enum.Parse(typeof(NPCCode), attackDamage.DamagerAccount);
            BaseAI ai = OnlineNPCCache.Instance.GetNPCAI(code);

            ai.SetDamaged(attackerAvaterInfo.AttributeInfoList[attackerIndex].Avater, SkillCode.ElementAttack, attackDamage.AttackerVector3Position);
            //Exam if that damageRequest from Kokomi, she can give a damage the same as healer amount
            if (attackerAvaterInfo.AttributeInfoList[attackerIndex].Avater == AvaterCode.SangonomiyaKokomi)
            {
                int kokomiHealer = attackerAvaterInfo.AttributeInfoList[attackerIndex].Attack * 2;
                ai.GetNPCGameObjectInfo().NPCAttributeInfo.HP -= kokomiHealer;
                AttackResult attackResultCache = new AttackResult
                {
                    AttackerAccount = attackDamage.AttackerAccount,
                    DamagerAccount = attackDamage.DamagerAccount,
                    DamageNumber = kokomiHealer,
                    AttackerAvaterInfo = attackerAvaterInfo,
                };
                //OnlineAccountCache.Instance.UpdateOnlineAvaterInfo(attackDamage.AttackerAccount, attackerAvaterInfo, attackDamage.DamagerAccount, damagerAvaterInfo);
                //TODO: 更新玩家状态
                return attackResultCache;
            }
            return null;
        }
    }
}
