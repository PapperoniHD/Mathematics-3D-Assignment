using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseGrid3D))]
public class NoiseGrid3DEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NoiseGrid3D noiseScript = (NoiseGrid3D)target;

        if (GUILayout.Button("Generate Cubes"))
        {
            noiseScript.GenerateCubes();
        }
        if (GUILayout.Button("Generate Cube Noise Material"))
        {
            noiseScript.GenerateCubeNoise();
        }
    }
}
