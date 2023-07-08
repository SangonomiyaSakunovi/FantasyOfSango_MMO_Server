using System;

//Developer : SangonomiyaSakunovi

namespace SangoMMOCommons.Classs
{
    [Serializable]
    public class ShopInfoReq
    {
        public string ShopItemId { get; set; }
        public int ShopNumber { get; set; }
    }

    [Serializable]
    public class ShopInfoRsp
    {
        public bool IsShopSuccess { get; set; }
        public string ShopItemId { get; set; }
    }
}
