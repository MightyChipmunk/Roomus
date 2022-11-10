using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JM_RoomDecoManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // new room deco function
    public void NewRoom()
    {
        SceneManager.LoadScene("NewRoomInit");
    }

    // load room deco function
    public void LoadRoom()
    {
        SceneManager.LoadScene("LoadRoomInit");
    }

}