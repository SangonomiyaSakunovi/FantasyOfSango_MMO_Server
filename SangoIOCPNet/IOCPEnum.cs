//Developer: SangonomiyaSakunovi

namespace SangoIOCPNet
{
    public enum IOCPLogColor
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow,
    }

    public enum ConnectionStateCode
    {
        None,
        Disconnected,
        Connecting,
        Connected,
        Disconnecting,
        DisposeDisconnecting,
        Disposed,
        Initializing
    }
}
