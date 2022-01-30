using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

// This is an informal unofficial API that I've deduced for last minute use at the stratis hackathon on devpost
// https://stratisphere.com/api/asset?page=1&pageSize=120&searchText=&startPrice=0&endPrice=0&status=0&orderBy=&orderDirection=
namespace Stratisphere
{
    public class StratisphereJSON
    {
        public int count;
        public int page;
        public int pageSize;
        public item[] items;
    }
    [System.Serializable]
    public class item
    {
        public string contract;
        public string tokenId;
        public string name;
        public string image;
        public float price;
        public string owner;
        public int status;
        public bool bidded;
        public bool verified;
    }

    public class StratisphereAPI : MonoBehaviour
    {
        public delegate void VoidPainting(Painting[] p);
        public static VoidPainting DownloadReturned;

        public string url_startispheremirror= "https://stratisphere.com/api/";

        public void LoadStratisphere(string search = null,int page=300)
        {
            string url = url_startispheremirror+"asset?page=1&pageSize="+page+"&searchText=" + search + "&startPrice=0&endPrice=0&status=0&orderBy=Id&orderDirection=Desc";
            StartCoroutine(ActuallyLoad(url));
        }

        IEnumerator ActuallyLoad(string url)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(" Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError("HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        ProcessDownloaded(webRequest.downloadHandler.text);
                        break;
                }
            }

        }

        void ProcessDownloaded(string s)
        {
            if (string.IsNullOrEmpty(s)) return;

            StratisphereJSON json = JsonConvert.DeserializeObject<StratisphereJSON>(s);

            // convert to Paintings array
            item[] items = json.items;
            Painting[] p = new Painting[items.Length];
            for (int i = 0; i < p.Length; i++)
            {
                item it = items[i];
                p[i] = new Painting(it.tokenId, it.contract, it.image, it.name, it.price, it.owner);
            }

            DownloadReturned?.Invoke(p);
        }

    }
}