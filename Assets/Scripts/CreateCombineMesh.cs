using UnityEngine;
using System.Collections;
using System;

public class CreateCombineMesh : MonoBehaviour {

    public void Combine (GameObject childBlock, GameObject chunk)
    {
        MeshFilter[] meshFilters = chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++){
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            //meshFilters[i].gameObject.SetActive(false); // desativa o mesh de todos os filhos para renderizar menos objetos porem desativa o principal tambem /*/ -> !tirar o comentario dessa linha apenas em caso de problemas de performance!
        }

        chunk.GetComponent<MeshFilter>().mesh = new Mesh();
        chunk.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true);
        chunk.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        chunk.GetComponent<MeshFilter>().mesh.RecalculateNormals();

       // chunk.SetActive(true); // ativa novamente o mesh do objeto principal (que desativado em /*/)para renderizar o terreno

        Destroy(childBlock);
    }
}
