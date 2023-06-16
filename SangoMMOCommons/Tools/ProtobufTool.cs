using ProtoBuf;
using System;
using System.IO;

//Developer: SangonomiyaSakunovi

namespace SangoMMOCommons.Tools
{
    public class ProtobufTool
    {
        public static byte[] SetProtoBytes(object ob)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, ob);
                byte[] bytes = new byte[ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, bytes, 0, (int)ms.Length);
                return bytes;
            }
        }

        public static T DeProtoBytes<T>(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}
