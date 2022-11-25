using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_PtLightBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPtLightBtn()
    {
        JM_LightManager.instance.OnClickPtLightBtn();
    }
}
