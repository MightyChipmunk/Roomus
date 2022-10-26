using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show_HideUI : MonoBehaviour
{
    public bool canHide;
    public GameObject RoomInfo;
    // Start is called before the first frame update
    void Start()
    {
        canHide = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canHide)
            RoomInfo.SetActive(false);
        else
            RoomInfo.SetActive(true);
    }
}
