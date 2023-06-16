using System.Collections.Generic;

//Developer: SangonomiyaSakunovi

namespace SangoMMOCommons.Tools
{
    public class DictTool
    {
        public static T_Value GetDictValue<T_Key, T_Value>(T_Key key, Dictionary<T_Key, T_Value> dict)
        {
            T_Value value;
            bool isGetValue = dict.TryGetValue(key, out value);
            if (isGetValue)
            {
                return value;
            }
            else
            {
                return default(T_Value);
            }
        }

        public static string GetStringValue(byte key, Dictionary<byte, object> dict)
        {
            return GetDictValue<byte, object>(key, dict) as string;
        }

        public static int GetIntValue(byte key, Dictionary<byte, object> dict)
        {
            return (int)GetDictValue<byte, object>(key, dict);
        }

        public static bool GetBoolValue(byte key, Dictionary<byte, object> dict)
        {
            return (bool)GetDictValue<byte, object>(key, dict);
        }
    }
}
