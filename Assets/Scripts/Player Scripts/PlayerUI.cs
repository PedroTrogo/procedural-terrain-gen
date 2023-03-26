using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject canvas;
    Text[] cord = new Text[3];

    void Start()
    {
        cord[0] = GetChildByName("X", canvas).GetComponent<Text>();
        cord[1] = GetChildByName("Z", canvas).GetComponent<Text>();
        cord[2] = GetChildByName("Y", canvas).GetComponent<Text>();
    }

    void Update()
    {
        cord[0].text = "X" + Mathf.RoundToInt(transform.position.x).ToString();
        cord[1].text = "Z" + Mathf.RoundToInt(transform.position.z).ToString();
        cord[2].text = "Y" + Mathf.RoundToInt(transform.position.y - 1.7f).ToString();
    }

    GameObject GetChildByName(string name, GameObject parent)
    {
        GameObject obj = null;
        Transform[] objs = parent.transform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < objs.Length; i++)
            if (objs[i].name == name)
                obj = objs[i].gameObject;

        return obj;
    }
}
