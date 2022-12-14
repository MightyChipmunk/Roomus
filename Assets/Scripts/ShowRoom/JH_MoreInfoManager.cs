using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JH_MoreInfoManager : MonoBehaviour
{
    public GameObject item;
    public Transform trContent;
    public ArrayJson arrayJson;

    public static JH_MoreInfoManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);

        arrayJson = new ArrayJson();
        arrayJson.roomNo = -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OnGetJson(UrlInfo.url + "/products"));
    }

    List<int> IDs = new List<int>();
    void GetIDs()
    {
        foreach (SaveJsonInfo info in arrayJson.datas)
        {
            IDs.Add(info.idx);
        }
    }

    IEnumerator OnGetJson(string uri)
    {
        while(arrayJson.roomNo < 0)
        {
            yield return null;
        }

        GetIDs();

        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            //전체 불러오기로 바꿔야함
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

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
                    if (IDs.Contains(data[i].no))
                        StartCoroutine(OnGetUrl(data[i]));
                }
                Debug.Log("UrlList Download complete!");
            }
        }
    }

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
                // Deco_FurnitItem
                AddContent(info.no, info.furnitName, www.downloadHandler.data);
                //AddContent(info.no, info.furnitName, www.downloadHandler.data);
                Debug.Log("ScreenShot Download complete!");
            }
        }
    }

    void AddContent(int id, string name, byte[] imgData)
    {
        GameObject obj = Instantiate(item, trContent);
        obj.GetComponent<Show_MoreInfoItem>().ItemName = name;
        obj.GetComponent<Show_MoreInfoItem>().ID = id;
        obj.GetComponent<Show_MoreInfoItem>().ImageBytes = imgData;
    }
}
