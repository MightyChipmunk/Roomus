using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Show_FurnitInfo : MonoBehaviour
{
    //public string furnitName;
    //public int price;
    //public string category;

    public Deco_Idx furnitInfo;

    Button likeButton;
    Image likeImage;

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("FurnitName").GetComponent<Text>().text = furnitInfo.Name;
        transform.Find("Price").GetComponent<Text>().text = furnitInfo.Price.ToString();
        transform.Find("Category").GetComponent<Text>().text = furnitInfo.Category;

        likeButton = transform.Find("Like").GetComponent<Button>();
        likeImage = transform.Find("Like").GetComponent<Image>();
        likeButton.onClick.AddListener(OnClickLike);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickLike()
    {
        StartCoroutine(Like(UrlInfo.url + "/products/" + furnitInfo.Id.ToString() + "/likes", furnitInfo.Id));
    }

    IEnumerator Like(string url, int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("productNo", id.ToString());
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                StartCoroutine(UnLike(url, id));
                Debug.Log(www.error);
            }
            else
            {
                iTween.ScaleTo(likeButton.gameObject, iTween.Hash("x", 1.2f, "y", 1.2f, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

                Debug.Log("Furnit Like complete!");
            }
        }
    }

    IEnumerator UnLike(string url, int id)
    {
        using (UnityWebRequest www = UnityWebRequest.Delete(url))
        {
            www.SetRequestHeader("Authorization", TokenManager.Instance.Token);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                iTween.ScaleTo(likeButton.gameObject, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

                Debug.Log("Furnit UnLike complete!");
            }
        }
    }
}
