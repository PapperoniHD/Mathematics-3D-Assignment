using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GabrielAssignment;

public class NoiseGrid3D : MonoBehaviour
{
    public GameObject cubePrefab;
    public int gridSize = 5;
    public float cubeOffset = 1f;

    [SerializeField] private List<GameObject> cubes = new();

    [Range(0, 1)]
    [SerializeField] private float frequency = 0.5f;
    public void GenerateCubes()
    {
        foreach (var cube in cubes)
        {
            Destroy(cube);
        }

        cubes.Clear();

        GameObject parentGameObject = new GameObject("Cube Grid");

        parentGameObject.transform.parent = this.transform;
        parentGameObject.transform.localPosition = Vector3.zero;


        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    GameObject cube = Instantiate(cubePrefab, parentGameObject.transform);

                    Vector3 pos = new(x, y, z);
                    pos *= cubeOffset;

                    cube.transform.localPosition = pos;

                    cubes.Add(cube);
                }
            }
        }
    }

    public void GenerateCubeNoise()
    {
        foreach (var cube in cubes)
        {
            Renderer renderer = cube.GetComponent<MeshRenderer>();

            renderer.sharedMaterial = new Material(renderer.sharedMaterial);

            Material mat = renderer.sharedMaterial;
           
            float n = PerlinNoise.Get3DPerlinNoise(cube.transform.position, frequency);

            float nNormalized = (n + 1f) / 2f;

            Color color = new(nNormalized, nNormalized, nNormalized, nNormalized);

            mat.color = color;
        }
    }
}
