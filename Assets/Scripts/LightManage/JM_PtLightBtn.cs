using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_PtLightBtn : MonoBehaviour
{
    public InputField input;
    string sptLightName;

    // Start is called before the first frame update
    void Start()
    {
        input.onSubmit.AddListener(OnSubmitName);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPtLightBtn()
    {
        JM_LightManager.instance.OnClickPtLightBtn();
    }

    public void OnSubmitName(string s)
    {
        input.placeholder.GetComponent<Text>().text = s;
        input.Select();
        input.text = "";
        //sptLightName = s;
    }
}
