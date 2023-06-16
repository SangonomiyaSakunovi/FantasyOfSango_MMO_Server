using System;
using System.Runtime.CompilerServices;

//Developer : SangonomiyaSakunovi

namespace SangoMMOCommons.Structs
{
    public struct Vector3Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector3Position a, Vector3Position b)
        {
            float num1 = a.X - b.X;
            float num2 = a.Y - b.Y;
            float num3 = a.Z - b.Z;
            return (float)Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Position Translate(Vector3Position currentPosition, Vector3Position targetPosition, float moveDis)
        {
            float num1 = targetPosition.X - currentPosition.X;
            float num2 = targetPosition.Y - currentPosition.Y;
            float num3 = targetPosition.Z - currentPosition.Z;
            float length = (float)Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
            float numX = moveDis * num1 / length;
            float numY = moveDis * num2 / length;
            float numZ = moveDis * num3 / length;
            return new Vector3Position(currentPosition.X + numX, currentPosition.Y + numY, currentPosition.Z + numZ);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Length(Vector3Position value)
        {
            return (float)Math.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Position Normalize(Vector3Position value)
        {
            float num = (float)Math.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
            return new Vector3Position(value.X / num, value.Y / num, value.Z / num);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Position EdgePosition(Vector3Position edge, Vector3Position mid)
        {
            return new Vector3Position(2 * mid.X - edge.X, 2 * mid.Y - edge.Y, 2 * mid.Z - edge.Z);
        }

        public Vector3Position(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }
    }
    public struct QuaternionRotation
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public QuaternionRotation(float x, float y, float z, float w)
        {
            X = x; Y = y; Z = z; W = w;
        }
    }
}
