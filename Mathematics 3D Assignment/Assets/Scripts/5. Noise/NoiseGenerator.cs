using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GabrielAssignment;

public class NoiseGenerator : MonoBehaviour
{
    public RawImage image;
    int width = 256;
    int height = 256;

    [Range(0,1)]
    [SerializeField] private float frequency = 0.5f;

    public void GenerateRandomNoiseMap()
    {
        Texture2D tex= new(width, height);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                float n = RandomNoise.GetRandomNoise();

                tex.SetPixel(j, i, new Color(n,n,n));
            }
        }

        tex.Apply();

        image.texture = tex;
    }

    public void GeneratePerlinNoise()
    {
        Texture2D tex = new(width, height);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector2 pixelVector = new(j, i);

                float n = PerlinNoise.Get2DPerlinNoise(pixelVector, frequency);

                float nNormalized = (n + 1f) / 2f;

                tex.SetPixel(j, i, new Color(nNormalized, nNormalized, nNormalized));
            }
        }

        tex.Apply();

        image.texture = tex;
    }
}
