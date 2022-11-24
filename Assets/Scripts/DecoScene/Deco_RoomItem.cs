using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Deco_RoomItem : MonoBehaviour
{
    public byte[] imgData;
    public bool access;
    public string roomName;

    Image thumb;
    Text accessContent;
    Text roomNameContent;
    Button deleteButton;

    int id = 0;
    public int ID { get { return id; } set { id = value; } }

    // Start is called before the first frame update
    void Start()
    {
        thumb = transform.Find("Thumb").GetComponent<Image>();

        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(imgData);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        thumb.sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
        accessContent = transform.Find("Bool").GetComponent<Text>();
        if (access)
            accessContent.text = "Y";
        else
            accessContent.text = "N";
        roomNameContent = transform.Find("RoomName").GetComponent<Text>();
        roomNameContent.text = roomName;

        deleteButton = transform.Find("Delete").GetComponent<Button>();
        deleteButton.onClick.AddListener(OnClickDelete);
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {
        Deco_LoadRoomList.Instance.RoomName = gameObject.name;
        Deco_LoadRoomList.Instance.ID = ID;
    }

    public void OnClickDelete()
    {
        StartCoroutine(Delete(UrlInfo.url + "/rooms/" + ID.ToString()));
    }

    IEnumerator Delete(string url)
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
                JH_PopUpUI.Instance.SetUI("", "Room Delete Complete!", false);
                Debug.Log("Delete complete!");
                Destroy(gameObject);
            }
        }
    }
}
