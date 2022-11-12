using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreenManager : MonoBehaviour
{
    public GameObject screenManager;
    JM_ScreenManager screenCode;

    // Start is called before the first frame update
    void Start()
    {
        screenCode = screenManager.GetComponent<JM_ScreenManager>();                                                               
    }

    // Update is called once per frame
    void Update()
    { 
        
    }  

    public void OnClickMyPage()
    {
        SceneManager.LoadScene("MyPage");
    }

    public void OnClickMessage()
    {
        SceneManager.LoadScene("Message");
    }

    public void OnClickRoomCustom()
    {
        SceneManager.LoadScene("RoomCustom_New");

    }

    public void OnClickUpload()
    {
        SceneManager.LoadScene("FBXLoad");
    }

    public void OnClickTravel()
    {
        SceneManager.LoadScene("Test_Connect");
    }

    public void OnClickShop()
    {

    }

    public void OnClickRoom()
    {
        SceneManager.LoadScene("ShowRoom_New");
    }
}
