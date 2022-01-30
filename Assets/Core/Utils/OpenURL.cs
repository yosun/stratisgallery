using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public string baseURL = "https://";

    public string myFinalURL;

    public void Init()
    {
        myFinalURL = baseURL + transform.name;
    }

    public void OpenName()
    {
        Application.OpenURL(myFinalURL);
    }
}
