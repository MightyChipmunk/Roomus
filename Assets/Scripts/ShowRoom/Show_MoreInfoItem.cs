using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Show_MoreInfoItem : MonoBehaviour
{
    int id = 0;
    public int ID { get { return id; } set { id = value; } }

    string itemName = "";
    public string ItemName { get { return itemName; } set { itemName = value; } }

    byte[] imgBytes;
    public byte[] ImageBytes { get { return imgBytes; } set { imgBytes = value; } }

    [SerializeField]
    GameObject itemMoreInfo1;
    [SerializeField]
    GameObject itemMoreInfo2;

    // Start is called before the first frame update
    void Start()
    {
        itemMoreInfo1 = GameObject.Find("Canvas").transform.Find("MoreInfo").transform.Find("Comment+Library").transform.Find("ItemMoreInfo1").gameObject;
        itemMoreInfo2 = GameObject.Find("Canvas").transform.Find("MoreInfo").transform.Find("Comment+Library").transform.Find("ItemMoreInfo2").gameObject;

        transform.Find("Text").GetComponent<Text>().text = itemName;
        // �޾ƿ� �̹����� ���̳ʸ� �����ͷ� �ڽ��� �̹��� ����
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imgBytes);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        transform.Find("Mask").transform.Find("Profile").GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        
        // show info1 if no info is shown yet
        if (!JM_FurnInfoManager.instance.info1Show && !JM_FurnInfoManager.instance.info2Show)
        {
            print("start furniture");
            StartCoroutine(OnClickGet1(UrlInfo.url + "/products" + "/" + id.ToString()));
            JM_FurnInfoManager.instance.ShowInit();
        }
        
        // show info2 if info1 is active
        else if (JM_FurnInfoManager.instance.info1Show && !JM_FurnInfoManager.instance.info2Show)
        {
            print("2");
            StartCoroutine(OnClickGet2(UrlInfo.url + "/products" + "/" + id.ToString()));
            JM_FurnInfoManager.instance.ShowInfo2();
        }

        // show info1 if info2 is active
        else if (JM_FurnInfoManager.instance.info2Show && !JM_FurnInfoManager.instance.info1Show)
        {
            print("12");
            StartCoroutine(OnClickGet1(UrlInfo.url + "/products" + "/" + id.ToString()));
            JM_FurnInfoManager.instance.ShowInfo1();
        }
        
    }

    FBXJson fbxJson = new FBXJson();
    IEnumerator OnClickGet1(string url)
    {
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

                itemMoreInfo1.SetActive(true);

                itemMoreInfo1.transform.Find("Image").GetComponent<Image>().sprite = transform.Find("Mask").transform.Find("Profile").GetComponent<Image>().sprite;
                itemMoreInfo1.transform.Find("Name").GetComponent<Text>().text = fbxJson.furnitName;
                itemMoreInfo1.transform.Find("DimensionContent").GetComponent<Text>().text =
                    (fbxJson.xsize * 100).ToString() + " x " + (fbxJson.zsize * 100).ToString() + " x " + (fbxJson.ysize * 100).ToString();
                itemMoreInfo1.transform.Find("Price").GetComponent<Text>().text = fbxJson.price.ToString();

                string[] lines = fbxJson.information.Split("\n");

                string information = "";
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    information += lines[i] + "\n";
                }
                string shotUrl = lines[lines.Length - 1];

                itemMoreInfo1.transform.Find("DescriptionContent").GetComponent<Text>().text = information;
                itemMoreInfo1.transform.Find("BuyBtn").GetComponent<BuyBtn>().url = shotUrl;
                itemMoreInfo1.transform.Find("LikeBtn").GetComponent<LikeBtn>().ID = fbxJson.no;
                Debug.Log(itemMoreInfo2.transform.Find("LikeBtn").GetComponent<LikeBtn>().ID);
            }
        }
    }

    IEnumerator OnClickGet2(string url)
    {
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

                itemMoreInfo2.SetActive(true);

                itemMoreInfo2.transform.Find("Image").GetComponent<Image>().sprite = transform.Find("Mask").transform.Find("Profile").GetComponent<Image>().sprite;
                itemMoreInfo2.transform.Find("Name").GetComponent<Text>().text = fbxJson.furnitName;
                itemMoreInfo2.transform.Find("DimensionContent").GetComponent<Text>().text =
                    (fbxJson.xsize * 100).ToString() + " x " + (fbxJson.zsize * 100).ToString() + " x " + (fbxJson.ysize * 100).ToString();
                itemMoreInfo2.transform.Find("Price").GetComponent<Text>().text = fbxJson.price.ToString();

                string[] lines = fbxJson.information.Split("\n");

                string information = "";
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    information += lines[i] + "\n";
                }
                string shotUrl = lines[lines.Length - 1];

                itemMoreInfo2.transform.Find("DescriptionContent").GetComponent<Text>().text = information;
                itemMoreInfo2.transform.Find("BuyBtn").GetComponent<BuyBtn>().url = shotUrl;
                itemMoreInfo2.transform.Find("LikeBtn").GetComponent<LikeBtn>().ID = fbxJson.no;
                Debug.Log(itemMoreInfo2.transform.Find("LikeBtn").GetComponent<LikeBtn>().ID);
            }
        }
    }
}
