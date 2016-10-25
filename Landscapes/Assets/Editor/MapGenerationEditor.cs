using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(MapGeneration))]
public class MapGenerationEditor : Editor {

    public override void OnInspectorGUI()
    {
        MapGeneration mapGen = target as MapGeneration;

        if (DrawDefaultInspector() && mapGen.refresh)
        {
            mapGen.GenerateMap();
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }

    }
}
