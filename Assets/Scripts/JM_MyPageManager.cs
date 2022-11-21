using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_MyPageManager : MonoBehaviour
{
    [SerializeField]
    bool isPWMove;
    [SerializeField]
    bool isPWEdit;
    bool isPWGood;
    public GameObject pwUI;
    public Transform pwUIPos;
    [SerializeField]
    public Transform pwUIOriginPos;

    public InputField newPWInput;
    public InputField reEnterPWInput;
    string newPW;

    // Start is called before the first frame update
    void Start()
    {
        pwUI.transform.position = pwUIOriginPos.position;
        newPWInput.onEndEdit.AddListener(UpdatePW);
        reEnterPWInput.onEndEdit.AddListener(CheckPW);
    }

    // Update is called once per frame
    void Update()
    { 
        if (isPWMove)
        {
            if (isPWEdit)
            {
                pwUI.transform.position = Vector3.Lerp(pwUI.transform.position, pwUIPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(pwUI.transform.position, pwUIPos.position) < 0.5f)
                {
                    pwUI.transform.position = pwUIPos.position;
                    isPWMove = false;
                }
            }
            if (!isPWEdit)
            {
                pwUI.transform.position = Vector3.Lerp(pwUI.transform.position, pwUIOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(pwUI.transform.position, pwUIOriginPos.position) < 0.5f)
                {
                    pwUI.transform.position = pwUIOriginPos.position;
                    isPWMove = false;
                }
            }
        }
    }

    public void OnClickEditPW()
    {
        isPWMove = true;
        isPWEdit = true;     
    }
    public void OnClickBackPW()
    {
        isPWMove = true;
        isPWEdit = false;
    }

    public void UpdatePW(string input)
    {

    }

    public void CheckPW(string input)
    {

    }
}
