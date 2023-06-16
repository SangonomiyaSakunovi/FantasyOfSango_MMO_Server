using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//Developer: SangonomiyaSakunovi

namespace SangoIOCPNet
{
    public class IOCPPeer<T> where T : IClientPeer, new()
    {
        private Socket socket;
        private SocketAsyncEventArgs socketAsyncEventArgs;

        public IOCPPeer()
        {
            socketAsyncEventArgs = new SocketAsyncEventArgs();
            socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIO_Completed);
        }

        #region Client
        public T ClientPeer;

        public void InitClient(string ip, int port)
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(point.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socketAsyncEventArgs.RemoteEndPoint = point;
            OnConnect();
        }

        private void OnConnect()
        {
            bool isConnetWaiting = socket.ConnectAsync(socketAsyncEventArgs);
            if (isConnetWaiting == false)
            {
                ProcessConnect();
            }
        }

        private void ProcessConnect()
        {
            ClientPeer = new T();
            ClientPeer.InitClientPeer(socket);
        }

        public void CloseClient()
        {
            if (ClientPeer != null)
            {
                ClientPeer.OnClientClose();
                ClientPeer = null;
            }
            if (socket != null)
            {
                socket = null;
            }
        }
        #endregion

        #region Server
        private int currentConnectCount = 0;
        public int backLog = ServerConstant.ServerBackLogCount;
        public int maxConnectCount = ServerConstant.ServerMaxConnectCount;

        private Semaphore acceptSeamaphore;
        private IOCPClientPeerPool<T> peerPool;
        private List<T> peerList;

        public void InitServer(string ip, int port, int maxConnectCount)
        {
            currentConnectCount = 0;
            acceptSeamaphore = new Semaphore(maxConnectCount, maxConnectCount);
            peerPool = new IOCPClientPeerPool<T>(maxConnectCount);
            for (int i = 0; i < maxConnectCount; i++)
            {
                T peer = new T
                {
                    PeerId = i,
                };
                peerPool.Push(peer);
            }
            peerList = new List<T>();
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(point.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(point);
            socket.Listen(backLog);
            IOCPLog.Done("IOCPServer is Init");
            OnAccept();
        }

        private void OnAccept()
        {
            socketAsyncEventArgs.AcceptSocket = null;
            acceptSeamaphore.WaitOne();
            bool isAcceptWaiting = socket.AcceptAsync(socketAsyncEventArgs);
            if (isAcceptWaiting == false)
            {
                ProcessAccept();
            }
        }

        private void ProcessAccept()
        {
            Interlocked.Increment(ref currentConnectCount);
            T peer = peerPool.Pop();
            lock (peerList)
            {
                peerList.Add(peer);
            }
            peer.InitClientPeer(socketAsyncEventArgs.AcceptSocket);
            peer.OnClientPeerCloseCallBack = RecycleClientPeerPool;
            IOCPLog.Done("Client Online, allocate ClientId:{0}", peer.PeerId);
            OnAccept();
        }

        public void CloseServer()
        {
            for (int i = 0; i < peerList.Count; i++)
            {
                peerList[i].OnClientClose();
            }
            peerList.Clear();
            peerList = null;
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
        }

        private void RecycleClientPeerPool(int peerId)
        {
            int index = -1;
            for (int i = 0; i < peerList.Count; i++)
            {
                if (peerList[i].PeerId == peerId)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                peerPool.Push(peerList[index]);
                lock (peerList)
                {
                    peerList.RemoveAt(index);
                }
                Interlocked.Decrement(ref currentConnectCount);
                acceptSeamaphore.Release();
            }
            else
            {
                IOCPLog.Error("IClientPeer: {0} can`t find in server peerList.", peerId);
            }
        }

        public List<T> GetAllClientPeerList()
        {
            return peerList;
        }
        #endregion

        private void OnIO_Completed(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            switch (socketAsyncEventArgs.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    ProcessAccept();
                    break;
                case SocketAsyncOperation.Connect:
                    ProcessConnect();
                    break;
                default:
                    IOCPLog.Warning("The last operation completed on the socket was not a accept or connect");
                    break;
            }
        }
    }
}
