using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JM_TutorialManager : MonoBehaviour
{
    
    public Transform tut1HidePos;
    public Transform tut1ShowPos;
    public Transform tut2HidePos;
    public Transform tut2ShowPos;
    public Transform tut3HidePos;
    public Transform tut3ShowPos;

    public GameObject tutorialMain;
   
    bool isTutMove;
    bool isTutShow;

    bool isTut2Move;
    bool isTut2Show;
    [SerializeField]
    bool isTut2Reset;

    bool isTut3Move;
    bool isTut3Show;
    [SerializeField]
    bool isTut3Reset;

    [SerializeField]
    GameObject tut2TargetGo;
    [SerializeField]
    GameObject tut2ShowGo;

    [SerializeField]
    GameObject tut3TargetGo;
    [SerializeField]
    GameObject tut3ShowGo;

    public List<GameObject> tut2List = new List<GameObject>();
    public List<GameObject> tut3List = new List<GameObject>();
    public List<Button> tut1BtnList = new List<Button>();
    public List<Button> libBtnList = new List<Button>();



    // Start is called before the first frame update
    void Start()
    {
        // tut1
        tutorialMain.transform.position = tut1HidePos.position;

        // tut2
        for (int i = 0; i < tut2List.Count; i++)
        {
            tut2List[i].transform.position = tut2HidePos.position;
        }

        // tut3
        for (int i = 0; i < tut3List.Count; i++)
        {
            tut3List[i].transform.position = tut3HidePos.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTutMove)
        {
            if (isTutShow)
            {
                tutorialMain.transform.position = Vector3.Lerp(tutorialMain.transform.position, tut1ShowPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(tutorialMain.transform.position, tut1ShowPos.position) < 0.5f)
                {
                    tutorialMain.transform.position = tut1ShowPos.position;
                    isTutMove = false;
                }
            }
            if (!isTutShow)
            {
                tutorialMain.transform.position = Vector3.Lerp(tutorialMain.transform.position, tut1HidePos.position, Time.deltaTime * 4);
                if (Vector3.Distance(tutorialMain.transform.position, tut1HidePos.position) < 0.5f)
                {
                    tutorialMain.transform.position = tut1HidePos.position;
                    isTutMove = false;
                }
            }
        }

        if (isTut2Move)
        {
            ShowTut2(tut2ShowGo);        
            if (tut2TargetGo != null && tut2TargetGo != tut2ShowGo)
            {
                HideTut2(tut2TargetGo);
            }
        }

        if (isTut3Move)
        {
            ShowTut3(tut3ShowGo);
            if (tut3TargetGo != null && tut3TargetGo != tut3ShowGo)
            {
                HideTut3(tut3TargetGo);
            }
        }

        if (isTut2Reset)
        {
            HideTut2(tut2TargetGo);
        }

        if (isTut3Reset)
        {
            HideTut3(tut3TargetGo);
        }
    }

    public void ShowTut2(GameObject go)
    {
        
        go.transform.position = Vector3.Lerp(go.transform.position, tut2ShowPos.position, Time.deltaTime * 4);
        if (Vector3.Distance(go.transform.position, tut2ShowPos.position) < 0.5f)
        {
            tut2TargetGo = go;
            go.transform.position = tut2ShowPos.position;
            isTut2Move = false;
            if (isTut2Reset)
            {
                tut2ShowGo = null;
                tut2TargetGo = null;
                isTut2Reset = false;
            }
        }
    }

    public void HideTut2(GameObject go)
    {
        go.transform.position = Vector3.Lerp(go.transform.position, tut2HidePos.position, Time.deltaTime * 4);
        if (Vector3.Distance(go.transform.position, tut2HidePos.position) < 0.5f)
        {
            go.transform.position = tut2HidePos.position;
            isTut2Move = false;
            if (isTut2Reset)
            {
                tut2ShowGo = null;
                tut2TargetGo = null;
                isTut2Reset = false;
            }
        }
    }

    public void ShowTut3(GameObject go)
    {
        go.transform.position = Vector3.Lerp(go.transform.position, tut3ShowPos.position, Time.deltaTime * 4);
        if (Vector3.Distance(go.transform.position, tut3ShowPos.position) < 0.5f)
        {
            tut3TargetGo = go;
            go.transform.position = tut3ShowPos.position;
            isTut3Move = false;
        }
    }

    public void HideTut3(GameObject go)
    {
        go.transform.position = Vector3.Lerp(go.transform.position, tut3HidePos.position, Time.deltaTime * 4);
        if (Vector3.Distance(go.transform.position, tut3HidePos.position) < 0.5f)
        {
            go.transform.position = tut3HidePos.position;
            isTut3Move = false;
            if (isTut3Reset)
            {
                tut3ShowGo = null;
                tut3TargetGo = null;
                isTut3Reset = false;
            }
                
        }
    }
    

    public void OnClickHelp()
    {
        if (tutorialMain.transform.position == tut1HidePos.position)
        {
            isTutShow = true;
        }
        else if (tutorialMain.transform.position == tut1ShowPos.position)
        {
            isTutShow = false;

            // check if tut2 active
            for (int i = 0; i < tut2List.Count; i++)
            {
                if (tut2List[i].transform.position == tut2ShowPos.position)
                {
                    isTut2Reset = true;
                }
            }

            // check if tut3 active
            for (int i = 0; i < tut3List.Count; i++)
            {
                if (tut3List[i].transform.position == tut3ShowPos.position)
                {
                    isTut3Reset = true;
                }
            }
        }
        isTutMove = true;
    }

    public void CheckIfTut3Active()
    {
        for (int i = 0; i < tut3List.Count; i++)
        {
            if (tut3List[i].transform.position == tut3ShowPos.position)
            {
                isTut3Reset = true;
            }
            else return;
        }
    }

    public void CheckIfTut2Active()
    {
        for (int i = 0; i < tut2List.Count; i++)
        {
            if (tut2List[i].transform.position == tut2ShowPos.position)
            {
                isTut2Reset = true;
            }
            else return;
        }
    }

    public void OnClickBtn1()
    {
        for (int i = 0; i < tut1BtnList.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == tut1BtnList[i].gameObject.name)
            {

                tut2ShowGo = tut2List[i];
                if (tut1BtnList[i].gameObject.name != "LibraryBtn")
                {
                    for (int j = 0; j < tut3List.Count; j++)
                    {
                        if (tut3List[j].transform.position == tut3ShowPos.position)
                        {
                            isTut3Reset = true;
                        }
                    }  
                }
            }

        }
        isTut2Move = true;
        isTut2Show = true;
    }

    public void OnClickBtn2()
    {
        for (int i = 0; i < libBtnList.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == libBtnList[i].gameObject.name)
            {
                tut3ShowGo = tut3List[i];
            }
        }
        isTut3Move = true;
        isTut2Show = true;
    }
}
