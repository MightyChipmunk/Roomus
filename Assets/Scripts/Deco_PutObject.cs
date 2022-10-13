using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_PutObject : MonoBehaviour
{
    Transform room;

    public GameObject objFactory;
    GameObject obj;

    public Material can;
    public Material cant;
    Material origMat;

    // Start is called before the first frame update
    void Start()
    {
        room = GameObject.Find("Room").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Deco_ChangeView.Instance.viewState == Deco_ChangeView.ViewState.Second_Demen)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 16f, LayerMask.GetMask("Floor")))
                {
                    obj = Instantiate(objFactory);
                    obj.transform.parent = transform;
                    //obj.transform.position = hit.point + obj.GetComponent<Collider>().;
                    //origMat
                }
            }
            else if (Input.GetKey(KeyCode.G))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 16f, LayerMask.GetMask("Floor")))
                {
                    obj.transform.position = hit.point;
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    obj.transform.Rotate(0, -50f * Time.deltaTime, 0);
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    obj.transform.Rotate(0, 50f * Time.deltaTime, 0);
                }
            }
            else if (Input.GetKeyUp(KeyCode.G))
            {
                obj.transform.parent = room;
                obj = null;
            }
        }
    }
}
