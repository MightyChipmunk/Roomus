using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JH_RoomDecoManager : MonoBehaviour
{
    public static JH_RoomDecoManager Instance;

    int selectedRoom = 0;
    public int SelectedRoom { get { return selectedRoom; } }

    public GameObject NewOrLoad;
    public GameObject LoadSelect;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        NewOrLoad.SetActive(true);
        LoadSelect.SetActive(false);

        if (Deco_LoadRoomList.Instance != null)
            Destroy(Deco_LoadRoomList.Instance.gameObject);
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
        NewOrLoad.SetActive(false);
        LoadSelect.SetActive(true);
    }

    public void OnClickRoom(int n)
    {
        //selectedRoom = n;
        SceneManager.LoadScene("LocalRoom");
    }
}
