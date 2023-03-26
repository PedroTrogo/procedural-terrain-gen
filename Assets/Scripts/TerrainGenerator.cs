using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SpawnTrees
{
    public GameObject treePrefab;
    public float spawnRatePercent;
}


[RequireComponent(typeof(MeshCreator))]
public class TerrainGenerator : MonoBehaviour
{
    public TextureGenerator shapeCoordinates; // objeto da classe que gera o relevo do terreno
    public Material terrainTexture; // tetura do terreno
    public SpawnTrees spawnTrees; // probabilidade de spawnar arvores

    MeshCreator meshCreator;
    GameObject chunkObj = null;
    GameObject treeAll = null;

    int chunkCount = 0; // conta quantos chunks existem

    int x = 0, z = 0; // cordenadas x, z (y é oferecido pelo PerlinNoise)
    int g = 0, xX, zZ = 0; // variaveis usadas para gerar os chunks em espiral.
    int trianglesCount = 0;
    public int chunkSize = 10; // tamanho do chunk
    public int heightMultiplier = 6; // multiplicador de altura do terreno
    public AnimationCurve heightMultplierCurve;

    void Awake() // Generate the chunks(a group of blocks)
    {
        meshCreator = GetComponent<MeshCreator>();
        treeAll = new GameObject("TreeAll");
        CreateChunk();
        xX = g;
        z = chunkSize * zZ;
        x = chunkSize * xX;
    }

    void Update()
    {
        if (chunkCount <= Mathf.Pow((shapeCoordinates.height / chunkSize), 2))
            for (int i = 0; i < 100; i++)
                DrawWorld();
    }

    void DrawWorld()
    {
        if (x < (chunkSize * xX + chunkSize))
        {
            if (z < (chunkSize * zZ + chunkSize))
            {
                Vector3 pos = new Vector3(x, Mathf.Round(shapeCoordinates.noiseMap[z, x] * heightMultplierCurve.Evaluate(shapeCoordinates.noiseMap[z, x]) * heightMultiplier), z);
                float lastHy, lastHx;

                InstantiateBlock(pos);

                lastHy = Mathf.Round(shapeCoordinates.noiseMap[z != 0 ? z - 1 : z, x] * heightMultplierCurve.Evaluate(shapeCoordinates.noiseMap[z != 0 ? z - 1 : z, x]) * heightMultiplier);
                lastHx = Mathf.Round(shapeCoordinates.noiseMap[z, x != 0 ? x - 1 : x] * heightMultplierCurve.Evaluate(shapeCoordinates.noiseMap[z, x != 0 ? x - 1 : x]) * heightMultiplier);

                meshCreator.MassMeshGen(pos, ref trianglesCount, lastHy, lastHx);

                z++;
                if (z == (chunkSize * zZ + chunkSize))
                {
                    x++;
                    z = (chunkSize * zZ);
                }
            }
        }
        else
        {
            meshCreator.ApplyMesh(chunkObj, out trianglesCount);
            chunkObj.AddComponent<MeshCollider>();
            CreateChunk();

            if (zZ < g)
                zZ++;
            else if (xX > 0)
                xX--;
            else
            {
                g++;
                zZ = 0;
                xX = g;
            }
            x = (chunkSize * xX);
            z = (chunkSize * zZ);
        }
    }

    void CreateChunk()
    {
        chunkCount++;
        chunkObj = new GameObject("Chunk_" + chunkCount.ToString());//create a empty object
        chunkObj.transform.parent = this.transform;
        //chunkObj.AddComponent<OcclusionCulling>();
        chunkObj.AddComponent<MeshFilter>(); // add mesh filter to new obj
        chunkObj.AddComponent<MeshRenderer>(); // add mesh renderer to new obj
        terrainTexture.mainTexture = shapeCoordinates.texture;
        chunkObj.GetComponent<MeshRenderer>().material = terrainTexture; // add new material to the new obj
    }

    void InstantiateBlock(Vector3 _pos)
    {
        //if(treeSpwn.de>1)
        //treeSpwn.de -= .025f; // make a fade effect on trees spawning.

        if (_pos.y > 5 || _pos.y < 0)
            return;

        if (Random.Range(0, 100) < spawnTrees.spawnRatePercent) // instatiate trees randomly 
        {
            _pos.y += 1.1f;
            GameObject tree = Instantiate(spawnTrees.treePrefab, _pos, Quaternion.identity);
            tree.transform.parent = treeAll.transform; // add trees to the same parent
        }
    }
}