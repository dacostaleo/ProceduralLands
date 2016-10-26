using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {

    public enum DrawMode {NoiseMap, ColorMap, Mesh}

    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;


    public int octaves;

    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool refresh;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorRegions = new Color[mapHeight * mapWidth];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int k = 0; k < regions.Length; k++)
                {
                    if (currentHeight <= regions[k].height)
                    {
                        colorRegions[y * mapWidth + x] = regions[k].color;
                        break;
                    }
                }
            }
        }

        DisplayMap display = FindObjectOfType<DisplayMap>();

        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorRegions, mapWidth, mapHeight));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.TextureFromColorMap(colorRegions, mapWidth, mapHeight));
    }

    void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;
        if (mapHeight < 1)
            mapHeight = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }
	
}

[System.Serializable]
public struct TerrainType
{
    public string name;

    public float height;
    public Color color;
}