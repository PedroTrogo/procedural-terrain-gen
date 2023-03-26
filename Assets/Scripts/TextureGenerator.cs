using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GroundLevels{
    public string name;
    [Range(0, 1)]
    public float height;
    public Color lowColor;
    public Color highColor;
}

public class TextureGenerator : MonoBehaviour
{
    [HideInInspector]
    public Texture2D texture;
    public int height;
    public int width;
    [Range(1, 10)]
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    [Range(1, 10)]
    public float lacunarity;
    [Range(1, 100)]
    public float scale;
    public bool greyScale = false;
    public GroundLevels[] groundLevels;
    
    Renderer textureRender;
    int colorInx = 0;
    float minColor, maxColor;

    [Range(0, 1000)]
    public float seed = 0;  

    [HideInInspector]
    public float [,] noiseMap;

    public void StartFromEditor()
    {
        textureRender = GetComponent<Renderer>();
        noiseMap = NoiseGenerator(width, height, scale, octaves, persistance, lacunarity);
        DrawTexture();
    }

    void Awake ()
    {
        textureRender = GetComponent<Renderer>();
        noiseMap = NoiseGenerator(width, height, scale, octaves, persistance, lacunarity);
        DrawTexture();
    }
    

    public void DrawTexture()
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        Color[] pixelColor = new Color[width*height];

        for(int x = 0; x < height; x++)
        {
            for(int y = 0; y < width; y++)
            {
                if(!greyScale)
                {
                    if(noiseMap[x,y] <= groundLevels[0].height)
                        colorInx = 0;
                    else if(noiseMap[x,y] <= groundLevels[1].height)
                        colorInx = 1;
                    else if(noiseMap[x,y] <= groundLevels[2].height)
                        colorInx = 2;
                    else if(noiseMap[x,y] <= groundLevels[3].height)
                        colorInx = 3;
                    else
                        colorInx = 4;

                    if(colorInx > 0)
                        pixelColor[(x*width)+y] = Color.Lerp(groundLevels[colorInx].lowColor, groundLevels[colorInx].highColor, MathP.TowardsProportional(noiseMap[x,y], groundLevels[colorInx-1].height, groundLevels[colorInx].height, 0, 1));
                    else
                        pixelColor[(x*width)+y] = Color.Lerp(groundLevels[colorInx].lowColor, groundLevels[colorInx].highColor, MathP.TowardsProportional(noiseMap[x,y], 0, groundLevels[colorInx].height, 0, 1));
                }
                else
                    pixelColor[(x * width) + y] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        
        texture.SetPixels(pixelColor);
        texture.Apply();
        textureRender.sharedMaterial.mainTexture = texture;
    }

    public float [,] NoiseGenerator(int mapWidth, int mapHeight, float noiseScale, int octaves, float persistance, float lacunarity)
    {
        float [,] noiseMap_local = new float[mapWidth,mapHeight];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for(int x = 0; x < mapHeight; x++)
        {
            for(int y = 0; y < mapWidth; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i =0; i<octaves; i++)
                {
                    float sampleX = (x+seed)/scale * frequency;
                    float sampleY = (y+seed)/scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue*amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if(noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if(noiseHeight<minNoiseHeight)
                    minNoiseHeight = noiseHeight;
                    
                noiseMap_local[x,y] = noiseHeight;
            }
        }
        for(int x = 0; x < mapHeight; x++)
            for(int y = 0; y < mapWidth; y++)
                noiseMap_local[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap_local[x, y]);

        return noiseMap_local;
    }

}
