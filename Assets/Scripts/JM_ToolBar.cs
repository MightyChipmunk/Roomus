using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JM_ToolBar : MonoBehaviour
{
    public GameObject toolBar;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ActivateToolBar();
        }

        else if (Input.GetKeyUp(KeyCode.Alpha0))
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
