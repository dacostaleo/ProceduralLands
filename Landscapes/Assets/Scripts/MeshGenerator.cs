using UnityEngine;
using System.Collections;

public static class MeshGenerator {

    public static MeshData GenerateTerrainMesh(float[,] heightMap, float hightMult, AnimationCurve curveHeight, int lod)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        int topLeftX = (width - 1) / -2;
        int topLeftZ = (height - 1) / 2;

        int meshSimplification = (lod == 0) ? 1 : (lod * 2);
        int vertexPerLine = (width - 1) / meshSimplification + 1;


        MeshData meshData = new MeshData(vertexPerLine, vertexPerLine);
        int vertexIndex = 0;


        for (int y = 0; y < height; y+= meshSimplification)
        {
            for (int x = 0; x < width; x+=meshSimplification)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, curveHeight.Evaluate(heightMap[x, y]) * hightMult, topLeftZ - y);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + vertexPerLine + 1, vertexIndex + vertexPerLine);
                    meshData.AddTriangle(vertexIndex + 1, vertexIndex + vertexPerLine + 1, vertexIndex); //DIFERENTE
                }

                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                vertexIndex++;
            }
        }

        return meshData;
    }


	
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;

    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        uvs = new Vector2[vertices.Length];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        
        return mesh;
    }
}
