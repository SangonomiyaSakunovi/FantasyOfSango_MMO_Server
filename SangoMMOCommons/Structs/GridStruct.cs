using SangoMMOCommons.Enums;
using System;

//Developer : SangonomiyaSakunovi

namespace SangoMMOCommons.Structs
{
    public struct AOISceneGrid : IEquatable<AOISceneGrid>
    {
        public SceneCode SceneCode { get; set; }
        public int GridX { get; set; }
        public int GridZ { get; set; }

        public AOISceneGrid(SceneCode sceneCode, int gridX, int gridZ) : this()
        {
            SceneCode = sceneCode;
            GridX = gridX;
            GridZ = gridZ;
        }

        public bool Equals(AOISceneGrid other) => SceneCode == other.SceneCode && GridX == other.GridX && GridZ == other.GridZ;
        public override bool Equals(object obj) => obj is AOISceneGrid aoi && this.Equals(aoi);
        public override int GetHashCode() => (GridX, GridZ).GetHashCode();
        public static bool operator ==(AOISceneGrid aoi1, AOISceneGrid aoi2) => aoi1.Equals(aoi2);
        public static bool operator !=(AOISceneGrid aoi1, AOISceneGrid aoi2) => !(aoi1 == aoi2);

    }

    public struct Vector2Grid : IEquatable<Vector2Grid>
    {
        public int GridX { get; set; }
        public int GridZ { get; set; }
        public Vector2Grid(int gridX, int gridZ)
        {
            GridX = gridX;
            GridZ = gridZ;
        }

        public bool Equals(Vector2Grid other) => GridX == other.GridX && GridZ == other.GridZ;
        public override bool Equals(object obj) => obj is Vector2Grid grid && this.Equals(grid);
        public override int GetHashCode() => (GridX, GridZ).GetHashCode();
        public static bool operator ==(Vector2Grid grid1, Vector2Grid grid2) => grid1.Equals(grid2);
        public static bool operator !=(Vector2Grid grid1, Vector2Grid grid2) => !(grid1 == grid2);
    }

    public struct AStarGraphJsonStruct
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int IsObstacle { get; set; }
        public AStarGraphJsonStruct(int x, int y, int z, int obs)
        {
            X = x; Y = y;
            Z = z; IsObstacle = obs;
        }
    }
}
