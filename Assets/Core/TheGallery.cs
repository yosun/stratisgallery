using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stratisphere;
using UnityEngine.UI; 

public class TheGallery : MonoBehaviour
{
    public StratisphereAPI sapi;
    public GalleryPagination gp;

    public InputField SearchInput;

    void DownloadReturned(Painting[] p)
    {
        gp.InitializeGalleries(p);
    }

    void Start()
    {
        // load stratisphere
        sapi.LoadStratisphere("",24);
        StratisphereAPI.DownloadReturned += DownloadReturned;
    }

    public void Input_SearchNFT()
    {
        SearchNFT(SearchInput.text);
    }

    public void SearchNFT(string search)
    {
        print("SearchNFT " + search);
        sapi.LoadStratisphere(search, 120);
    }

    public void LoadFromUser(string user)
    {
        //TODO

    }
     
}
