using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPoolParent : MonoBehaviour
{
    Transform poolParent;

    public void SetParent(Transform t)
    {
        poolParent = t;
    }

    public void ReturnMe()
    {
        transform.parent = poolParent;
    }
     
}
