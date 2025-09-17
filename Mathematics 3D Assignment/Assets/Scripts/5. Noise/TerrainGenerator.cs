using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GabrielAssignment;
using System;


// Source: https://www.youtube.com/watch?v=64NblGkAabk
[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    [Header("Noise")]
    [Range(1, 10f)]
    public float heightMultiplier = 2f;
    [Range(0, 1f)]
    public float noiseFrequency = 0.3f;

    private float scrollX = 0;
    private float scrollZ = 0;

    [Header("Scroll")]
    public bool scroll = true;
    public float xScrollSpeed = 1.2f;
    public float zScrollSpeed = -0.5f;

    [Header("Collider")]
    public bool generateCollider = false;
    private MeshCollider meshCollider = null;

    public void CreateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
        if (generateCollider)
        {
            GenerateCollider();
        }
       
    }


    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                if (scroll)
                {
                    Vector2 point1 = new(transform.position.x + x, transform.position.z + z + scrollZ);
                    Vector2 point2 = new(transform.position.x + x + scrollX, transform.position.z + z);

                    float y1 = PerlinNoise.Get2DPerlinNoise(point1, noiseFrequency) * heightMultiplier;
                    float y2 = PerlinNoise.Get2DPerlinNoise(point2, noiseFrequency) * heightMultiplier;

                    vertices[i] = new Vector3(x, y1 + y2, z);
                }
                else
                {
                    Vector2 point = new(transform.position.x + x + scrollX, transform.position.z + z + scrollZ);

                    float y = PerlinNoise.Get2DPerlinNoise(point, noiseFrequency) * heightMultiplier;

                    vertices[i] = new Vector3(x, y, z);
                }
                
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        

        for (int i = 0; i < zSize; i++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

    }

    private void GenerateCollider()
    {
        // Very expensive collider generation, would make a new mesh and reduce the triangle count. 

        if (GetComponent<MeshCollider>() == null)
        {
            this.gameObject.AddComponent(typeof(MeshCollider));       
        }
        meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    private void FixedUpdate()
    {
        if (!scroll) return;
        scrollX += Time.deltaTime * xScrollSpeed;
        scrollZ += Time.deltaTime * zScrollSpeed;
        CreateMesh();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(transform.position + vertices[i], 0.1f);
        }
    }

    public Vector2 GetScroll()
    {
        return new Vector2(scrollX, scrollZ);
    }

    public void SetRandomScroll()
    {
        scrollX = UnityEngine.Random.Range(0, 100);
        scrollZ = UnityEngine.Random.Range(0, 100);
    }
}
