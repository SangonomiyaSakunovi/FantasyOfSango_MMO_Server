using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Configs;
using FantasyOfSango_MMO_Server.Constants;
using SangoMMOCommons.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Services
{
    public class ResourceService : BaseService
    {
        public static ResourceService Instance = null;

        private Dictionary<string, MissionConfig> missionConfigDict = new Dictionary<string, MissionConfig>();
        private Dictionary<string, WeaponBreakConfig> weaponBreakConfigDict = new Dictionary<string, WeaponBreakConfig>();
        private Dictionary<string, WeaponDetailsConfig> weaponDetailsConfigDict = new Dictionary<string, WeaponDetailsConfig>();
        private Dictionary<string, WeaponValueConfig> weaponValueConfigDict = new Dictionary<string, WeaponValueConfig>();

        public override void InitService()
        {
            base.InitService();
            Instance = this;
            InitMissionConfig(ConfigConstant.MissionConfigPath_01);
            InitWeaponBreakConfig(ConfigConstant.WeaponBreakConfigPath_01);
            InitWeaponDetailsConfig(ConfigConstant.WeaponDetailsConfigPath_01);
            InitWeaponValueConfig(ConfigConstant.WeaponValueConfigPath_01);
        }

        #region MissionConfig
        private void InitMissionConfig(string path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNodeList xmlNodeList = document.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement xmlElement = xmlNodeList[i] as XmlElement;
                if (xmlElement.GetAttributeNode("_id") == null)
                {
                    continue;
                }
                string missionId = xmlElement.GetAttributeNode("_id").InnerText;
                MissionConfig config = new MissionConfig
                {
                    _id = missionId
                };
                foreach (XmlElement element in xmlNodeList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "npcID":
                            config.npcID = element.InnerText;
                            break;
                        case "actionID":
                            config.actionID = element.InnerText;
                            break;
                        case "coinRewards":
                            config.coinRewards = int.Parse(element.InnerText);
                            break;
                        case "worldExpRewards":
                            config.worldExpRewards = int.Parse(element.InnerText);
                            break;
                        case "material1Rewards":
                            config.material1Rewards = element.InnerText;
                            break;
                        case "material2Rewards":
                            config.material2Rewards = element.InnerText;
                            break;
                    }
                }
                missionConfigDict.Add(missionId, config);
            }
        }

        public MissionConfig GetMissionConfig(string missionId)
        {
            return DictTool.GetDictValue<string, MissionConfig>(missionId, missionConfigDict);
        }
        #endregion

        #region WeaponsConfig
        private void InitWeaponBreakConfig(string path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNodeList xmlNodeList = document.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement xmlElement = xmlNodeList[i] as XmlElement;
                if (xmlElement.GetAttributeNode("_id") == null)
                {
                    continue;
                }
                string weaponBreakId = xmlElement.GetAttributeNode("_id").InnerText;
                WeaponBreakConfig config = new WeaponBreakConfig
                {
                    _id = weaponBreakId
                };
                foreach (XmlElement element in xmlNodeList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "weaponBreakCoin":
                            config.weaponBreakCoin = int.Parse(element.InnerText);
                            break;
                        case "weaponBreakMaterial1":
                            config.weaponBreakMaterial1 = element.InnerText;
                            break;
                        case "weaponBreakMaterial2":
                            config.weaponBreakMaterial2 = element.InnerText;
                            break;
                    }
                }
                weaponBreakConfigDict.Add(weaponBreakId, config);
            }
        }

        private void InitWeaponDetailsConfig(string path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNodeList xmlNodeList = document.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement xmlElement = xmlNodeList[i] as XmlElement;
                if (xmlElement.GetAttributeNode("_id") == null)
                {
                    continue;
                }
                string weaponInfoId = xmlElement.GetAttributeNode("_id").InnerText;
                WeaponDetailsConfig config = new WeaponDetailsConfig
                {
                    _id = weaponInfoId
                };
                foreach (XmlElement element in xmlNodeList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "weaponQuanlity":
                            config.weaponQuanlity = int.Parse(element.InnerText);
                            break;
                    }
                }
                weaponDetailsConfigDict.Add(weaponInfoId, config);
            }
        }

        private void InitWeaponValueConfig(string path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNodeList xmlNodeList = document.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement xmlElement = xmlNodeList[i] as XmlElement;
                if (xmlElement.GetAttributeNode("_id") == null)
                {
                    continue;
                }
                string weaponValueId = xmlElement.GetAttributeNode("_id").InnerText;
                WeaponValueConfig config = new WeaponValueConfig
                {
                    _id = weaponValueId
                };
                foreach (XmlElement element in xmlNodeList[i].ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "weaponBaseATK":
                            config.weaponBaseATK = int.Parse(element.InnerText);
                            break;
                        case "weaponAbility1":
                            config.weaponAbility1 = element.InnerText;
                            break;
                        case "weaponAbility2":
                            config.weaponAbility2 = element.InnerText;
                            break;
                        case "weaponEnhanceLevelExp":
                            config.weaponEnhanceLevelExp = int.Parse(element.InnerText);
                            break;
                        case "weaponAccumulateExp":
                            config.weaponAccumulateExp = int.Parse(element.InnerText);
                            break;
                    }
                }
                weaponValueConfigDict.Add(weaponValueId, config);
            }
        }

        public WeaponBreakConfig GetWeaponBreakConfig(string weaponBreakId)
        {
            return DictTool.GetDictValue<string, WeaponBreakConfig>(weaponBreakId, weaponBreakConfigDict);
        }

        public WeaponDetailsConfig GetWeaponDetailsConfig(string weaponDetailsId)
        {
            return DictTool.GetDictValue<string, WeaponDetailsConfig>(weaponDetailsId, weaponDetailsConfigDict);
        }

        public WeaponValueConfig GetWeaponValueConfig(string weaponValueId)
        {
            return DictTool.GetDictValue<string, WeaponValueConfig>(weaponValueId, weaponValueConfigDict);
        }
        #endregion
    }
}
