using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LikeBtn : MonoBehaviour
{
    int id;
    public int ID { get { return id; } set { id = value; } }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickLike);
    }

    private void Update()
    {
        Debug.Log(id);
    }

    public void OnClickLike()
    {
        Debug.Log(id);
        StartCoroutine(Like(UrlInfo.url + "/products/" + id.ToString() + "/likes", id));
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
                //iTween.ScaleTo(likeButton.gameObject, iTween.Hash("x", 1.2f, "y", 1.2f, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
                JH_PopUpUI.Instance.SetUI("", "Like Complete!", true, 0.3f);
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
                //iTween.ScaleTo(likeButton.gameObject, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
                JH_PopUpUI.Instance.SetUI("", "UnLike Complete!", true, 0.3f);
                Debug.Log("Furnit UnLike complete!");
            }
        }
    }
}
