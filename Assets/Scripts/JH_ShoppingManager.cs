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
                // �������� �����͸� �޾ƿ�
                furnitInfos[] data = JsonHelper.FromJson<furnitInfos>(www.downloadHandler.text);
                for (int i = 0; i < data.Length; i++)
                {
                    // �������� �����Ϳ� �ִ� ��ũ���� url�� �ٽ� Get ��û�� ����
                    StartCoroutine(OnGetUrl(data[i]));
                }
                Debug.Log("Furnit UrlList Download complete!");
            }
        }
    }

    // ���� ������ ��ũ���� url�� Get ��û�� ����
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
                // �޾ƿ� ��ũ������ �̸� ���� ������ �̿��� ���̺귯���� �߰�
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
