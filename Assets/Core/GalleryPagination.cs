using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * GalleryPagination is a class with a physical analogy method for traditional pagination in a 3D gallery space
 * 
 * The big ideas/analogies: 
 * 1) Each "Painting" represents an image
 * 2) Each "Gallery" represents a set of n=12 Paintings (2 sides per "Wall segment" and 2  3 painting Wall segments - per "Gallery segment") 
 * 3) Each "Floor" represent sets of 120 paintings or 10 galleries (just assuming a LCD that you don't want to render more than 120 Paintings at the same time). 
 * 4) The "Elevator" represents Responsive-ish Explicit Loading. You use the Elevator to "paginate" among sets of 120 Paintings, that you can walk through on the same floor. 

 * Visuals are stored in the GalleryPaginationVisuals. I'm an indie dev using Asset Store items for the hackathon. Please supply your own GameObject for each public variable.

 * Usage: 
 * Raw data is stored in Painting[] thePaintings array, which initializes the Galleries object
 */

[System.Serializable]
public class Painting
{
    //public Transform paintingFrame;
    public string tokenId;
    public string contract="";
    public string image;
    public string name;
    public float price;
    public string owner;
    

    public int floor=0;
    public int gallery=0;

    public Painting(string t,string c,string i,string n,float p,string o)
    {
        tokenId = t; contract = c; image = i; name = n; price = p; owner = o;
    }

    public Painting()
    {

    }

    public string DumpMe()
    {
        return JsonUtility.ToJson(this);
    }
}
[System.Serializable]
public class Gallery
{
    public int gallery;
    public Painting[] paintings;
    public GameObject[] goGalleryWalls;

    public Gallery(int g)
    {
        gallery = g;
        Debug.Log("Created Gallery " + g);
    }
}
[System.Serializable]
public class Floor
{
    public int floor;
    public Floor(int f)
    {
        floor = f;
    }
    public Gallery[] galleries;
}

[RequireComponent(typeof(GalleryPaginationVisuals))]
public class GalleryPagination : MonoBehaviour
{

    public const int n_PaintingsPerGallery=12;
    public const int n_GalleriesPerFloor=10;

    public Painting[] thePaintings = new Painting[0];
    public Gallery[] theGalleries = new Gallery[0];
    public Floor[] theFloors = new Floor[0];

    GalleryPaginationVisuals gpv;
    public static int totalFloors; public static int totalGalleries; public static int totalPaintings;

    private void Awake()
    {
        gpv = GetComponent<GalleryPaginationVisuals>();
    }

    public void InitializeGalleries(Painting[] paintings)
    {
        gpv.NewFloor(); // initializes visuals

        int N = paintings.Length; // useful constant

        // initialize totals and a useful catch-all parent array of theFloors
        totalPaintings = N;// thePaintings = paintings;
        totalFloors = Mathf.CeilToInt( (float) N / ( (float) n_GalleriesPerFloor * n_PaintingsPerGallery ) ); theFloors = new Floor[totalFloors];
        totalGalleries = Mathf.CeilToInt( (float) totalPaintings / (float) n_PaintingsPerGallery );// theGalleries = new Gallery[totalGalleries];

        print("Total Floors: " + totalFloors + " Paintings: " + totalPaintings + " Galleries: "+totalGalleries);

        int n = 0;
        for(int i = 0; i < totalFloors; i++)
        {
           
            theFloors[i] = new Floor(i);
            theFloors[i].galleries = new Gallery[ Mathf.Min(totalGalleries - i * n_GalleriesPerFloor, n_GalleriesPerFloor) ];
            
            for (int j = 0; j < theFloors[i].galleries.Length; j++)
            {
                theFloors[i].galleries[j] = new Gallery((j+1)*(i+1));
                theFloors[i].galleries[j].paintings = new Painting[n_PaintingsPerGallery];
                for (int k = 0; k < Mathf.Min(totalPaintings-(i*totalGalleries*n_PaintingsPerGallery),n_PaintingsPerGallery); k++)
                {
                    if (n < N)
                    {
                        theFloors[i].galleries[j].paintings[k] = paintings[n];
                        print("painting n=" + n + " stored in .paintings[k]=" + k);
                        n++;
                    }
                    else break;
                       
                }
            }

            // visuals ---------
            // place endwall - linear equation based on your end wall geometry position
            float x = totalGalleries * 11f + 2;
            gpv.go_EndWall.transform.position = new Vector3(x, 0, 7);

            // generate the rest!
            GenerateFloor(i);
        }
    }

