using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartActiveMan : MonoBehaviour
{
    public GameObject[] turnOn;
    public GameObject[] turnOff;

    void Start()
    {
        for(int i = 0; i < turnOff.Length; i++)
        {
            turnOff[i].SetActive(false);
        }
        for (int i = 0; i < turnOn.Length; i++)
        {
            turnOn[i].SetActive(true);
        }
    }
 
}
