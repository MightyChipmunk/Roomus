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
        SceneManager.LoadScene("Shopping");
    }

    public void OnClickLikes()
    {
        TokenManager.Instance.roomTypeP = RoomType.Liked;
        SceneManager.LoadScene("ShowRoom_New");
    }

    public void OnClickRoom()
    {
        TokenManager.Instance.roomTypeP = RoomType.All;
        SceneManager.LoadScene("ShowRoom_New");
    }
}
