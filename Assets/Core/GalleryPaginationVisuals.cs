using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryPaginationVisuals : MonoBehaviour
{
    public GameObject go_WallSegment;
    public GameObject go_GallerySegment;
    public GameObject go_Painting;

    public GameObject go_EndWall;

    public Transform tpool_Walls; public Transform tpool_Galleries; public Transform tpool_Paintings;
    int currentWall=-1; int currentGallery=-1; int currentPainting=-1;

    void Awake() {
        // create pools
        CreatePool(tpool_Walls, go_WallSegment,GalleryPagination.n_GalleriesPerFloor*4);
        CreatePool(tpool_Galleries, go_GallerySegment, GalleryPagination.n_GalleriesPerFloor * 6);
        CreatePool(tpool_Paintings,go_Painting, GalleryPagination.n_PaintingsPerGallery * GalleryPagination.n_GalleriesPerFloor);
    }

    public void NewFloor()
    {
        // move all pools away
        MovePoolAway(tpool_Walls);
        MovePoolAway(tpool_Galleries);
        MovePoolAway(tpool_Paintings);

        // initialize currents
         currentWall = -1;  currentGallery = -1;  currentPainting = -1;

        // clear progif
        ProGifManager.Instance.Clear();
    }

    public GameObject NewWall()
    {
        currentWall++;
        return NewFromPool(tpool_Walls, currentWall);
    }
    public GameObject NewGallery()
    {
        currentGallery++;
        return NewFromPool(tpool_Galleries, currentGallery);
    }
    public GameObject NewPainting()
    {
        currentPainting++;
        return NewFromPool(tpool_Paintings, currentPainting);
    }

    GameObject NewFromPool(Transform pool,int n)
    { 
        return pool.GetChild(n).gameObject;
    }

    void MovePoolAway(Transform t)
    {
        foreach(Transform c in t)
        {
            c.position = new Vector3(-9999, -9999, -9999);

            if (c.GetComponentInChildren < ReturnToPoolParent >()!= null)
            {
                ReturnToPoolParent[] rtp = c.GetComponentsInChildren<ReturnToPoolParent>();
                for(int i = 0; i < rtp.Length; i++)
                {
                    rtp[i].ReturnMe();
                }
            }
        }
    }

    void CreatePool(Transform t,GameObject go,int N)
    {
        int d = N - t.childCount;
        if(d>0)
            for(int i = 0; i < d; i++)
            {
                GameObject g = Instantiate(go);
                g.transform.parent = t;
                g.transform.position = new Vector3(-9999, -9999, -9999);
                g.GetComponent<ReturnToPoolParent>().SetParent(t);
            }
    }

}
