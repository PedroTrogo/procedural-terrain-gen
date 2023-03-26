using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionCulling : MonoBehaviour
{
    bool isVisible = false;

    void Update()
    {
        isVisible = IsVisibleFrom(transform.GetComponent<Renderer>(), Camera.main);

        if (isVisible)
        {
            if (this.gameObject.GetComponent<MeshCollider>() != null)
                this.gameObject.GetComponent<MeshCollider>().enabled = true;

            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            if (this.gameObject.GetComponent<MeshCollider>() != null)
                this.gameObject.GetComponent<MeshCollider>().enabled = false;

            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}
