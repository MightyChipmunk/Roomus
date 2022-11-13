using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JH_MoreInfoManager : MonoBehaviour
{
    public GameObject item;
    public GameObject itemMoreInfo;

    public Transform trContent;

    // Start is called before the first frame update
    void Start()
    {
        itemMoreInfo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OnGetJson(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                furnitInfos[] data = JsonHelper.FromJson<furnitInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    StartCoroutine(OnGetUrl(data[i]));
                }
                Debug.Log("UrlList Download complete!");
            }
        }
    }

    // ?????? ????????? id?? ?????? ???
    IEnumerator OnGetUrl(furnitInfos info)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(info.screenShotUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // ?????? ????????? id?? ????귯???? ???? ???
                // Deco_FurnitItem
                AddContent(info.category, www.downloadHandler.data);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }

    void AddContent(string name, byte[] imgData)
    {
        GameObject obj = Instantiate(item, trContent);
        obj.transform.Find("Text").GetComponent<Text>().text = name;
        // 받아온 이미지의 바이너리 데이터로 자신의 이미지 변경
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imgData);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        obj.transform.Find("Profile").GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
    }

    void OnClicked()
    {
        itemMoreInfo.SetActive(true);
    }
}
