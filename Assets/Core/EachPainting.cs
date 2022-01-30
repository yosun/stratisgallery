using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.IO;

public class EachPainting : MonoBehaviour
{
    Renderer myRenderer; 

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
    }

    public void LoadImage(string url)
    {
        GetComponentInChildren<OpenURL>().Init();

        print("LoadImage " + url);
        // stratisphere images do not have a file extension 
        //string ext = Path.GetExtension(url).ToLower();
        LoadGif(url);
    } 
    void LoadGif(string url)
    {
        // modified to accomodate with texture2d when gif fail occurs
        //ProGifManager.Instance.PlayGif(url, myRenderer);//, Action<float>: onLoading, bool: shouldSaveFromWeb);
        PGif.iPlayGif(url,myRenderer,transform.name);
    }
    
}
