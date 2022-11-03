using System.Collections;
using System.Collections.Generic;
using System.IO;
using TriLibCore;
using UnityEngine;

public class Deco_PutObject : MonoBehaviour
{
    public static Deco_PutObject Instance;

    public FBXJson fbxJson = new FBXJson();

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

    public void LoadFBX()
    {
        var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        string path = Application.dataPath + "/LocalServer/" + fbxJson.furnitName + ".fbx";
        AssetLoader.LoadModelFromFile(path, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);

        StartCoroutine(WaitForObj());
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
                //var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
                //string path = Application.dataPath + "/LocalServer/" + fbxJson.furnitName + ".fbx";
                //AssetLoader.LoadModelFromFile(path, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);

                StartCoroutine(WaitForObj_t(hit));
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
            GameObject loadObj = Instantiate(obj, transform);
            loadObj.transform.position = obj.transform.position;
            loadObj.transform.eulerAngles = obj.transform.eulerAngles;
            loadObj.transform.localScale = obj.transform.localScale;
            ChangeToOrigMat(loadObj);
            loadObj.name = obj.name;
            loadObj.GetComponentInChildren<Deco_ObjectCol>().enabled = false;
            Deco_Json.Instance.SaveJson(loadObj, loadObj.GetComponent<Deco_Idx>().Idx);
            loadObj.GetComponentInChildren<Collider>().isTrigger = false;
            if (fbxJson.location)
                loadObj.GetComponentInChildren<Rigidbody>().useGravity = true;
            loadObj.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            loadObj.transform.parent = GameObject.Find("Room").transform;
            obj.SetActive(false);
        }
        // 배치 불가능 할 시 키를 떼면 제거
        else if (Input.GetKeyUp(KeyCode.G) && !canPut && obj)
        {
            obj.SetActive(false);
            canPut = true;
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
            if (Physics.Raycast(ray, out hit, 50f, LayerMask.GetMask("Floor", "Wall")))
            {
                //var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
                //string path = Application.dataPath + "/LocalServer/" + fbxJson.furnitName + ".fbx";
                //AssetLoader.LoadModelFromFile(path, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);

                //StartCoroutine(WaitForObj(hit));

                StartCoroutine(WaitForObj_t(hit));
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
        // 배치 가능할 시 키를 떼면 생성
        else if (Input.GetKeyUp(KeyCode.G) && canPut && obj)
        {
            GameObject loadObj = Instantiate(obj, transform);
            loadObj.transform.position = obj.transform.position;
            loadObj.transform.eulerAngles = obj.transform.eulerAngles;
            loadObj.transform.localScale = obj.transform.localScale;
            ChangeToOrigMat(loadObj);
            loadObj.name = obj.name;
            loadObj.GetComponentInChildren<Deco_ObjectCol>().enabled = false;
            Deco_Json.Instance.SaveJson(loadObj, loadObj.GetComponent<Deco_Idx>().Idx);
            loadObj.GetComponentInChildren<Collider>().isTrigger = false;
            if (fbxJson.location)
                loadObj.GetComponentInChildren<Rigidbody>().useGravity = true;
            loadObj.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            loadObj.transform.parent = GameObject.Find("Room").transform;
            obj.SetActive(false);
        }
        // 배치 불가능 할 시 키를 떼면 제거
        else if (Input.GetKeyUp(KeyCode.G) && !canPut && obj)
        {
            obj.SetActive(false);
            canPut = true;
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
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, LayerMask.GetMask("Floor", "Wall")))
            {
                //var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
                //string path = Application.dataPath + "/LocalServer/" + fbxJson.furnitName + ".fbx";
                //AssetLoader.LoadModelFromFile(path, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);

                StartCoroutine(WaitForObj_t(hit));
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
        // 배치 가능할 시 키를 떼면 생성
        else if (Input.GetKeyUp(KeyCode.G) && canPut && obj)
        {
            GameObject loadObj = Instantiate(obj, transform);
            loadObj.transform.position = obj.transform.position;
            loadObj.transform.eulerAngles = obj.transform.eulerAngles;
            loadObj.transform.localScale = obj.transform.localScale;
            ChangeToOrigMat(loadObj);
            loadObj.name = obj.name;
            loadObj.GetComponentInChildren<Deco_ObjectCol>().enabled = false;
            Deco_Json.Instance.SaveJson(loadObj, loadObj.GetComponent<Deco_Idx>().Idx);
            loadObj.GetComponentInChildren<Collider>().isTrigger = false;
            if (fbxJson.location)
                loadObj.GetComponentInChildren<Rigidbody>().useGravity = true;
            loadObj.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            loadObj.transform.parent = GameObject.Find("Room").transform;
            obj.SetActive(false);
        }
        // 배치 불가능 할 시 키를 떼면 제거
        else if (Input.GetKeyUp(KeyCode.G) && !canPut && obj)
        {
            obj.SetActive(false);
            canPut = true;
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

    void ChangeToOrigMat(GameObject _obj)
    {
        Transform go = _obj.transform.GetChild(0);
        for (int i = 0; i < go.childCount; i++)
        {
            go.GetChild(i).GetComponent<Renderer>().material = origMats[i];
        }
    }

    IEnumerator WaitForObj_t(RaycastHit hit)
    {
        while (!obj)
        {
            yield return null;
        }

        obj.transform.position = hit.point;
        if (!fbxJson.location)
            obj.transform.forward = hit.normal;
        else if (Deco_ChangeView.Instance.viewState == Deco_ChangeView.ViewState.First)
            obj.transform.forward = -Camera.main.transform.forward;
        obj.SetActive(true);
    }


    IEnumerator WaitForObj()
    {
        while (!obj)
        {
            yield return null;
        }

        obj.transform.parent = transform;
        obj.SetActive(false);
    }

    #region Trilib
    /// <summary>
    /// Called when any error occurs.
    /// </summary>
    /// <param name="obj">The contextualized error, containing the original exception and the context passed to the method where the error was thrown.</param>
    private void OnError(IContextualizedError obj)
    {
        Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
    }

    /// <summary>
    /// Called when the Model loading progress changes.
    /// </summary>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    /// <param name="progress">The loading progress.</param>
    private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
    {
        Debug.Log($"Loading Model. Progress: {progress:P}");
    }

    /// <summary>
    /// Called when the Model (including Textures and Materials) has been fully loaded.
    /// </summary>
    /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Materials loaded. Model fully loaded.");
        obj = assetLoaderContext.RootGameObject;
        GameObject go = obj.transform.GetChild(0).gameObject;
        if (fbxJson.location)
            go.transform.localPosition = Vector3.zero;
        else
            go.transform.localPosition = Vector3.zero + Vector3.forward * (fbxJson.zSize / 2 + 0.01f);
        go.transform.localRotation = Quaternion.identity;
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
            go.transform.GetChild(i).GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
        }

        obj.name = fbxJson.furnitName;

        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (File.Exists(Application.dataPath + "/LocalServer/" + fbxJson.furnitName + "Tex" + i.ToString() + ".jpg"))
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(File.ReadAllBytes(Application.dataPath + "/LocalServer/" + fbxJson.furnitName + "Tex" + i.ToString() + ".jpg"));
                go.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = tex;
            }
        }

        AddOrigMats();
    }

    /// <summary>
    /// Called when the Model Meshes and hierarchy are loaded.
    /// </summary>
    /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    private void OnLoad(AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Model loaded. Loading materials.");
    }
    #endregion
}