    public void GenerateFloor(int floor)
    {
        Gallery[] galleries = theFloors[floor].galleries;
        for(int j = 0; j < galleries.Length; j++)
        {
            GenerateGallery(j,floor);
        }

        
    }

    void GenerateGallery(int gallery,int floor)
    {
        print("GenerateGallery " + gallery);

        // generate walls
        GameObject wall = gpv.NewWall();
        wall.transform.position = new Vector3(gallery * 11f, 0, -7f);
        wall = gpv.NewWall();
        wall.transform.position = new Vector3(gallery * 11f, 0, 21f);
        wall.transform.localRotation = Quaternion.Euler(0, 180, 0);

        // generate gallery display holders
        Vector3 offset = new Vector3(gallery*10,0,0); // TODO consider parenting to gallery empty game object
        int gallerywalls = Mathf.CeilToInt(n_PaintingsPerGallery * 0.5f);
        theFloors[floor].galleries[gallery].goGalleryWalls = new GameObject[gallerywalls];
        for(int j = 0; j < gallerywalls; j++)
        {
            GameObject g = gpv.NewGallery();
            int col = j % 2;
            int row = j / 2;
            theFloors[floor].galleries[gallery].goGalleryWalls[j] = g;

            g.transform.localPosition = new Vector3(col*5f,0,row*7f) + offset; 
        }

        // plaster walls with paintings!
        int m = 0;
        for (int k = 0; k < Mathf.Min(totalPaintings - ((floor+1) * k * gallery), n_PaintingsPerGallery); k++)
        {
            print(floor + " " + gallery + " " + k);
            if (theFloors[floor].galleries[gallery].paintings[k] != null) {
                GameObject g = GeneratePainting(theFloors[floor].galleries[gallery].paintings[k]);
                PlacePainting(g, theFloors[floor].galleries[gallery].goGalleryWalls[m]);
                theFloors[floor].galleries[gallery].paintings[k].floor = floor;
                theFloors[floor].galleries[gallery].paintings[k].gallery = gallery;
                if ((k + 1) % 2 == 0 && k > 0) m++;
            }
            

        }
    }

    void PlacePainting(GameObject painting, GameObject gallerywall)
    {
        // each gallery wall is currently designed to hold 2 paintings. one behind and one in front.
        painting.transform.parent = gallerywall.transform;
        painting.transform.localPosition = Vector3.zero;
        if (gallerywall.transform.childCount > 1)
        {
            // place it behind
            painting.transform.localRotation *= Quaternion.Euler(0, 180, 0);
            painting.transform.localPosition += new Vector3(0,1.3f,.1f);
        }
        else
        {
            painting.transform.localPosition += new Vector3(0,1.3f,-.1f);
        }
    }

    GameObject GeneratePainting(Painting p)  
    {
        GameObject g = gpv.NewPainting();
         if (p != null) {
            print(p.DumpMe());
            g.name = p.contract + "/" + p.tokenId;

            g.transform.Find("Canvas/Panel Name/Text").GetComponent<Text>().text = p.name;
            g.transform.Find("Canvas/Buy").name = g.name;
            g.transform.Find("Canvas/Panel Price/Text").GetComponent<Text>().text = "CRS"+ p.price;

            EachPainting ep = g.GetComponent<EachPainting>();
            ep.LoadImage(p.image);


        }
        

        return g;
    }

}
