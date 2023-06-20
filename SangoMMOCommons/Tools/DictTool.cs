using System.Collections.Concurrent;
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
                return default;
            }
        }

        public static T_Value GetDictValue<T_Key, T_Value>(T_Key key, ConcurrentDictionary<T_Key, T_Value> dict)
        {
            T_Value value;
            bool isGetValue = dict.TryGetValue(key, out value);
            if (isGetValue)
            {
                return value;
            }
            else
            {
                return default;
            }
        }
    }
}
