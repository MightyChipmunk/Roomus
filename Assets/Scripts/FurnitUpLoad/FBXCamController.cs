using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBXCamController : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 100f;
    [SerializeField]
    float zoomSpeed = 3;

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float mx;
    float my;
    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
            // 마우스 위치에 따른 각도 조절
            float mh = Input.GetAxis("Horizontal");
            float mv = Input.GetAxis("Vertical");
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            mx += mh * rotSpeed * Time.deltaTime;
            my += mv * rotSpeed * Time.deltaTime;
            my = Mathf.Clamp(my, -60, 60);
            transform.eulerAngles = new Vector3(-my, mx, 0);
            transform.position = target.transform.position;
            Camera.main.transform.position += scrollWheel * transform.forward * zoomSpeed * 50 * Time.deltaTime;
        }
    }
}
