using System;
using System.Collections.Generic;
using System.Net.Sockets;

//Developer: SangonomiyaSakunovi

namespace SangoIOCPNet
{
    public abstract class IClientPeer
    {
        public int PeerId;
        private SocketAsyncEventArgs receiveAsyncEventArgs;
        private SocketAsyncEventArgs sendAsyncEventArgs;

        private Socket socket;
        private List<byte> readList = new List<byte>();
        private Queue<byte[]> cacheQueue = new Queue<byte[]>();
        private bool isWrite = false;

        public Action<int> OnClientPeerCloseCallBack;
        public ConnectionStateCode ConnectionState = ConnectionStateCode.None;

        public IClientPeer()
        {
            receiveAsyncEventArgs = new SocketAsyncEventArgs();
            sendAsyncEventArgs = new SocketAsyncEventArgs();
            receiveAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIO_Completed);
            sendAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIO_Completed);
            receiveAsyncEventArgs.SetBuffer(new byte[ServerConstant.ServerBufferCount], 0, ServerConstant.ServerBufferCount);
        }

        protected abstract void OnConnect();

        protected abstract void OnDisconnect();

        protected abstract void OnRecieveMessage(byte[] byteMessages);

        public void InitClientPeer(Socket skt)
        {
            IOCPLog.Info("正在初始化ClientPeer，开始异步接收");
            socket = skt;
            ConnectionState = ConnectionStateCode.Connected;
            OnConnect();
            OnReceiveAsync();
        }

        private void OnReceiveAsync()
        {
            bool isConnetWaiting = socket.ReceiveAsync(receiveAsyncEventArgs);
            if (isConnetWaiting == false)
            {
                ProcessReceive();
            }
        }

        private void ProcessReceive()
        {
            if (receiveAsyncEventArgs.BytesTransferred > 0 && receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                byte[] bytes = new byte[receiveAsyncEventArgs.BytesTransferred];
                Buffer.BlockCopy(receiveAsyncEventArgs.Buffer, 0, bytes, 0, receiveAsyncEventArgs.BytesTransferred);
                readList.AddRange(bytes);
                ProcessByteList();
                OnReceiveAsync();
            }
            else
            {
                IOCPLog.Warning("IClientPeer:{0}  Close:{1}", PeerId, receiveAsyncEventArgs.SocketError.ToString());
                OnClientClose();
            }
        }

        private void ProcessByteList()
        {
            byte[] byteMessages = IOCPTool.SplitLogicBytes(ref readList);
            if (byteMessages != null)
            {
                OnRecieveMessage(byteMessages);
                ProcessByteList();
            }
        }

        public bool SendMessage(byte[] byteMessage)
        {
            byte[] bytes = IOCPTool.PackMessageLengthInfo(byteMessage);
            return SendPackMessage(bytes);
        }

        public byte[] GetPackMessage(byte[] byteMessage)
        {
            return IOCPTool.PackMessageLengthInfo(byteMessage);
        }

        public bool SendPackMessage(byte[] bytePackMessages)
        {
            if (ConnectionState != ConnectionStateCode.Connected)
            {
                IOCPLog.Warning("Connection is break, can`t send net message.");
                return false;
            }
            if (isWrite)
            {
                cacheQueue.Enqueue(bytePackMessages);
                return true;
            }
            isWrite = true;
            sendAsyncEventArgs.SetBuffer(bytePackMessages, 0, bytePackMessages.Length);
            bool isSendWaiting = socket.SendAsync(sendAsyncEventArgs);
            if (isSendWaiting == false)
            {
                ProcessSend();
            }
            return true;
        }

        private void ProcessSend()
        {
            if (sendAsyncEventArgs.SocketError == SocketError.Success)
            {
                isWrite = false;
                if (cacheQueue.Count > 0)
                {
                    byte[] item = cacheQueue.Dequeue();
                    SendPackMessage(item);
                }
            }
            else
            {
                IOCPLog.Error("Process Send Error: {0}", sendAsyncEventArgs.SocketError.ToString());
                OnClientClose();
            }
        }

        public void OnClientClose()
        {
            if (socket != null)
            {
                ConnectionState = ConnectionStateCode.Disconnected;
                OnDisconnect();
                if (OnClientPeerCloseCallBack != null)
                {
                    OnClientPeerCloseCallBack(PeerId);
                }
                readList.Clear();
                cacheQueue.Clear();
                isWrite = false;
                try
                {
                    socket.Shutdown(SocketShutdown.Send);
                }
                catch (Exception e)
                {
                    IOCPLog.Error("Shutdown socket Error:{0}", e.ToString());
                }
                finally
                {
                    socket.Close();
                    socket = null;
                    IOCPLog.Done("Client is Offline");
                }
            }
        }

        private void OnIO_Completed(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            switch (socketAsyncEventArgs.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive();
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend();
                    break;
                default:
                    IOCPLog.Warning("The last operation completed on the socket was not a receive or send");
                    break;
            }
        }
    }
}
