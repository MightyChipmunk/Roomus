using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_ImageUIManager : MonoBehaviour
{
    public GameObject imageGo;
    public Transform originPos;
    public Transform showPos;
    bool isMove;
    bool isShow;
    float currentTime;


    // Start is called before the first frame update
    void Start()
    {
        imageGo.SetActive(false);
        imageGo.transform.position = originPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            if (isShow)
            {
                imageGo.transform.position = Vector3.Lerp(imageGo.transform.position, showPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(imageGo.transform.position, showPos.position) < 0.5f)
                {
                    imageGo.transform.position = showPos.position;
                    currentTime += Time.deltaTime;
                    if (currentTime >= 2)
                    {
                        isShow = false;
                        currentTime = 0;
                    }
                }
            }
            else
            {
                imageGo.transform.position = Vector3.Lerp(imageGo.transform.position, originPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(imageGo.transform.position, originPos.position) < 0.5f)
                {
                    imageGo.transform.position = originPos.position;
                    imageGo.SetActive(false);
                }
            }
        }
    }

    public void SetImage()
    {
        imageGo.SetActive(true);
        isMove = true;
        isShow = true;
    }
}
