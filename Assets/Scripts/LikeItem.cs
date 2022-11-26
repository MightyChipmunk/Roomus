using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LikeItem : MonoBehaviour
{
    public string roomName;
    public float likes;
    float val;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("LikesText").GetComponent<Text>().text = likes.ToString();
        transform.Find("RoomText").GetComponent<Text>().text = roomName;
        transform.Find("likes").GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, 0);
        //transform.Find("Likes").GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, likes * 10 + 30);
    }

    // Update is called once per frame
    void Update()
    {
        val = Mathf.Lerp(val, likes, Time.deltaTime * 4);
        if (likes - val < 0.1f)
            val = likes;
        transform.Find("Likes").GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, val * 30);
    }

    //().sizeDelta = new Vector2(first.GetComponent<RectTransform>().sizeDelta.x, valList[i]);
}
