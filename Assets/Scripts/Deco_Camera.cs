using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_Camera : MonoBehaviour
{
    Transform room;
    Camera cam;

    private void Start()
    {
        room = GameObject.Find("Room").transform;
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
        if (!cam.orthographic)
        {
            cam.orthographic = true;
            cam.orthographicSize = 7.0f;

            transform.position = new Vector3(transform.position.x, 5.0f, transform.position.z);
        }

        transform.eulerAngles = new Vector3(90, 0, 0);
    }

    void Third_Demen_Cam()
    {
        if (cam.orthographic)
        {
            cam.orthographic = false;

            transform.position = new Vector3(transform.position.x, 15.0f, transform.position.z);
        }
    }

    void First_Cam()
    {
        transform.position = Deco_ChangeView.Instance.FirstPos;
        transform.forward = Vector3.forward;
    }
}
