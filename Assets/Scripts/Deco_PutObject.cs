using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_PutObject : MonoBehaviour
{
    Transform room;

    public GameObject objFactory;
    GameObject obj;
    bool canPut = true;
    public Material can;
    public Material cant;
    Material origMat;
    Renderer objRenderer;
    Deco_ObjectCol objCol;

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
                    objRenderer = obj.GetComponentInChildren<Renderer>();
                    obj.transform.parent = transform;
                    objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                    origMat = objRenderer.material;
                }
            }
            else if (Input.GetKey(KeyCode.G))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 16f, LayerMask.GetMask("Floor")))
                    obj.transform.position = hit.point;
                
                if (Input.GetKey(KeyCode.Q))
                    obj.transform.Rotate(0, -100f * Time.deltaTime, 0);
                else if (Input.GetKey(KeyCode.E))
                    obj.transform.Rotate(0, 100f * Time.deltaTime, 0);

                canPut = !objCol.IsCollide;
                if (canPut)
                    objRenderer.material = can;
                else
                    objRenderer.material = cant;
            }
            else if (Input.GetKeyUp(KeyCode.G) && canPut)
            {
                objRenderer.material = origMat;
                obj.GetComponentInChildren<Collider>().isTrigger = false;
                obj.transform.parent = room;
                obj = null;
            }
            else if (Input.GetKeyUp(KeyCode.G) && !canPut)
            {
                Destroy(obj);
                obj = null;
            }
        }
    }
}
