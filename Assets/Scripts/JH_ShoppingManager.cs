using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JH_ShoppingManager : MonoBehaviour
{
    public GameObject shopItem;
    public Transform content;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OnGetJson(UrlInfo.url + "/products"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OnGetJson(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // 가구들의 데이터를 받아옴
                furnitInfos[] data = JsonHelper.FromJson<furnitInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // 가구들의 데이터에 있는 스크린샷 url로 다시 Get 요청을 보냄
                    StartCoroutine(OnGetUrl(data[i]));
                }
                Debug.Log("Furnit UrlList Download complete!");
            }
        }
    }

    // 가구 정보의 스크린샷 url로 Get 요청을 보냄
    IEnumerator OnGetUrl(furnitInfos info)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(info.screenShotUrl))
        {
            //www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // 받아온 스크린샷과 미리 받은 정보를 이용해 라이브러리에 추가
                // Deco_FurnitItem
                AddContent(info.furnitName, info.category, info.no, www.downloadHandler.data);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }

    void AddContent(string furnitName, string category, int id = 0, byte[] imgBytes = null)
    {
        GameObject item = Instantiate(shopItem, content);
        item.name = id.ToString();
        //item.GetComponent<Deco_FurnitItem>().fbxJson = fbxJson;
        item.GetComponent<ShopItem>().ID = id;
        item.GetComponent<ShopItem>().ItemName = furnitName;
        if (category != null)
            item.GetComponent<ShopItem>().Category = category;
        item.GetComponent<ShopItem>().ImageBytes = imgBytes;
        item.GetComponentInChildren<Text>().text = furnitName;
    }
}
