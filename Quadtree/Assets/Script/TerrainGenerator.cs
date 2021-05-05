using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainGenerator
{
	[SerializeField] static int xSize = 4;
	[SerializeField] static int ySize = 4;

	static Mesh mesh;
	static Vector3[] vertices;

	private static void Generate()
	{
		mesh = new Mesh();
		mesh.name = "Procedural Grid";

		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
		for (int i = 0, y = 0; y <= ySize; y++)
		{
			for (int x = 0; x <= xSize; x++, i++)
			{
				vertices[i] = new Vector3(x, y);
			}
		}
		mesh.vertices = vertices;

		int[] triangles = new int[xSize * ySize * 6];
		for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
		{
			for (int x = 0; x < xSize; x++, ti += 6, vi++)
			{
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
			}
		}
		mesh.triangles = triangles;
	}

	[MenuItem("Assets/Create Procedural Mesh")]
	static void Create()
	{
		Generate();

		string filePath = EditorUtility.SaveFilePanelInProject("Save Procedural Mesh", "Procedural Mesh", "asset", "");
		if (filePath == "") return;


		AssetDatabase.CreateAsset(mesh, filePath);
	}
}
