using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_FurnInfoManager : MonoBehaviour
{
    public static JM_FurnInfoManager instance;

    public GameObject info1;
    public GameObject info2;

    public Transform showPos;
    public Transform originPos;

    public bool info1Move;
    public bool info2Move;
    public bool info1Show;
    public bool info2Show;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {       
        info1.transform.position = originPos.position;
        info2.transform.position = originPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (info1Move)
        {
            if (info1Show)
            {
                info1.transform.position = Vector3.Lerp(info1.transform.position, showPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(info1.transform.position, showPos.position) < 0.5f)
                {
                    info1.transform.position = showPos.position;
                    info1Move = false;
                }
            }
            if (!info1Show)
            {
                info1.transform.position = Vector3.Lerp(info1.transform.position, originPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(info1.transform.position, originPos.position) < 0.5f)
                {
                    info1.transform.position = originPos.position;
                    info1Move = false;
                }
            }
        }

        if (info2Move)
        {
            if (info2Show)
            {
                info2.transform.position = Vector3.Lerp(info2.transform.position, showPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(info2.transform.position, showPos.position) < 0.5f)
                {
                    info2.transform.position = showPos.position;
                    info2Move = false;
                }
            }
            if (!info2Show)
            {
                info2.transform.position = Vector3.Lerp(info2.transform.position, originPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(info2.transform.position, originPos.position) < 0.5f)
                {
                    info2.transform.position = originPos.position;
                    info2Move = false;
                }
            }
        }
    }

    public void ShowInit()
    {
        info1Move = true;
        info1Show = true;
    }

    public void ShowInfo1()
    {
        print("working");
        info1Move = true;
        info1Show = true;
        info2Move = true;
        info2Show = false;
    }

    public void ShowInfo2()
    {
        info1Move = true;
        info1Show = false;
        info2Move = true;
        info2Show = true;
    }

    public void Reset()
    {
        info1Move = false;
        info1Show = false;
        info2Move = false;
        info2Show = false;
        info1.transform.position = originPos.position;
        info2.transform.position = originPos.position;
    }
}
