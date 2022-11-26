using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LikeItem : MonoBehaviour
{
    public string roomName;
    public int likes;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("LikesText").GetComponent<Text>().text = likes.ToString();
        transform.Find("RoomText").GetComponent<Text>().text = roomName;

        transform.Find("Likes").GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, likes * 10 + 30);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
