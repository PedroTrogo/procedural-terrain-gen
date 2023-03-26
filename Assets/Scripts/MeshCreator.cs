using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    Mesh mesh;

   /* Vector2[] uv = new Vector2[4]{
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),
    };*/

    //Cria uma malha pequena para testes
    /*private void PStart()
    {
        gameObject.AddComponent<MeshFilter>(); // add mesh filter to new obj
        gameObject.AddComponent<MeshRenderer>(); // add mesh renderer to new obj
        gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        int tris = 0;
        float[,] y = new float[100, 100];

        for(int x = 0; x < 100; x++)
            for(int z= 0; z< 100; z++)
            {
                y[x, z] = Random.Range(0, 4);
                MassMeshGen(new Vector3(x, y[x, z], z), ref tris, y[x, z!=0?z-1:z], y[x!=0?x-1:x, z]);
            }

        ApplyMesh(this.gameObject, out tris);
    }*/

    public void MassMeshGen(Vector3 pos, ref int tris, float lastHeightY, float lastHeightX)
    {
        int t = tris; //variavel auxiliar para modificar o contador de triangulos

        // verifica a diferença de altura do bloco anterior para cobrir toda extensao do relevo
        if ((pos.y - lastHeightY) > 0) 
            for (float yy = lastHeightY; yy < pos.y; yy++)
                BackFace(new Vector3(pos.x, yy + 1, pos.z), ref t);
        else if((pos.y - lastHeightY) < 0)
            for (float yy = lastHeightY; yy > pos.y; yy--)
                FrontFace(new Vector3(pos.x, yy, pos.z -1), ref t);

        if ((pos.y - lastHeightX) > 0)
            for (float yy = lastHeightX; yy < pos.y; yy++)
                LeftFace(new Vector3(pos.x, yy+1, pos.z), ref t);
        else if ((pos.y - lastHeightX) < 0)
            for (float yy = lastHeightX; yy > pos.y; yy--)
                RightFace(new Vector3(pos.x-1, yy, pos.z), ref t);

        UpFace(new Vector3(pos.x, pos.y, pos.z), ref t);

        tris = t;
    }

    public void ApplyMesh(GameObject obj, out int tris) // Aplica o novo mesh ao gameObject
    {
        mesh = new Mesh();
        mesh.Clear();

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.Optimize();
        mesh.UploadMeshData(true);
        obj.GetComponent<MeshFilter>().mesh = mesh;

        //limpa a informação do mesh anterior e reseta a contagem de triangulos
        vertices.Clear();
        triangles.Clear();
        normals.Clear();
        uvs.Clear();
        tris = 0;
    }

    void BackFace(Vector3 posAdder, ref int trisMultiplier)
    {
        Vector3[] verts = new Vector3[4]
        {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,0,0),
            new Vector3(1,1,0),
        };

        int[] tris = new int[6] { 0, 1, 2, 2, 1, 3 };

        Vector2 uv = new Vector2(posAdder.x/500, posAdder.z/500);

        for (int i = 0; i < verts.Length; i++)
        {
            vertices.Add(verts[i] + posAdder);
            normals.Add(Vector3.back);
            uvs.Add(uv);
        }

        for (int i = 0; i < tris.Length; i++)
            triangles.Add(tris[i] + trisMultiplier);

        trisMultiplier += 4;
    }

    void UpFace(Vector3 posAdder, ref int trisMultiplier)
    {
        Vector3[] verts = new Vector3[4]
        {
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,0),
            new Vector3(1,1,1),
        };

        int[] tris = new int[6] { 0, 1, 2, 2, 1, 3 };

        Vector2 uv = new Vector2(posAdder.x / 500, posAdder.z / 500);

        for (int i = 0; i < verts.Length; i++)
        {
            vertices.Add(verts[i] + posAdder);
            normals.Add(Vector3.up);
            uvs.Add(uv);
        }

        for (int i = 0; i < tris.Length; i++)
            triangles.Add(tris[i] + trisMultiplier);

        trisMultiplier += 4;
    }

    void FrontFace(Vector3 posAdder, ref int trisMultiplier)
    {
        Vector3[] verts = new Vector3[4]
        {
            new Vector3(1,0,1),
            new Vector3(1,1,1),
            new Vector3(0,0,1),
            new Vector3(0,1,1)
        };

        int[] tris = new int[6] { 0, 1, 2, 2, 1, 3 };

        Vector2 uv = new Vector2(posAdder.x / 500, posAdder.z / 500);


        for (int i = 0; i < verts.Length; i++)
        {
            vertices.Add(verts[i] + posAdder);
            normals.Add(Vector3.forward);
            uvs.Add(uv);
        }

        for (int i = 0; i < tris.Length; i++)
            triangles.Add(tris[i] + trisMultiplier);

        trisMultiplier += 4;
    }

    void LeftFace(Vector3 posAdder, ref int trisMultiplier)
    {
        Vector3[] verts = new Vector3[4]
        {
            new Vector3(0,0,1),
            new Vector3(0,1,1),
            new Vector3(0,0,0),
            new Vector3(0,1,0),
        };

        int[] tris = new int[6] { 0, 1, 2, 2, 1, 3 };

        Vector2 uv = new Vector2(posAdder.x / 500, posAdder.z / 500);

        for (int i = 0; i < verts.Length; i++)
        {
            vertices.Add(verts[i] + posAdder);
            normals.Add(Vector3.left);
            uvs.Add(uv);
        }

        for (int i = 0; i < tris.Length; i++)
            triangles.Add(tris[i] + trisMultiplier);

        trisMultiplier += 4;
    }

    void RightFace(Vector3 posAdder, ref int trisMultiplier)
    {
        Vector3[] verts = new Vector3[4]
        {
            new Vector3(1,0,0),
            new Vector3(1,1,0),
            new Vector3(1,0,1),
            new Vector3(1,1,1),
        };

        int[] tris = new int[6] { 0, 1, 2, 2, 1, 3 };

        Vector2 uv = new Vector2(posAdder.x / 500, posAdder.z / 500);

        for (int i = 0; i < verts.Length; i++)
        {
            vertices.Add(verts[i] + posAdder);
            normals.Add(Vector3.right);
            uvs.Add(uv);
        }

        for (int i = 0; i < tris.Length; i++)
            triangles.Add(tris[i] + trisMultiplier);

        trisMultiplier += 4;
    }
}
