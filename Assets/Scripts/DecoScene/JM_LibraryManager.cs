using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_LibraryManager : MonoBehaviour
{
    public GameObject libraryUI;
    public GameObject aiUI;
    public Transform originPos;
    public Transform showPos;

    public bool isLibraryMove;
    [SerializeField]
    bool isLibShow;

    public bool isAIMove;
    bool isAIShow;

    // Start is called before the first frame update
    void Start()
    {
        libraryUI.transform.position = originPos.position;
        aiUI.transform.position = originPos.position;

        libraryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLibraryMove)
        {
            if (isLibShow)
            {
                libraryUI.SetActive(true);
                libraryUI.transform.position = Vector3.Lerp(libraryUI.transform.position, showPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(libraryUI.transform.position, showPos.position) < 0.5f)
                {
                    libraryUI.transform.position = showPos.position;
                    isLibraryMove = false;
                }
            }
            if (!isLibShow)
            {
                libraryUI.transform.position = Vector3.Lerp(libraryUI.transform.position, originPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(libraryUI.transform.position, originPos.position) < 0.5f)
                {
                    libraryUI.transform.position = originPos.position;
                    isLibraryMove = false;
                    libraryUI.SetActive(false);
                }
            }
        }
        if (isAIMove)
        {
            if (isAIShow)
            {
                aiUI.transform.position = Vector3.Lerp(aiUI.transform.position, showPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(aiUI.transform.position, showPos.position) < 0.5f)
                {
                    aiUI.transform.position = showPos.position;
                    isAIMove = false;
                }
            }
            if (!isAIShow)
            {
                aiUI.transform.position = Vector3.Lerp(aiUI.transform.position, originPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(aiUI.transform.position, originPos.position) < 0.5f)
                {
                    aiUI.transform.position = originPos.position;
                    isAIMove = false;
                }
            }
        }
    }

    public void OnClickLibrary()
    {
        isLibraryMove = true;

        if (libraryUI.transform.position == showPos.position)
        {
            isLibShow = false;
            
        }
        if (libraryUI.transform.position == originPos.position)
        {
            isLibShow = true;
            
        }
    }

    public void OnClickAI()
    {
        if (aiUI.transform.position == showPos.position) isAIShow = false;
        if (aiUI.transform.position == originPos.position) isAIShow = true;
        isAIMove = true;
    }

    
}
