using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_Camera : MonoBehaviour
{
    Transform room;
    Transform thirdCamPos;
    Camera cam;

    [SerializeField]
    float speed;
    [SerializeField]
    float rotSpeed;

    private void Start()
    {
        room = GameObject.Find("Room").transform;
        thirdCamPos = GameObject.Find("ThirdCamPos").transform;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(Deco_ChangeView.Instance.viewState)
        {
            case Deco_ChangeView.ViewState.Second_Demen:
                Second_Demen_Cam();
                break;
            case Deco_ChangeView.ViewState.Third_Demen:
                Third_Demen_Cam();
                break;
            case Deco_ChangeView.ViewState.First:
                First_Cam();
                break;
        }
    }

    void Second_Demen_Cam()
    {
        // 캠을 orthographic으로 전환
        if (!cam.orthographic)
        {
            cam.orthographic = true;
            cam.orthographicSize = 7.0f;

            transform.position = new Vector3(room.position.x, 15.0f, room.position.z);
        }
        // 캠 각도 고정
        transform.eulerAngles = new Vector3(90, 0, 0);

        Vector3 dir;
        // WASD 키로 캠 위치 이동
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");
        dir = h * Vector3.right + v * Vector3.forward;
        dir.Normalize();
        // 마우스 좌클릭으로 캠 위치 이동
        if (Input.GetMouseButton(0))
            dir = (-mh * Vector3.right + -mv * Vector3.forward) * 2;
        transform.position += dir * speed * 5 * Time.deltaTime;
    }

    void Third_Demen_Cam()
    {
        // 캠의 orthographic을 해제
        if (cam.orthographic)
            cam.orthographic = false;

        Vector3 dir;
        // WASD 키로 캠 위치 이동
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");
        dir = h * transform.right + v * transform.up;
        // 마우스 좌클릭으로 캠 위치 이동
        if (Input.GetMouseButton(0))
            dir = -mh * transform.right + -mv * transform.up;
        thirdCamPos.position += dir * speed * 5 * Time.deltaTime;
        // 마우스 우클릭으로 캠 각도 이동
        Vector3 rotDir = new Vector3(-mv, mh, 0);
        if (Input.GetMouseButton(1))
            thirdCamPos.eulerAngles += rotDir * rotSpeed * 5 * Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, thirdCamPos.position, Time.deltaTime * 8.0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, thirdCamPos.rotation, Time.deltaTime * 8.0f);
    }

    float mx;
    float my;
    void First_Cam()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = h * transform.right + v * transform.forward;
        dir.y = 0;
        dir.Normalize();
        transform.position += dir * Time.deltaTime * speed;

        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");
        mx += mh * rotSpeed * Time.deltaTime;
        my += mv * rotSpeed * Time.deltaTime;
        my = Mathf.Clamp(my, -60, 60);
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
