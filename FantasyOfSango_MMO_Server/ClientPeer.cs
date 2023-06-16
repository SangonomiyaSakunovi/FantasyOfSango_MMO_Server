using FantasyOfSango_MMO_Server.Caches;
using FantasyOfSango_MMO_Server.Enums;
using FantasyOfSango_MMO_Server.Services;
using SangoIOCPNet;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using SangoMMOCommons.Tools;
using SangoMMONetProtocol;

//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server
{
    public class ClientPeer : IClientPeer
    {
        public string Account { get; private set; }
        public AOISceneGrid AOISceneGrid { get; private set; }
        public AvaterCode CurrentAvater { get; private set; }
        public int CurrentAvaterIndex { get; private set; }
        public AvaterInfo AvaterInfo { get; private set; }
        public MissionInfo MissionInfo { get; private set; }
        public ItemInfo ItemInfo { get; private set; }

        public int TransformClock { get; private set; }
        public TransformOnline CurrentTransformOnline { get; private set; }
        public TransformOnline LastTransformOnline { get; private set; }

        public PeerEnhanceModeCode PeerEnhanceModeCode { get; private set; }

        protected override void OnConnect()
        {
            IOCPLog.Info("A new client is Connected.");
        }

        protected override void OnDisconnect()
        {
            IOCPLog.Info("A client is DisConnected.");
            OnlineAccountCache.Instance.RemoveOnlineAccount(this);
        }

        protected override void OnRecieveMessage(byte[] byteMessages)
        {
            SangoNetMessage sangoNetMessage = ProtobufTool.DeProtoBytes<SangoNetMessage>(byteMessages);
            switch (sangoNetMessage.MessageHead.MessageCommand)
            {
                case MessageCommand.OperationRequest:
                    {
                        NetService.Instance.OnOperationRequestDistribute(sangoNetMessage, this);
                    }
                    break;
            }
        }

        public void SendOperationResponse(OperationCode operationCode, string messageString)
        {
            MessageHead messageHead = new()
            {
                OperationCode = operationCode,
                MessageCommand = MessageCommand.OperationResponse
            };
            MessageBody messageBody = new()
            {
                MessageString = messageString
            };
            SangoNetMessage sangoNetMessage = new()
            {
                MessageHead = messageHead,
                MessageBody = messageBody
            };
            SendData(sangoNetMessage);
        }

        public void SendOperationResponse(OperationCode operationCode, ReturnCode returnCode)
        {
            MessageHead messageHead = new()
            {
                OperationCode = operationCode,
                MessageCommand = MessageCommand.OperationResponse
            };
            MessageBody messageBody = new()
            {
                ReturnCode = returnCode
            };
            SangoNetMessage sangoNetMessage = new()
            {
                MessageHead = messageHead,
                MessageBody = messageBody
            };
            SendData(sangoNetMessage);
        }

        public void SendEvent(OperationCode operationCode, string messageString)
        {
            MessageHead messageHead = new()
            {
                OperationCode = operationCode,
                MessageCommand = MessageCommand.EventData
            };
            MessageBody messageBody = new()
            {
                MessageString = messageString
            };
            SangoNetMessage sangoNetMessage = new()
            {
                MessageHead = messageHead,
                MessageBody = messageBody
            };
            SendData(sangoNetMessage);
        }

        private void SendData(SangoNetMessage sangoNetMessage)
        {
            byte[] bytes = ProtobufTool.SetProtoBytes(sangoNetMessage);
            SendMessage(bytes);
        }

        public void SetAccount(string account)
        {
            Account = account;
        }

        public void SetAOIGrid(AOISceneGrid aoiSceneGrid)
        {
            AOISceneGrid = aoiSceneGrid;
        }

        public void SetAvaterInfo(AvaterInfo avaterInfo)
        {
            AvaterInfo = avaterInfo;
        }

        public void SetMissionInfo(MissionInfo missionInfo)
        {
            MissionInfo = missionInfo;
        }

        public void SetItemInfo(ItemInfo itemInfo)
        {
            ItemInfo = itemInfo;
        }

        public void SetCurrentAvaterIndexByAvaterCode(AvaterCode avater)
        {
            CurrentAvater = avater;
            int index = 0;
            for (int i = 0; i < AvaterInfo.AttributeInfoList.Count; i++)
            {
                if (AvaterInfo.AttributeInfoList[i].Avater == avater)
                {
                    index = i; break;
                }
            }
            CurrentAvaterIndex = index;
        }

        public void SetTransformOnline(TransformOnline transformOnline)
        {
            LastTransformOnline = CurrentTransformOnline;
            CurrentTransformOnline = transformOnline;
        }

        public void SetTransformClock(int clock)
        {
            TransformClock = clock;
        }

        public void SetPeerEnhanceModeCode(PeerEnhanceModeCode peerEnhanceModeCode)
        {
            PeerEnhanceModeCode = peerEnhanceModeCode;
        }
    }
}
