using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_UIManager : MonoBehaviour
{
    // bool that checks if perspective ui is activated or not
    bool isPerspUI;

    // perspective ui
    public GameObject perspUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isPerspUI) PerspUIInvisible();

            else PerspUIVisible();       
        }
    }

    // perspective ui visible function
    public void PerspUIVisible()
    {
        perspUI.SetActive(true);
        isPerspUI = true;
    }

    // perspective ui invisible function
    public void PerspUIInvisible()
    {
        perspUI.SetActive(false);
        isPerspUI = false;
    }


}
