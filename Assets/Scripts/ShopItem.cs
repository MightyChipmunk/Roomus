using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    int id = 0;
    public int ID { get { return id; } set { id = value; } }

    string itemName = "";
    public string ItemName { get { return itemName; } set { itemName = value; } }

    string category = "";
    public string Category { get { return category; } set { category = value; } }

    byte[] imgBytes;
    public byte[] ImageBytes { get { return imgBytes; } set { imgBytes = value; } }

    public GameObject itemMoreInfo;

    // Start is called before the first frame update
    void Start()
    {
        itemMoreInfo = GameObject.Find("Canvas").transform.Find("MyRoomAnalysisBG").transform.Find("ItemMoreInfo").gameObject;

        transform.Find("Text").GetComponent<Text>().text = itemName;
        // �޾ƿ� �̹����� ���̳ʸ� �����ͷ� �ڽ��� �̹��� ����
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imgBytes);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        transform.GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {

        StartCoroutine(OnClickGet(UrlInfo.url + "/products" + "/" + id.ToString()));
    }

    FBXJson fbxJson = new FBXJson();
    IEnumerator OnClickGet(string url)
    {
        // ui
        JM_ShoppingUIManager.instance.ShowInfo();

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                FBXWrapper wrapper = JsonUtility.FromJson<FBXWrapper>(www.downloadHandler.text);
                fbxJson = wrapper.data;

                //FBXJson[] data = JsonHelper.FromJson<FBXJson>(www.downloadHandler.text);
                Debug.Log("FBXJson Download complete!");

                itemMoreInfo.SetActive(true);

                itemMoreInfo.transform.Find("Image").GetComponent<Image>().sprite = transform.GetComponent<Image>().sprite;
                itemMoreInfo.transform.Find("Name").GetComponent<Text>().text = fbxJson.furnitName;
                itemMoreInfo.transform.Find("DimensionContent").GetComponent<Text>().text =
                    (fbxJson.xsize * 100).ToString() + " x " + (fbxJson.zsize * 100).ToString() + " x " + (fbxJson.ysize * 100).ToString();
                itemMoreInfo.transform.Find("Price").GetComponent<Text>().text = fbxJson.price.ToString();

                string[] lines = fbxJson.information.Split("\n");

                string information = "";
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    information += lines[i] + "\n";
                }
                string shotUrl = lines[lines.Length - 1];

                itemMoreInfo.transform.Find("DescriptionContent").GetComponent<Text>().text = information;
                itemMoreInfo.transform.Find("BuyBtn").GetComponent<BuyBtn>().url = shotUrl;
            }
        }
    }
}
