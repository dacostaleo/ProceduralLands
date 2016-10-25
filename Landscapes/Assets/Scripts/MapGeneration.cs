using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;

    public float noiseScale;

    public bool refresh;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);

        DisplayMap display = FindObjectOfType<DisplayMap>();
        display.DrawNoiseMap(noiseMap);
    }
	
}
