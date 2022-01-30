using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenURLOnHit : MonoBehaviour
{
     
    public Camera cam;
    public RawImage rawImage;
    public RectTransform rt;

    public string tag = "Painting";

    // Update is called once per frame
    public void TestButtonHit()
    {

        bool inside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt,
            Input.mousePosition,
            null,
            out Vector2 pointInRect
        );  

        Vector2 textureCoord = pointInRect - rt.rect.min;
        textureCoord.x *= rawImage.uvRect.width / rt.rect.width;
        textureCoord.y *= rawImage.uvRect.height / rt.rect.height;
        textureCoord += rawImage.uvRect.min;
        textureCoord = new Vector2(textureCoord.x * rt.transform.localScale.x,textureCoord.y*rt.transform.localScale.y);

        Ray ray = cam.ViewportPointToRay(textureCoord);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, 1000f))
            {
                if (hit.transform != null)
                {
                    print(hit.transform.name);
                    if (hit.transform.GetComponentInChildren<OpenURL>() != null && hit.transform.tag == tag)
                    {
                        string url = hit.transform.GetComponentInChildren<OpenURL>().myFinalURL;
                        Application.OpenURL(url);
                    }
                }
            }
        }
    
}
