using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Ookii.Dialogs;
using UnityEngine;
using UnityEngine.UI;
using TriLibCore;
using TriLibCore.General;
using Button = UnityEngine.UI.Button;
using UnityEngine.Networking;

public class FBXUpLoad : MonoBehaviour
{
    private VistaOpenFileDialog m_OpenFileDialog = new VistaOpenFileDialog();

    private string[] m_FilePaths; // 파일 패스

    int idx = 0;
    string fileName;
    public string FileName
    {
        get { return fileName; }
        set { fileName = value; }
    }
    GameObject obj;

    Texture2D currentTex;

    public List<Button> buttons = new List<Button>();

    public static FBXUpLoad Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Directory.CreateDirectory(UnityEngine.Application.dataPath + "/LocalServer");
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 50f) && currentTex)
        {
            ChangeMat(hit.transform, currentTex);
        }
    }

    public void OnFBXButtonOpenFile() // 버튼에 추가할 메서드
    {
        SetOpenFBXFileDialog();
        m_FilePaths = FileOpen(m_OpenFileDialog);

        if (m_FilePaths.Length > 0)
            OpenFBXFile();
    }

    public void OnImageButtonOpenFile(int buttonIdx) // 버튼에 추가할 메서드
    {
        if (buttons[buttonIdx].transform.GetChild(0).gameObject.activeSelf)
        {
            SetOpenImageFileDialog();
            m_FilePaths = FileOpen(m_OpenFileDialog);

            if (m_FilePaths.Length > 0)
                OpenImageFile(buttonIdx);
        }
        else
        {
            currentTex = buttons[buttonIdx].GetComponent<Image>().mainTexture as Texture2D;
            idx = buttonIdx;
        }
    }

    public void OnDeleteButton(int buttonIdx)
    {
        if (!buttons[buttonIdx].transform.GetChild(0).gameObject.activeSelf)
        {
            buttons[buttonIdx].transform.GetChild(0).gameObject.SetActive(true);
            buttons[buttonIdx].GetComponent<Image>().sprite = Resources.Load<Sprite>("BtnUI");
        }
    }

    string[] FileOpen(VistaOpenFileDialog openFileDialog)
    {
        var result = openFileDialog.ShowDialog();
        var filenames = result == DialogResult.OK ?
            openFileDialog.FileNames :
            new string[0];
        openFileDialog.Dispose();
        return filenames;
    }

    void SetOpenFBXFileDialog()
    {
        m_OpenFileDialog.Title = "파일 열기";
        m_OpenFileDialog.Filter = "Fbx 파일| *.fbx";
        m_OpenFileDialog.FilterIndex = 1;
        m_OpenFileDialog.Multiselect = false;
    }

    void SetOpenImageFileDialog()
    {
        m_OpenFileDialog.Title = "파일 열기";
        m_OpenFileDialog.Filter = "Image 파일| *.jpg";
        m_OpenFileDialog.FilterIndex = 1;
        m_OpenFileDialog.Multiselect = false;
    }

    public void OpenFBXFile()
    {
        //fileName = Path.GetFileName(m_FilePaths[0]).Substring(0, Path.GetFileName(m_FilePaths[0]).Length - 4);
        string path = UnityEngine.Application.dataPath + "/LocalServer/" + fileName + ".fbx";
        byte[] data = File.ReadAllBytes(m_FilePaths[0]);

        //post
        File.WriteAllBytes(path, data);

        path = UnityEngine.Application.persistentDataPath + "/" + fileName + ".fbx";
        File.WriteAllBytes(path, data);

        StartCoroutine(WaitForFile(path)); 
    }

    public void OpenImageFile(int buttonIdx)
    {
        byte[] data = File.ReadAllBytes(m_FilePaths[0]);
        string path = UnityEngine.Application.persistentDataPath + "/Pallet" + buttonIdx.ToString() + ".jpg";
        File.WriteAllBytes(path, data);

        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(data);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        buttons[buttonIdx].GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
        buttons[buttonIdx].transform.GetChild(0).gameObject.SetActive(false);
    }

    void ChangeMat(Transform obj, Texture2D texture)
    {
        obj.GetComponent<Renderer>().material.mainTexture = texture;
        byte[] data = File.ReadAllBytes(UnityEngine.Application.persistentDataPath + "/Pallet" + idx.ToString() + ".jpg");
        int objIdx = obj.GetSiblingIndex();
        //post
        string path = UnityEngine.Application.dataPath + "/LocalServer/" + fileName + "Tex" + objIdx.ToString() + ".jpg";
        File.WriteAllBytes(path, data);


    }

    IEnumerator WaitForFile(string path)
    {
        while(true)
        {
            if (File.Exists(path))
                break;
            yield return null;
        }

        var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        AssetLoader.LoadModelFromFile(path, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
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
        obj = assetLoaderContext.RootGameObject.transform.GetChild(0).gameObject;
        obj.transform.parent = null;
        //Destroy(assetLoaderContext.RootGameObject);
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).GetComponent<MeshRenderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
        }
        obj.name = fileName;

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Rigidbody rig = obj.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            rig.useGravity = false;
            rig.isKinematic = true;
            MeshCollider col = obj.transform.GetChild(i).gameObject.AddComponent<MeshCollider>();
        }

        GameObject.Find("CamPos").GetComponent<FBXCamController>().target = obj;
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
