using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GabrielAssignment
{
    public static class NoiseUtility
    {
        
    }

    public static class RandomNoise
    {
        static System.Random rnd = new();
        public static float GetRandomNoise()
        {
            System.Random rnd = new();
            return (float)rnd.Next(0,256) / 255f;
        }
    }

    public static class PerlinNoise
    {
        // Source: https://en.wikipedia.org/wiki/Perlin_noise
        // permutation is repeated for values above 255
        static int[] permutation = {
                      151, 160, 137,  91,  90,  15, 131,  13, 201,  95,  96,  53, 194, 233, 7, 225,
                      140,  36, 103,  30,  69, 142,   8,  99,  37, 240,  21,  10,  23, 190,   6, 148,
                      247, 120, 234,  75,   0,  26, 197,  62,  94, 252, 219, 203, 117,  35,  11,  32,
                       57, 177,  33,  88, 237, 149,  56,  87, 174,  20, 125, 136, 171, 168,  68, 175,
                       74, 165,  71, 134, 139,  48,  27, 166,  77, 146, 158, 231,  83, 111, 229, 122,
                       60, 211, 133, 230, 220, 105,  92,  41,  55,  46, 245,  40, 244, 102, 143,  54,
                       65,  25,  63, 161,   1, 216,  80,  73, 209,  76, 132, 187, 208,  89,  18, 169,
                      200, 196, 135, 130, 116, 188, 159,  86, 164, 100, 109, 198, 173, 186,   3,  64,
                       52, 217, 226, 250, 124, 123,   5, 202,  38, 147, 118, 126, 255,  82,  85, 212,
                      207, 206,  59, 227,  47,  16,  58,  17, 182, 189,  28,  42, 223, 183, 170, 213,
                      119, 248, 152,   2,  44, 154, 163,  70, 221, 153, 101, 155, 167,  43, 172,   9,
                      129,  22,  39, 253,  19,  98, 108, 110,  79, 113, 224, 232, 178, 185, 112, 104,
                      218, 246,  97, 228, 251,  34, 242, 193, 238, 210, 144,  12, 191, 179, 162, 241,
                       81,  51, 145, 235, 249,  14, 239, 107,  49, 192, 214,  31, 181, 199, 106, 157,
                      184,  84, 204, 176, 115, 121,  50,  45, 127,   4, 150, 254, 138, 236, 205,  93,
                      222, 114,  67,  29,  24,  72, 243, 141, 128, 195,  78,  66, 215,  61, 156, 180
        };

        static int[] p = new int[512];
        const int permutationCount = 255;

        private static Vector3[] directions3D = {
            new Vector3( 1f, 1f, 0f),
            new Vector3(-1f, 1f, 0f),
            new Vector3( 1f,-1f, 0f),
            new Vector3(-1f,-1f, 0f),
            new Vector3( 1f, 0f, 1f),
            new Vector3(-1f, 0f, 1f),
            new Vector3( 1f, 0f,-1f),
            new Vector3(-1f, 0f,-1f),
            new Vector3( 0f, 1f, 1f),
            new Vector3( 0f,-1f, 1f),
            new Vector3( 0f, 1f,-1f),
            new Vector3( 0f,-1f,-1f),

            new Vector3( 1f, 1f, 0f),
            new Vector3(-1f, 1f, 0f),
            new Vector3( 0f,-1f, 1f),
            new Vector3( 0f,-1f,-1f)
        };

        private const int directionCount3D = 15;

        private static Vector2[] directions2D = {
            new Vector2(0, 1), 
            new Vector2(1, 1), 
            new Vector2(1, 0), 
            new Vector2(1, -1),
            new Vector2(0, -1), 
            new Vector2(-1, -1), 
            new Vector2(-1, 0), 
            new Vector2(-1, 1)
        };

        private const int directionCount2D = 7;

        // Source: https://www.youtube.com/watch?v=SoakEoUQ7Rg&t=64s
        public static float Get3DPerlinNoise(Vector3 point, float frequency)
        {
            point *= frequency;

            CalculateP();

            int floorPointX0 = Mathf.FloorToInt(point.x);
            int floorPointY0 = Mathf.FloorToInt(point.y);
            int floorPointZ0 = Mathf.FloorToInt(point.z);

            float distanceX0 = point.x - floorPointX0;
            float distanceY0 = point.y - floorPointY0;
            float distanceZ0 = point.z - floorPointZ0;

            float distanceX1 = distanceX0 - 1f;
            float distanceY1 = distanceY0 - 1f;
            float distanceZ1 = distanceZ0 - 1f;

            floorPointX0 &= permutationCount;
            floorPointY0 &= permutationCount;
            floorPointZ0 &= permutationCount;

            int floorPointX1 = floorPointX0 + 1;
            int floorPointY1 = floorPointY0 + 1;
            int floorPointZ1 = floorPointZ0 + 1;

            int permutationX0 = p[floorPointX0];
            int permutationX1 = p[floorPointX1];

            int permutationY00 = p[permutationX0 + floorPointY0];
            int permutationY10 = p[permutationX1 + floorPointY0];
            int permutationY01 = p[permutationX0 + floorPointY1];
            int permutationY11 = p[permutationX1 + floorPointY1];

            float smoothDistanceX = MathUtility.SmoothDistance(distanceX0);
            float smoothDistanceY = MathUtility.SmoothDistance(distanceY0);
            float smoothDistanceZ = MathUtility.SmoothDistance(distanceZ0);

            // 3D, 8 corners per cell
            Vector3 direction000 = directions3D[p[permutationY00 + floorPointZ0] & directionCount3D];
            Vector3 direction100 = directions3D[p[permutationY10 + floorPointZ0] & directionCount3D];
            Vector3 direction010 = directions3D[p[permutationY01 + floorPointZ0] & directionCount3D];
            Vector3 direction110 = directions3D[p[permutationY11 + floorPointZ0] & directionCount3D];
            Vector3 direction001 = directions3D[p[permutationY00 + floorPointZ1] & directionCount3D];
            Vector3 direction101 = directions3D[p[permutationY10 + floorPointZ1] & directionCount3D];
            Vector3 direction011 = directions3D[p[permutationY01 + floorPointZ1] & directionCount3D];
            Vector3 direction111 = directions3D[p[permutationY11 + floorPointZ1] & directionCount3D];

            float value000 = MathUtility.Vector3Dot(direction000, new Vector3(distanceX0, distanceY0, distanceZ0));
            float value100 = MathUtility.Vector3Dot(direction100, new Vector3(distanceX1, distanceY0, distanceZ0));
            float value010 = MathUtility.Vector3Dot(direction010, new Vector3(distanceX0, distanceY1, distanceZ0));
            float value110 = MathUtility.Vector3Dot(direction110, new Vector3(distanceX1, distanceY1, distanceZ0));
            float value001 = MathUtility.Vector3Dot(direction001, new Vector3(distanceX0, distanceY0, distanceZ1));
            float value101 = MathUtility.Vector3Dot(direction101, new Vector3(distanceX1, distanceY0, distanceZ1));
            float value011 = MathUtility.Vector3Dot(direction011, new Vector3(distanceX0, distanceY1, distanceZ1));
            float value111 = MathUtility.Vector3Dot(direction111, new Vector3(distanceX1, distanceY1, distanceZ1));

            return MathUtility.Lerp(
            MathUtility.Lerp(MathUtility.Lerp(value000, value100, smoothDistanceX), MathUtility.Lerp(value010, value110, smoothDistanceX), smoothDistanceY),
            MathUtility.Lerp(MathUtility.Lerp(value001, value101, smoothDistanceX), MathUtility.Lerp(value011, value111, smoothDistanceX), smoothDistanceY),
            smoothDistanceZ);
        }

        // Modified from Get3DPerlinNoise Function
        // Additional resources: https://stackoverflow.com/questions/8659351/2d-perlin-noise
        public static float Get2DPerlinNoise(Vector2 point, float frequency)
        {
            point *= frequency;

            CalculateP();

            int floorPointX0 = Mathf.FloorToInt(point.x);
            int floorPointY0 = Mathf.FloorToInt(point.y);

            float distanceX0 = point.x - floorPointX0;
            float distanceY0 = point.y - floorPointY0;

            float distanceX1 = distanceX0 - 1f;
            float distanceY1 = distanceY0 - 1f;

            floorPointX0 &= permutationCount;
            floorPointY0 &= permutationCount;

            int floorPointX1 = floorPointX0 + 1;
            int floorPointY1 = floorPointY0 + 1;

            int permutationX0 = p[floorPointX0];
            int permutationX1 = p[floorPointX1];

            int permutationY00 = p[permutationX0 + floorPointY0];
            int permutationY10 = p[permutationX1 + floorPointY0];
            int permutationY01 = p[permutationX0 + floorPointY1];
            int permutationY11 = p[permutationX1 + floorPointY1];

            float smoothDistanceX = MathUtility.SmoothDistance(distanceX0);
            float smoothDistanceY = MathUtility.SmoothDistance(distanceY0);

            // 2D, 4 corners per cell
            Vector2 direction000 = directions2D[p[permutationY00] & directionCount2D];
            Vector2 direction100 = directions2D[p[permutationY10] & directionCount2D];
            Vector2 direction010 = directions2D[p[permutationY01] & directionCount2D];
            Vector2 direction110 = directions2D[p[permutationY11] & directionCount2D];

            float value000 = MathUtility.Vector2Dot(direction000, new Vector2(distanceX0, distanceY0));
            float value100 = MathUtility.Vector2Dot(direction100, new Vector2(distanceX1, distanceY0));
            float value010 = MathUtility.Vector2Dot(direction010, new Vector2(distanceX0, distanceY1));
            float value110 = MathUtility.Vector2Dot(direction110, new Vector2(distanceX1, distanceY1));

            return MathUtility.Lerp(
                MathUtility.Lerp(value000, value100, smoothDistanceX), 
                MathUtility.Lerp(value010, value110, smoothDistanceX),
                smoothDistanceY
            );
        }

        public static void CalculateP()
        {
            p = new int[permutation.Length * 2];

            for (int i = 0; i < 256; i++)
            {
                p[256 + i] = p[i] = permutation[i];
            }
        }
    }
    public enum NoiseType
    {
        RandomNoise,
        Perlin,
        Simplex
    }
}


