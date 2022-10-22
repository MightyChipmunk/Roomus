using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deco_FurnitItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClicked()
    {
        foreach (GameObject go in Deco_Json.Instance.objects.datas)
        {
            if (go.GetComponent<Deco_Idx>().Idx == Int32.Parse(gameObject.name))
            {
                Deco_PutObject.Instance.objFactory = go;
            }
        }
    }
}
