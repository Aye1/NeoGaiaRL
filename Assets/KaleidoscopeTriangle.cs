using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaleidoscopeTriangle : MonoBehaviour {

    MeshFilter msFilter;

    // Use this for initialization
    void Start () {
        msFilter = GetComponent<MeshFilter>();
        CreateMesh();
	}
	
    private void CreateMesh()
    {
        Mesh mesh = new Mesh();
        GenerateVertices(mesh);
        GenerateTriangles(mesh);
        //GenerateColors(mesh);
        GenerateUV(mesh);
        msFilter.mesh = mesh;
    }

	// Update is called once per frame
	void Update () {
		
	}

    private void GenerateVertices(Mesh mesh)
    {
        Vector3[] vertices = new Vector3[3];
        vertices[0] = new Vector3(0.0f, 0.0f, 0.0f);
        vertices[1] = new Vector3(1.0f, 0.0f, 0.0f);
        vertices[2] = new Vector3(0.5f, Mathf.Sqrt(0.75f), 0.0f);
        mesh.vertices = vertices;
    }

    private void GenerateTriangles(Mesh mesh)
    {
        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        mesh.triangles = triangles;
    }

    private void GenerateColors(Mesh mesh)
    {
        Color[] colors = new Color[3];
        colors[0] = Color.red;
        colors[1] = Color.green;
        colors[2] = Color.blue;
        mesh.colors = colors;
    }

    private void GenerateUV(Mesh mesh)
    {
        int nbVertices = mesh.vertices.Length;
        Vector2[] newUV = new Vector2[nbVertices];
        for (int i = 0; i < nbVertices; i++)
        {
            newUV[i] = (Vector2)mesh.vertices[i];
        }
        mesh.uv = newUV;
    }
}
