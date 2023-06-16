using FantasyOfSango_MMO_Server.Bases;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Systems
{
    public class AOISystem : BaseSystem
    {
        public static AOISystem Instance = null;

        public override void InitSystem()
        {
            base.InitSystem();
            Instance = this;
        }

        public AOISceneGrid SetAOIGrid(SceneCode sceneCode, float x, float z)
        {
            AOISceneGrid aoiSceneGrid;
            //We define the GridName with xRange/100_zRange/100
            if (sceneCode == SceneCode.Island)
            {
                //In this real Scene, the xRange is (-700,800) => +700 => (0,1500)
                //In this real Scene, the zRange is (-400,800) => +400 => (0,1200)
                int gridX = (int)(x + 700) / 100;
                int gridZ = (int)(z + 400) / 100;
                aoiSceneGrid = new AOISceneGrid(sceneCode, gridX, gridZ);
            }
            else
            {
                aoiSceneGrid = new AOISceneGrid(SceneCode.Default, 0, 0);
            }
            return aoiSceneGrid;
        }

        public List<AOISceneGrid> GetSurroundAOIGrid(AOISceneGrid aoiSceneGrid)
        {
            SceneCode sceneCode = aoiSceneGrid.SceneCode;
            int centerGridX = aoiSceneGrid.GridX;
            int centerGridZ = aoiSceneGrid.GridZ;
            List<AOISceneGrid> aoiSceneGridList = new List<AOISceneGrid>(8)
            {
                new AOISceneGrid(sceneCode, centerGridX, centerGridZ),
                new AOISceneGrid(sceneCode, centerGridX, centerGridZ + 1),
                new AOISceneGrid(sceneCode, centerGridX, centerGridZ - 1),
                new AOISceneGrid(sceneCode, centerGridX - 1, centerGridZ),
                new AOISceneGrid(sceneCode, centerGridX + 1, centerGridZ),
                new AOISceneGrid(sceneCode, centerGridX - 1, centerGridZ + 1),
                new AOISceneGrid(sceneCode, centerGridX - 1, centerGridZ - 1),
                new AOISceneGrid(sceneCode, centerGridX + 1, centerGridZ + 1),
                new AOISceneGrid(sceneCode, centerGridX + 1, centerGridZ - 1)
            };
            return aoiSceneGridList;
        }
    }
}
