using System.Collections.Generic;

//Developer: SangonomiyaSakunovi

namespace SangoIOCPNet
{
    public class IOCPClientPeerPool<T> where T : IClientPeer, new()
    {
        private Stack<T> clientPeerStack;
        public int Size => clientPeerStack.Count;

        public IOCPClientPeerPool(int capacity)
        {
            clientPeerStack = new Stack<T>(capacity);
        }

        public T Pop()
        {
            lock (clientPeerStack)
            {
                return clientPeerStack.Pop();
            }
        }

        public void Push(T peer)
        {
            if (peer == null)
            {
                IOCPLog.Error("The clientPeer to pool can`t be null");
            }
            lock (clientPeerStack)
            {
                clientPeerStack.Push(peer);
            }
        }
    }
}
