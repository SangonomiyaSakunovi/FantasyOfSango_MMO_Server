using FantasyOfSango_MMO_Server.Services;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Bases
{
    public class BaseSystem
    {
        protected ResourceService resourceService;

        public virtual void InitSystem()
        {
            resourceService = ResourceService.Instance;
        }
    }
}
