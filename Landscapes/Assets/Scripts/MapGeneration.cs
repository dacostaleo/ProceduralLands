using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {

    public enum DrawMode {NoiseMap, ColorMap, Mesh}

    public DrawMode drawMode;

    const int mapChunkSize = 241;

    [Range(0,6)]
    public int levelOfDetail;
    //public int mapWidth;
    //public int mapHeight;
    public float noiseScale;


    public int octaves;

    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool refresh;

    public float hightMult = 10;
    public AnimationCurve curveHeight;
    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorRegions = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int k = 0; k < regions.Length; k++)
                {
                    if (currentHeight <= regions[k].height)
                    {
                        colorRegions[y * mapChunkSize + x] = regions[k].color;
                        break;
                    }
                }
            }
        }

        DisplayMap display = FindObjectOfType<DisplayMap>();

        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorRegions, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, hightMult, curveHeight, levelOfDetail), TextureGenerator.TextureFromColorMap(colorRegions, mapChunkSize, mapChunkSize));
    }

    void OnValidate()
    {
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