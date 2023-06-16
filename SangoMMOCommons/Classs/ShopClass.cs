//Developer : SangonomiyaSakunovi

namespace SangoMMOCommons.Classs
{
    public class ShopInfoReq
    {
        public string ShopItemId { get; set; }
        public int ShopNumber { get; set; }
    }

    public class ShopInfoRsp
    {
        public bool IsShopSuccess { get; set; }
        public string ShopItemId { get; set; }
    }
}
