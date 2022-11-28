using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreenManager : MonoBehaviour
{
    public GameObject screenManager;
    JM_ScreenManager screenCode;

    bool isMyPage;
    bool isMessage;
    bool isRoomCustom;
    bool isUpload;
    bool isShowRoom;

    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory(Application.dataPath + "/LocalServer");
        Directory.Delete(UnityEngine.Application.dataPath + "/LocalServer", true);
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] files = dir.GetFiles("*.zip");

        foreach (FileInfo file in files)
        {
            file.Delete();
        }
        screenCode = screenManager.GetComponent<JM_ScreenManager>();

        if (JH_RoomDecoManager.Instance != null)
            Destroy(JH_RoomDecoManager.Instance.gameObject);
        else if (Deco_LoadRoomList.Instance != null)
            Destroy(Deco_LoadRoomList.Instance.gameObject);
        else if (Deco_GetXYZ.Instance != null)
            Destroy(Deco_GetXYZ.Instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("PutDeleteTest");
        }

        if (screenCode.isSceneChange)
        {
            if (isMyPage) SceneManager.LoadScene("MyPage");
            if (isMessage) SceneManager.LoadScene("Message");
            if (isRoomCustom) SceneManager.LoadScene("RoomCustom_New");
            if (isUpload) SceneManager.LoadScene("FBXLoad");
            if (isShowRoom) SceneManager.LoadScene("ShowRoom_New");
        }
    }  

    public void OnClickMyPage()
    {
        screenCode.Darken();
        isMyPage = true;
        //SceneManager.LoadScene("MyPage");
    }

    public void OnClickMessage()
    {
        screenCode.Darken();
        isMessage = true;
        // SceneManager.LoadScene("Message");
    }

    public void OnClickRoomCustom()
    {
        screenCode.Darken();
        isRoomCustom = true;
        // SceneManager.LoadScene("RoomCustom_New");
    }

    public void OnClickUpload()
    {
        screenCode.Darken();
        isUpload = true;
        // SceneManager.LoadScene("FBXLoad");
    }

    public void OnClickTravel()
    {
        SceneManager.LoadScene("Shopping");
    }

    public void OnClickLikes()
    {
        TokenManager.Instance.roomTypeP = RoomType.Liked;
        screenCode.Darken();
        isShowRoom = true;
        // SceneManager.LoadScene("ShowRoom_New");
    }

    public void OnClickRoom()
    {
        TokenManager.Instance.roomTypeP = RoomType.All;
        screenCode.Darken();
        isShowRoom = true;
        // SceneManager.LoadScene("ShowRoom_New");
    }
}
