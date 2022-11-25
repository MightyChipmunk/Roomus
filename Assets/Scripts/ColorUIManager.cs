using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUIManager : MonoBehaviour
{
    public GameObject library;
    public GameObject advColor;
    public Transform originPos;
    public Transform showPos;

    bool isAdvUIMove;
    bool isAdvUIShow;

    public bool isLibraryMove;
    bool isLibShow;

    // Start is called before the first frame update
    void Start()
    {
        advColor.transform.position = originPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAdvUIMove)
        {
            if (isAdvUIShow)
            {
                advColor.transform.position = Vector3.Lerp(advColor.transform.position, showPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(advColor.transform.position, showPos.position) < 0.5f)
                {
                    advColor.transform.position = showPos.position;
                    isLibraryMove = false;
                }
            }
            if (!isAdvUIShow)
            {
                advColor.transform.position = Vector3.Lerp(advColor.transform.position, originPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(advColor.transform.position, originPos.position) < 0.5f)
                {
                    advColor.transform.position = originPos.position;
                    isLibraryMove = false;
                }
            }
        }
    }
    /*
    public void OnClickLibrary()
    {
        isLibraryMove = true;

        if (advColor.transform.position == showPos.position)
        {
            isLibShow = false;

        }
        if (advColor.transform.position == originPos.position)
        {
            isLibShow = true;
        }
    }
    */

    public void OnClickAdvUI()
    {
        isAdvUIMove = true;
        isAdvUIShow = true;
    }

    public void OnClickDone()
    {
        isAdvUIMove = true;
        isAdvUIShow = false;
        JM_LibraryManager.instance.isLibraryMove = true;
        JM_LibraryManager.instance.isLibShow = true;
    }

    public void OnScreenShot()
    {
        isLibraryMove = true;
        isLibShow = false;
    }
}
