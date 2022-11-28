using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JM_ToolBar : MonoBehaviour
{
    public GameObject toolBar;

    bool isHome;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            ActivateToolBar();
        }

        else if (Input.GetKeyUp(KeyCode.Slash))
        {
            DeactivateToolBar();
        }
    }

    void ActivateToolBar()
    {
        toolBar.transform.position = Input.mousePosition;
        toolBar.SetActive(true);
    }

    void DeactivateToolBar()
    {
        toolBar.SetActive(false);
    }

    public void OnClickHome()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnClickBack()
    {

    }

    public void OnClickMyPage()
    {

    }
     
    public void OnClickTravel()
    {

    }

    public void OnClickSettings()
    {

    }

}
