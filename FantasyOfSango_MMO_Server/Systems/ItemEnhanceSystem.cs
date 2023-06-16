using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Configs;
using FantasyOfSango_MMO_Server.Services;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Systems
{
    public class ItemEnhanceSystem : BaseSystem
    {
        public static ItemEnhanceSystem Instance = null;

        public override void InitSystem()
        {
            base.InitSystem();
            Instance = this;
        }

        public ItemEnhanceRsp GetWeaponEnhanceOrBreakResult(ItemEnhanceReq itemEnhanceReq, ClientPeer clientPeer)
        {
            ItemEnhanceRsp itemEnhanceRsp = null;
            if (itemEnhanceReq.EnhanceTypeCode == EnhanceTypeCode.Enhance)
            {
                itemEnhanceRsp = GetEnhanceWeaponResult(itemEnhanceReq, clientPeer);
            }
            else
            {
                itemEnhanceRsp = GetBreakWeaponResult(itemEnhanceReq, clientPeer);
            }
            return itemEnhanceRsp;
        }

        private ItemEnhanceRsp UpdateOnlineAccountWeaponInfo(ItemEnhanceReq itemEnhanceReq, ClientPeer clientPeer)
        {
            WeaponValueConfig avaterWeaponValueConfig = resourceService.GetWeaponValueConfig(itemEnhanceReq.ItemId);
            int tempWeaponExp = clientPeer.AvaterInfo.AttributeInfoList[0].WeaponExp;
            for (int i = 0; i < itemEnhanceReq.ItemModelMaterialList.Count; i++)
            {
                WeaponValueConfig materialWeaponValueConfig = resourceService.GetWeaponValueConfig(itemEnhanceReq.ItemModelMaterialList[i]);
                tempWeaponExp += materialWeaponValueConfig.weaponAccumulateExp / 2;
            }
            for (int i = 0; i < itemEnhanceReq.ItemRawMaterialList.Count; i++)
            {

            }
            if (tempWeaponExp < avaterWeaponValueConfig.weaponEnhanceLevelExp)
            {
                clientPeer.AvaterInfo.AttributeInfoList[0].WeaponExp = tempWeaponExp;
            }
            else
            {
                string[] weaponIdSplit = itemEnhanceReq.ItemId.Split('_');
                RecursionCallUpdateOnlineAccountWeaponInfo(weaponIdSplit, tempWeaponExp, avaterWeaponValueConfig.weaponEnhanceLevelExp, clientPeer);
            }
            ItemEnhanceRsp itemEnhanceRsp = new ItemEnhanceRsp
            {
                IsEnhanceSuccess = true,
                ItemTypeCode = itemEnhanceReq.ItemTypeCode,
                EnhanceTypeCode = itemEnhanceReq.EnhanceTypeCode,
            };
            return itemEnhanceRsp;
        }

        private void RecursionCallUpdateOnlineAccountWeaponInfo(string[] tempWeaponIdSplit, int oldTempExp, int oldRequireExp, ClientPeer clientPeer)
        {
            if (int.Parse(tempWeaponIdSplit[2]) % 5 == 0)
            {
                //TODO How to deal this leftExp?
                int leftExp = oldTempExp - oldRequireExp;
                clientPeer.AvaterInfo.AttributeInfoList[0].WeaponExp = 0;
                clientPeer.AvaterInfo.AttributeInfoList[0].WeaponId = tempWeaponIdSplit[0] + "_" + tempWeaponIdSplit[1] + "_" + tempWeaponIdSplit[2] + "_S";
            }
            else
            {
                tempWeaponIdSplit[2] = (int.Parse(tempWeaponIdSplit[2]) + 1).ToString();
                string newWeaponId = tempWeaponIdSplit[0] + "_" + tempWeaponIdSplit[1] + "_" + tempWeaponIdSplit[2];
                int newWeaponExp = oldTempExp - oldRequireExp;
                WeaponValueConfig newAvaterWeaponValueConfig = resourceService.GetWeaponValueConfig(newWeaponId);
                if (newWeaponExp > newAvaterWeaponValueConfig.weaponEnhanceLevelExp)
                {
                    RecursionCallUpdateOnlineAccountWeaponInfo(tempWeaponIdSplit, newWeaponExp, newAvaterWeaponValueConfig.weaponEnhanceLevelExp, clientPeer);
                }
                else
                {
                    clientPeer.AvaterInfo.AttributeInfoList[0].WeaponId = newWeaponId;
                    clientPeer.AvaterInfo.AttributeInfoList[0].WeaponExp = newWeaponExp;
                }
            }
        }

        private ItemEnhanceRsp GetEnhanceWeaponResult(ItemEnhanceReq itemEnhanceReq, ClientPeer clientPeer)
        {
            ItemEnhanceRsp itemEnhanceRsp = null;
            int enhanceCoinRequire = 100 * itemEnhanceReq.ItemModelMaterialList.Count;
            if (enhanceCoinRequire <= clientPeer.ItemInfo.Coin)
            {
                itemEnhanceRsp = UpdateOnlineAccountWeaponInfo(itemEnhanceReq, clientPeer);
                clientPeer.ItemInfo.Coin -= enhanceCoinRequire;
            }
            else
            {
                itemEnhanceRsp = new ItemEnhanceRsp
                {
                    IsEnhanceSuccess = false
                };
            }
            return itemEnhanceRsp;
        }

        private ItemEnhanceRsp GetBreakWeaponResult(ItemEnhanceReq itemEnhanceReq, ClientPeer clientPeer)
        {
            ItemEnhanceRsp itemEnhanceRsp;
            //TODO
            return null;
        }
    }
}
