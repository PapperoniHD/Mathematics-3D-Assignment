using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseGenerator))]
public class NoiseGeneratorEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NoiseGenerator noiseScript = (NoiseGenerator)target;

        if (GUILayout.Button("Generate Random Noise Map"))
        {
            noiseScript.GenerateRandomNoiseMap();
        }
        if (GUILayout.Button("Generate Perlin Noise Map"))
        {
            noiseScript.GeneratePerlinNoise();
        }
    }
}
