using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Deco_PutObject : MonoBehaviour
{
    public static Deco_PutObject Instance;

    public FBXJson fbxJson = new FBXJson();

    public GameObject objFactory;
    GameObject obj;
    bool canPut = true;
    public Material can;
    public Material cant;
    List<Material> origMats = new List<Material>();
    Deco_ObjectCol objCol;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Deco_ChangeView.Instance.viewState == Deco_ChangeView.ViewState.Second_Demen && fbxJson.location)
        {
            SecondPut();
        }
        else if (Deco_ChangeView.Instance.viewState == Deco_ChangeView.ViewState.Third_Demen)
        {
            ThirdPut();
        }
        else if (Deco_ChangeView.Instance.viewState == Deco_ChangeView.ViewState.First)
        {
            FirstPut();
        }
    }

    public void delObj()
    {
        if (obj)
        {
            Destroy(obj);
            obj = null;
        }
    }

    void SecondPut()
    {
        // 키를 누르면 오브젝트 미리보기 생성
        if (Input.GetKeyDown(KeyCode.G))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 16f, LayerMask.GetMask("Floor")))
            {
                //obj = Instantiate(objFactory);
                //obj.transform.parent = transform;
                //objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                //AddOrigMats();

                obj = new GameObject(fbxJson.furnitName);
                obj.transform.position = hit.point;
                obj.transform.parent = transform;
                GameObject go = Instantiate(objFactory);
                go.transform.parent = obj.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = objFactory.transform.eulerAngles;
                BoxCollider col = go.AddComponent<BoxCollider>();
                objCol = go.AddComponent<Deco_ObjectCol>();
                col.center = new Vector3(0, fbxJson.ySize / 2, 0);
                col.size = new Vector3(fbxJson.xSize, fbxJson.ySize, fbxJson.zSize);
                Rigidbody rb = go.AddComponent<Rigidbody>();
                rb.useGravity = false;
                Deco_Idx decoIdx = obj.AddComponent<Deco_Idx>();
                decoIdx.Name = fbxJson.furnitName;
                decoIdx.Price = fbxJson.price;
                decoIdx.Category = fbxJson.category;
                decoIdx.Idx = fbxJson.id;

                for (int i = 0; i < go.transform.childCount; i++)
                {
                    go.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture =
                        Resources.Load<Texture>(fbxJson.furnitName + "Tex" + i.ToString());
                }

                AddOrigMats();
            }
        }
        // 누르고 있는 동안 오브젝트 이동
        else if (Input.GetKey(KeyCode.G) && obj)
        {
            canPut = !objCol.IsCollide;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 16f, LayerMask.GetMask("Floor")))
                obj.transform.position = hit.point;
            else
            {
                canPut = false;
            }

            ChangeMat(canPut);

            if (Input.GetKey(KeyCode.Q))
                obj.transform.Rotate(0, -100f * Time.deltaTime, 0);
            else if (Input.GetKey(KeyCode.E))
                obj.transform.Rotate(0, 100f * Time.deltaTime, 0);
        }
        // 배치 가능할 시 키를 떼면 생성
        else if (Input.GetKeyUp(KeyCode.G) && canPut && obj)
        {
            Deco_Json.Instance.SaveJson(obj, obj.GetComponent<Deco_Idx>().Idx);
            ChangeToOrigMat();
            obj.GetComponentInChildren<Collider>().isTrigger = false;
            obj.GetComponentInChildren<Rigidbody>().useGravity = true;
            obj.transform.parent = GameObject.Find("Room").transform;
            obj = null;
        }
        // 배치 불가능 할 시 키를 떼면 제거
        else if (Input.GetKeyUp(KeyCode.G) && !canPut && obj)
        {
            Destroy(obj);
            obj = null;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 16f))
            {
                Deco_Idx deco_Idx;
                if (hit.transform.parent.TryGetComponent<Deco_Idx>(out deco_Idx))
                {
                    Deco_Json.Instance.DeleteJson(hit.transform.parent.gameObject);
                    Destroy(hit.transform.parent.gameObject);
                }
            }
        }
    }

    void ThirdPut()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Floor")) && fbxJson.location)
            {
                //obj = Instantiate(objFactory);
                //obj.transform.parent = transform;
                //objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                //AddOrigMats();

                obj = new GameObject(fbxJson.furnitName);
                obj.transform.position = hit.point;
                obj.transform.parent = transform;
                GameObject go = Instantiate(objFactory);
                go.transform.parent = obj.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = objFactory.transform.eulerAngles;
                BoxCollider col = go.AddComponent<BoxCollider>();
                objCol = go.AddComponent<Deco_ObjectCol>();
                col.center = new Vector3(0, fbxJson.ySize / 2, 0);
                col.size = new Vector3(fbxJson.xSize, fbxJson.ySize, fbxJson.zSize);
                Rigidbody rb = go.AddComponent<Rigidbody>();
                rb.useGravity = false;
                Deco_Idx decoIdx = obj.AddComponent<Deco_Idx>();
                decoIdx.Name = fbxJson.furnitName;
                decoIdx.Price = fbxJson.price;
                decoIdx.Category = fbxJson.category;
                decoIdx.Idx = fbxJson.id;

                for(int i = 0; i < go.transform.childCount; i++)
                {
                    go.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture =
                        Resources.Load<Texture>(fbxJson.furnitName + "Tex" + i.ToString());
                }

                AddOrigMats();
            }
            else if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Wall")) && !fbxJson.location)
            {
                //obj = Instantiate(objFactory);
                //obj.transform.parent = transform;
                //obj.transform.forward = hit.normal;
                //objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                //AddOrigMats();obj = new GameObject(fbxJson.furnitName);

                obj = new GameObject(fbxJson.furnitName);
                obj.transform.position = hit.point;
                obj.transform.parent = transform;
                obj.transform.forward = hit.normal;
                GameObject go = Instantiate(objFactory);
                go.transform.parent = obj.transform;
                go.transform.localPosition = Vector3.zero + Vector3.forward * (fbxJson.zSize / 2 + 0.01f); 
                go.transform.localEulerAngles = objFactory.transform.eulerAngles;
                BoxCollider col = go.AddComponent<BoxCollider>();
                objCol = go.AddComponent<Deco_ObjectCol>();
                col.center = new Vector3(0, fbxJson.ySize / 2, 0);
                col.size = new Vector3(fbxJson.xSize, fbxJson.ySize, fbxJson.zSize);
                Rigidbody rb = go.AddComponent<Rigidbody>();
                rb.useGravity = false;
                Deco_Idx decoIdx = obj.AddComponent<Deco_Idx>();
                decoIdx.Name = fbxJson.furnitName;
                decoIdx.Price = fbxJson.price;
                decoIdx.Category = fbxJson.category;
                decoIdx.Idx = fbxJson.id;

                for (int i = 0; i < go.transform.childCount; i++)
                {
                    go.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture =
                        Resources.Load<Texture>(fbxJson.furnitName + "Tex" + i.ToString());
                }

                AddOrigMats();
            }
        }
        else if (Input.GetKey(KeyCode.G) && obj)
        {
            canPut = !objCol.IsCollide;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Floor")) && fbxJson.location)
            {
                obj.transform.position = hit.point;
                Vector3 angle = obj.transform.eulerAngles;
                angle.z = 0;
                angle.x = hit.normal.x;
                obj.transform.eulerAngles = angle;
            }
            else if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Wall")) && !fbxJson.location)
            {
                obj.transform.position = hit.point;
                obj.transform.forward = hit.normal;
            }
            else
            {
                canPut = false;
            }

            ChangeMat(canPut);

            if (fbxJson.location)
            {
                if (Input.GetKey(KeyCode.Q))
                    obj.transform.Rotate(0, -100f * Time.deltaTime, 0);
                else if (Input.GetKey(KeyCode.E))
                    obj.transform.Rotate(0, 100f * Time.deltaTime, 0);
            }
        }
        else if (Input.GetKeyUp(KeyCode.G) && canPut && obj)
        {
            Deco_Json.Instance.SaveJson(obj, obj.GetComponent<Deco_Idx>().Idx);
            ChangeToOrigMat();
            obj.GetComponentInChildren<Collider>().isTrigger = false;
            if (fbxJson.location)
                obj.GetComponentInChildren<Rigidbody>().useGravity = true;
            obj.transform.parent = GameObject.Find("Room").transform;
            obj = null;
        }
        else if (Input.GetKeyUp(KeyCode.G) && !canPut && obj)
        {
            Destroy(obj);
            obj = null;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f))
            {
                Deco_Idx deco_Idx;
                if (hit.transform.parent.TryGetComponent<Deco_Idx>(out deco_Idx))
                {
                    Deco_Json.Instance.DeleteJson(hit.transform.parent.gameObject);
                    Destroy(hit.transform.parent.gameObject);
                }
            }
        }
    }

    void FirstPut()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, LayerMask.GetMask("Floor")) && fbxJson.location)
            {
                //obj = Instantiate(objFactory);
                //obj.transform.parent = transform;
                //obj.transform.forward = -Camera.main.transform.forward;
                //objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                //AddOrigMats();

                obj = new GameObject(fbxJson.furnitName);
                obj.transform.position = hit.point;
                obj.transform.parent = transform;
                obj.transform.forward = -Camera.main.transform.forward;
                GameObject go = Instantiate(objFactory);
                go.transform.parent = obj.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = objFactory.transform.eulerAngles;
                BoxCollider col = go.AddComponent<BoxCollider>();
                objCol = go.AddComponent<Deco_ObjectCol>();
                col.center = new Vector3(0, fbxJson.ySize / 2, 0);
                col.size = new Vector3(fbxJson.xSize, fbxJson.ySize, fbxJson.zSize);
                Rigidbody rb = go.AddComponent<Rigidbody>();
                rb.useGravity = false;
                Deco_Idx decoIdx = obj.AddComponent<Deco_Idx>();
                decoIdx.Name = fbxJson.furnitName;
                decoIdx.Price = fbxJson.price;
                decoIdx.Category = fbxJson.category;
                decoIdx.Idx = fbxJson.id;

                for (int i = 0; i < go.transform.childCount; i++)
                {
                    go.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture =
                        Resources.Load<Texture>(fbxJson.furnitName + "Tex" + i.ToString());
                }

                AddOrigMats();
            }
            else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, LayerMask.GetMask("Wall")) && !fbxJson.location)
            {
                //obj = Instantiate(objFactory);
                //obj.transform.parent = transform;
                //obj.transform.forward = hit.normal;
                //objCol = obj.transform.GetChild(0).gameObject.AddComponent<Deco_ObjectCol>();
                //AddOrigMats();

                obj = new GameObject(fbxJson.furnitName);
                obj.transform.position = hit.point;
                obj.transform.parent = transform;
                obj.transform.forward = hit.normal;
                GameObject go = Instantiate(objFactory);
                go.transform.parent = obj.transform;
                go.transform.localPosition = Vector3.zero + Vector3.forward * (fbxJson.zSize / 2 + 0.01f);
                go.transform.localEulerAngles = objFactory.transform.eulerAngles;
                BoxCollider col = go.AddComponent<BoxCollider>();
                objCol = go.AddComponent<Deco_ObjectCol>();
                col.center = new Vector3(0, fbxJson.ySize / 2, 0);
                col.size = new Vector3(fbxJson.xSize, fbxJson.ySize, fbxJson.zSize);
                Rigidbody rb = go.AddComponent<Rigidbody>();
                rb.useGravity = false;
                Deco_Idx decoIdx = obj.AddComponent<Deco_Idx>();
                decoIdx.Name = fbxJson.furnitName;
                decoIdx.Price = fbxJson.price;
                decoIdx.Category = fbxJson.category;
                decoIdx.Idx = fbxJson.id;

                for (int i = 0; i < go.transform.childCount; i++)
                {
                    go.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture =
                        Resources.Load<Texture>(fbxJson.furnitName + "Tex" + i.ToString());
                }

                AddOrigMats();
            }
        }
        else if (Input.GetKey(KeyCode.G) && obj)
        {
            canPut = !objCol.IsCollide;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, LayerMask.GetMask("Floor")) && fbxJson.location)
            {
                obj.transform.position = hit.point;
                Vector3 angle = obj.transform.eulerAngles;
                angle.z = 0;
                angle.x = hit.normal.x;
                obj.transform.eulerAngles = angle;
            }
            else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, LayerMask.GetMask("Wall")) && !fbxJson.location)
            {
                obj.transform.position = hit.point;
                obj.transform.forward = hit.normal;
            }
            else
            {
                canPut = false;
            }

            ChangeMat(canPut);

            if (fbxJson.location)
            {
                if (Input.GetKey(KeyCode.Q))
                    obj.transform.Rotate(0, -100f * Time.deltaTime, 0);
                else if (Input.GetKey(KeyCode.E))
                    obj.transform.Rotate(0, 100f * Time.deltaTime, 0);
            }
        }
        else if (Input.GetKeyUp(KeyCode.G) && canPut && obj)
        {
            Deco_Json.Instance.SaveJson(obj, obj.GetComponent<Deco_Idx>().Idx);
            ChangeToOrigMat();
            obj.GetComponentInChildren<Collider>().isTrigger = false;
            if (fbxJson.location)
                obj.GetComponentInChildren<Rigidbody>().useGravity = true;
            obj.transform.parent = GameObject.Find("Room").transform;
            obj = null;
        }
        else if (Input.GetKeyUp(KeyCode.G) && !canPut && obj)
        {
            Destroy(obj);
            obj = null;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f))
            {
                Deco_Idx deco_Idx;
                if (hit.transform.parent.TryGetComponent<Deco_Idx>(out deco_Idx))
                {
                    Deco_Json.Instance.DeleteJson(hit.transform.parent.gameObject);
                    Destroy(hit.transform.parent.gameObject);
                }
            }
        }
    }

    void ChangeMat(bool value)
    {
        Transform go = obj.transform.GetChild(0);
        if (value)
        {
            for (int i = 0; i < go.childCount; i++)
            {
                go.GetChild(i).GetComponent<Renderer>().material = can;
            }
        }
        else
        {
            for (int i = 0; i < go.childCount; i++)
            {
                go.GetChild(i).GetComponent<Renderer>().material = cant;
            }
        }
    }

    void AddOrigMats()
    {
        origMats.Clear();
        Transform go = obj.transform.GetChild(0);
        for (int i = 0; i < go.childCount; i++)
        {
            origMats.Add(go.GetChild(i).GetComponent<Renderer>().material);
        }
    }

    void ChangeToOrigMat()
    {
        Transform go = obj.transform.GetChild(0);
        for (int i = 0; i < go.childCount; i++)
        {
            go.GetChild(i).GetComponent<Renderer>().material = origMats[i];
        }
    }
}
