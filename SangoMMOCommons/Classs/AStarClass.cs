using SangoMMOCommons.Structs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

//Developer : SangonomiyaSakunovi

namespace SangoMMOCommons.Classs
{
    public class AStarSystem
    {
        //Attention! In this method, we define that NPC can`t go to the edge of map, that should avoid before it runs in Server.
        //These transforms need set in games, so must get float, and also need change to int = 10*float.        
        //Attention! Int gridLenth = 1, means the real map grid is 0.1f.        
        public Dictionary<Vector2Grid, AStarGridPoint> SceneAStarGridDict = null;
        private int GridLength = 1;

        public AStarSystem(Dictionary<Vector2Grid, AStarGridPoint> sceneAStarGridDict)
        {
            SceneAStarGridDict = sceneAStarGridDict;
        }

        public List<AStarGridPoint> GetPath(Vector3Position startPos, Vector3Position targetPos)
        {
            int startXInt = (int)((startPos.X + float.Epsilon) * 10);
            int startZInt = (int)((startPos.Z + float.Epsilon) * 10);
            int targetXInt = (int)((targetPos.X + float.Epsilon) * 10);
            int targetZInt = (int)((targetPos.Z + float.Epsilon) * 10);
            Vector2Grid startGrid = new Vector2Grid(startXInt, startZInt);
            Vector2Grid targetGrid = new Vector2Grid(targetXInt, targetZInt);
            AStarGridPoint startPoint = SceneAStarGridDict[startGrid];
            AStarGridPoint targetPoint = SceneAStarGridDict[targetGrid];
            return PathFinder(startGrid, startPoint, targetGrid, targetPoint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private List<AStarGridPoint> PathFinder(Vector2Grid startGrid, AStarGridPoint startPoint, Vector2Grid targetGrid, AStarGridPoint targetPoint)
        {
            Dictionary<Vector2Grid, AStarGridPoint> openDict = new Dictionary<Vector2Grid, AStarGridPoint>();
            Dictionary<Vector2Grid, AStarGridPoint> closeDict = new Dictionary<Vector2Grid, AStarGridPoint>();
            openDict.Add(startGrid, startPoint);
            while (openDict.Count > 0)
            {
                AStarGridPoint minFValuePoint = FindMinFValuePoint(openDict);
                Vector2Grid minFValuePointPos = new Vector2Grid(minFValuePoint.X, minFValuePoint.Z);
                openDict.Remove(minFValuePointPos);
                closeDict.Add(minFValuePointPos, minFValuePoint);
                List<AStarGridPoint> surroundPointList = GetSurroundPoints(minFValuePointPos, closeDict);
                for (int i = 0; i < surroundPointList.Count; i++)
                {
                    AStarGridPoint tempPoint = surroundPointList[i];
                    Vector2Grid tempPointPos = new Vector2Grid(tempPoint.X, tempPoint.Z);
                    if (openDict.ContainsKey(tempPointPos))
                    {
                        float tempGValue = CalculateGValue(minFValuePoint, tempPoint);
                        if (tempGValue < tempPoint.G)
                        {
                            tempPoint.Parent = minFValuePoint;
                            tempPoint.G = tempGValue;
                            tempPoint.F = tempGValue + tempPoint.H;
                            openDict[tempPointPos] = tempPoint;
                        }
                    }
                    else
                    {
                        tempPoint.Parent = minFValuePoint;
                        CalculateFValue(tempPoint, targetPoint);
                        openDict.Add(tempPointPos, tempPoint);
                    }
                }
                if (openDict.ContainsKey(targetGrid))
                {
                    List<AStarGridPoint> resultAStarGridList = new List<AStarGridPoint>();
                    AStarGridPoint tempPoint = targetPoint;
                    while (tempPoint.Parent != null)
                    {
                        resultAStarGridList.Add(tempPoint);
                        tempPoint = tempPoint.Parent;
                    }
                    return resultAStarGridList;
                }
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private List<AStarGridPoint> GetSurroundPoints(Vector2Grid currentPointPos, Dictionary<Vector2Grid, AStarGridPoint> closeDict)
        {
            List<AStarGridPoint> surroundPointList = new List<AStarGridPoint>();
            int xInt = currentPointPos.GridX;
            int zInt = currentPointPos.GridZ;
            Vector2Grid upPoint = new Vector2Grid(xInt, zInt + GridLength);
            Vector2Grid downPoint = new Vector2Grid(xInt, zInt - GridLength);
            Vector2Grid leftPoint = new Vector2Grid(xInt - GridLength, zInt);
            Vector2Grid rightPoint = new Vector2Grid(xInt + GridLength, zInt);
            Vector2Grid leftUpPoint = new Vector2Grid(xInt - GridLength, zInt + GridLength);
            Vector2Grid leftDownPoint = new Vector2Grid(xInt - GridLength, zInt - GridLength);
            Vector2Grid rightUpPoint = new Vector2Grid(xInt + GridLength, zInt + GridLength);
            Vector2Grid rightDownPoint = new Vector2Grid(xInt + GridLength, zInt - GridLength);
            if (SceneAStarGridDict.ContainsKey(upPoint) && !SceneAStarGridDict[upPoint].IsObstacle && !closeDict.ContainsKey(upPoint))
            {
                surroundPointList.Add(SceneAStarGridDict[upPoint]);
            }
            if (SceneAStarGridDict.ContainsKey(downPoint) && !SceneAStarGridDict[downPoint].IsObstacle && !closeDict.ContainsKey(downPoint))
            {
                surroundPointList.Add(SceneAStarGridDict[downPoint]);
            }
            if (SceneAStarGridDict.ContainsKey(leftPoint) && !SceneAStarGridDict[leftPoint].IsObstacle && !closeDict.ContainsKey(leftPoint))
            {
                surroundPointList.Add(SceneAStarGridDict[leftPoint]);
            }
            if (SceneAStarGridDict.ContainsKey(rightPoint) && !SceneAStarGridDict[rightPoint].IsObstacle && !closeDict.ContainsKey(rightPoint))
            {
                surroundPointList.Add(SceneAStarGridDict[rightPoint]);
            }
            if (SceneAStarGridDict.ContainsKey(leftUpPoint) && !SceneAStarGridDict[leftUpPoint].IsObstacle && !closeDict.ContainsKey(leftUpPoint))
            {
                surroundPointList.Add(SceneAStarGridDict[leftUpPoint]);
            }
            if (SceneAStarGridDict.ContainsKey(leftDownPoint) && !SceneAStarGridDict[leftDownPoint].IsObstacle && !closeDict.ContainsKey(leftDownPoint))
            {
                surroundPointList.Add(SceneAStarGridDict[leftDownPoint]);
            }
            if (SceneAStarGridDict.ContainsKey(rightUpPoint) && !SceneAStarGridDict[rightUpPoint].IsObstacle && !closeDict.ContainsKey(rightUpPoint))
            {
                surroundPointList.Add(SceneAStarGridDict[rightUpPoint]);
            }
            if (SceneAStarGridDict.ContainsKey(rightDownPoint) && !SceneAStarGridDict[rightDownPoint].IsObstacle && !closeDict.ContainsKey(rightDownPoint))
            {
                surroundPointList.Add(SceneAStarGridDict[rightDownPoint]);
            }
            return surroundPointList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AStarGridPoint FindMinFValuePoint(Dictionary<Vector2Grid, AStarGridPoint> openDict)
        {
            float minFValue = float.MaxValue;
            AStarGridPoint minFValuePoint = null;
            foreach (AStarGridPoint point in openDict.Values)
            {
                if (point.F < minFValue)
                {
                    minFValue = point.F;
                    minFValuePoint = point;
                }
            }
            return minFValuePoint;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float CalculateGValue(AStarGridPoint currentPoint, AStarGridPoint nextPoint)
        {
            float gValue = CalculateVector2Dis(currentPoint.X, currentPoint.Z, nextPoint.X, nextPoint.Z) + currentPoint.G;
            return gValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CalculateFValue(AStarGridPoint currentPoint, AStarGridPoint targetPoint)
        {
            //F = G + H
            float hValue = Math.Abs(currentPoint.X - targetPoint.X) + Math.Abs(currentPoint.Z - targetPoint.Z);
            AStarGridPoint parentPoint = currentPoint.Parent;
            float gValue = CalculateVector2Dis(currentPoint.X, currentPoint.Z, parentPoint.X, parentPoint.Z) + parentPoint.G;
            currentPoint.F = gValue + hValue;
            currentPoint.G = gValue;
            currentPoint.H = hValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float CalculateVector2Dis(int x1, int z1, int x2, int z2)
        {
            int xlen = x1 - x2;
            int zlen = z1 - z2;
            return (float)Math.Sqrt(xlen * xlen + zlen * zlen);
        }
    }

    public class AStarGridPoint
    {
        public AStarGridPoint Parent { get; set; }
        public float F { get; set; }
        public float G { get; set; }
        public float H { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public bool IsObstacle { get; set; }
        public AStarGridPoint(int x, int y, int z, bool isObstacle)
        {
            X = x;
            Y = y;
            Z = z;
            IsObstacle = isObstacle;
            Parent = null;
            F = 0;
            G = 0;
            H = 0;
        }
    }
}
