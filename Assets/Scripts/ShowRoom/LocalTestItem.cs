using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalTestItem : MonoBehaviour
{

    [SerializeField]
    bool localTest = false;
    [SerializeField]
    string category = "Bedroom";

    public string Category { get { return category; } }

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
        //Show_LoadRoomList.Instance.localTest = localTest;
        SceneManager.LoadScene("Test_Connect");
    }
}
