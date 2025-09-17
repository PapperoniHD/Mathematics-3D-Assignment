using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TerrainGenerator noiseScript = (TerrainGenerator)target;

        if (GUILayout.Button("Create Terrain Mesh"))
        {
            noiseScript.CreateMesh();
        }

    }
}
