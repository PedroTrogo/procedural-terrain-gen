using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teste : MonoBehaviour
{
    public GameObject block;
    public int w, l;

    [Range(1,100)]
    public float scale = 2;
    GameObject[,] blocks;

    public float jumpForce = 0;

    Vector2 transformPos;

    Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        blocks = new GameObject[w,l];

        for(int x=0; x<w;x++)
            for(int z =0;z<l;z++)
                blocks[x, z] = Instantiate(block, new Vector3(x, 0, z), Quaternion.identity);  
    }

    // Update is called once per frame
    void Update()
    {
        Perlin();

        transformPos += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(Input.GetButtonDown("Jump"))
            body.AddForce(jumpForce * Vector3.up * Time.deltaTime, ForceMode.Impulse);
    }

    void Perlin()
    {
        for(int x=0; x< w; x++)
            for(int z =0; z<l; z++)
            {
                Vector3 pos = blocks[x, z].transform.position;
                pos.y = 10 * Mathf.PerlinNoise((float)((x+ transformPos.x)/(Mathf.Sqrt(w)*scale)), (float)((z+ transformPos.y)/(Mathf.Sqrt(l)*scale)));
                blocks[x, z].transform.position = pos;
            }
    }

    void MeshGen()
    {
        
    }
}
