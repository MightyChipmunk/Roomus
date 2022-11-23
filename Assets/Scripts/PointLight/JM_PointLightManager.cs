using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_PointLightManager : MonoBehaviour
{
    public GameObject lightFactory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNewLight()
    {
        GameObject light = Instantiate(lightFactory);
        light.transform.position = new Vector3(0, 5, 0);
    }
}
