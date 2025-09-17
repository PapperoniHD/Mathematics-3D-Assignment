using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GabrielAssignment;

public class MushroomGenerator : MonoBehaviour
{
    public Transform[] mushrooms;
    public TerrainGenerator terrain;

    private void Start()
    {
        GenerateArea();
    }

    public void GenerateArea()
    {
        terrain.SetRandomScroll();
        Vector2 scroll = terrain.GetScroll();

        foreach (var mushroom in mushrooms)
        {

            Vector3 pos = mushroom.transform.position;

            Vector2 point = new Vector2(mushroom.position.x + scroll.x, mushroom.position.z + scroll.y);
            float mushroomYOffset = 1.6f;
            float terrainOffset = terrain.transform.position.y - mushroom.position.y;

            float y = PerlinNoise.Get2DPerlinNoise(point, terrain.noiseFrequency) * terrain.heightMultiplier;
            y += terrainOffset;
            y += mushroomYOffset;
            mushroom.transform.position = new Vector3(pos.x, y, pos.z);
        }

        terrain.CreateMesh();
    }
}
