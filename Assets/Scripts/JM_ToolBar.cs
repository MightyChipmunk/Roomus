using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JM_ToolBar : MonoBehaviour
{
    public GameObject toolBar;
    public GameObject screenManager;
    JM_ScreenManager screenCode;
    bool isHome;

    // Start is called before the first frame update
    void Start()
    {
        screenCode = screenManager.GetComponent<JM_ScreenManager>();
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

        if (screenCode.isSceneChange)
        {
            if (isHome)
            {
                GoHome();
            }

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
        //screenCode.Darken();
        //isHome = true;
        SceneManager.LoadScene("Main");
    }

    void GoHome()
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
