using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_ShoppingUIManager : MonoBehaviour
{

    public static JM_ShoppingUIManager instance;
    public GameObject furnInfo;
    public Transform showPos;
    public Transform hidePos;
    bool isInfoMove;
    bool isInfoShow;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        furnInfo.transform.position = hidePos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInfoMove)
        {
            if (isInfoShow)
            {
                furnInfo.transform.position = Vector3.Lerp(furnInfo.transform.position, showPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(furnInfo.transform.position, showPos.position) < 0.5f)
                {
                    furnInfo.transform.position = showPos.position;
                    isInfoMove = false;
                }
            }
            if (!isInfoShow)
            {
                furnInfo.transform.position = Vector3.Lerp(furnInfo.transform.position, hidePos.position, Time.deltaTime * 4);
                if (Vector3.Distance(furnInfo.transform.position, hidePos.position) < 0.5f)
                {
                    furnInfo.transform.position = hidePos.position;
                    isInfoMove = false;
                }
            }
        }
        
    }

    public void ShowInfo()
    {
        isInfoMove = true;
        isInfoShow = true;
    }

    public void HideInfo()
    {
        isInfoMove = true;
        isInfoShow = false;
    }

    public void OnClickBack()
    {
        HideInfo();
    }
}
