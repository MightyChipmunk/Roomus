using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Show_RoomItem : MonoBehaviour
{
    int id = 0;
    public int ID { get { return id; } set { id = value; } }

    byte[] imgBytes;
    public byte[] ImageBytes { get { return imgBytes; } set { imgBytes = value; } }

    public Text roomName;
    public Text roomDescription;

    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {
        Show_LoadRoomList.Instance.RoomName = gameObject.name;
        Show_LoadRoomList.Instance.ID = ID;
        SceneManager.LoadScene("Test_Connect");
    }
}
