using UnityEngine;

namespace GabrielAssignment
{
    /// <summary>
    /// Custom Math Utility
    /// This is way slower than just using Mathf, 
    /// but I thought it would be fun to create my own and also to learn more.
    /// </summary>
    public static class MathUtility
    {
        // Easing Functions
        public static float EaseInCubic(float t)
        {
            return t * t * t;
        }
        public static float EaseOutCubic(float t)
        {
            return 1 - Pow(1 - t, 3);
        }
 
        public static float EaseInOutCubic(float t)
        {
            return t < 0.5f ? 4 * t * t * t : 1 - Pow(-2 * t + 2, 3) / 2;
        }

        public static float EaseInSine(float t)
        {
            return 1 - Cos((t * PI) / 2);
        }

        public static float EaseOutSine(float t)
        {     
            return Sine((t * PI) / 2);
        }

        // Math functions
        public static float PI = 3.14159274f;

        // Source: https://www.youtube.com/watch?v=cmz4GLXx6xY&ab_channel=DigiDev
        public static float Pow(float f, int p) // Fast loop
        {
            if (p == 0) return 1;

            float result = 1;
            while (p != 0)
            {
                p--;
                result *= f;
            }
            return result;
        }

        public static float Pow(float f, float p) // General formula but slower
        {
            if (f == 0 && p == 0) return 1;
            if (f == 0) return 0;

            return Exp(p * Log(f));
        }

        public static float Fact(float f)
        {
            if (f == 0)
            {
                return 1;
            }

            return f * (Fact(f - 1));
        }
        // Source: https://www.youtube.com/watch?v=SoakEoUQ7Rg&t=64s
        public static float SmoothDistance(float d)
        {
            return d * d * d * (d * (d * 6f - 15f) + 10f);
        }
        public static float Vector3Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static float Vector2Dot(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static float Fmod(float a, float b)
        {
            float frac = a / b;
            int floor = frac > 0 ? (int)frac : (int)(frac - 0.9999999999999999);
            return a - b * floor;
        }
        // Source: https://stackoverflow.com/questions/77723846/implementing-a-cosine-function-in-c
        public static float Cos(float f)
        {
            float f2 = f * f;
            return 1
                - f2 / 2f
                + (f2 * f2) / 24f
                - (f2 * f2 * f2) / 720f
                + (f2 * f2 * f2 * f2) / 40320f;
        }
        // Source: https://stackoverflow.com/questions/48758757/creating-a-custom-sine-function
        public static float Sine(float f)
        {
            float f2 = f * f;
            return f
                - (f * f2) / 6f
                + (f * f2 * f2) / 120f
                - (f * f2 * f2 * f2) / 5040f
                + (f * f2 * f2 * f2 * f2) / 362880f;
        }

        public static float Log(float x)
        {
            if (x <= 0)
            {
                return 0;
            }

            float y = (x - 1) / (x + 1);
            float y2 = y * y;

            float result = 0.0f;
            float term = y;

            for (int i = 1; i < 20; i += 2) 
            {
                result += term / i;
                term *= y2;
            }

            return 2 * result;
        }

        public static float Exp(float x)
        {
            float sum = 1.0f; 
            float term = 1.0f;

            for (int i = 1; i < 20; i++) 
            {
                term *= x / i;
                sum += term;
            }

            return sum;
        }

        // Source: https://www.ascensiongamedev.com/topic/6417-linear-interpolation-explained/
        public static float Lerp(float start, float end, float t)
        {
            return start + (end - start) * t;
        }

        public static Vector3 VectorLerp(Vector3 start, Vector3 end, float t)
        {
            return new Vector3(
                Lerp(start.x, end.x, t), 
                Lerp(start.y, end.y, t), 
                Lerp(start.z, end.z, t));
        }

        public static Color ColorLerp(Color start, Color end, float t)
        {
            return new Color(
                Lerp(start.r, end.r, t), 
                Lerp(start.g, end.g, t), 
                Lerp(start.b, end.b, t), 
                Lerp(start.a, end.a, t));
        }
    }
} 

