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
    string fileName;
    public string FileName
    {
        get { return fileName; }
        set { fileName = value; }
    }
    Transform obj;

    public static FBXUpLoad Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Directory.CreateDirectory(UnityEngine.Application.dataPath + "/LocalServer");
    }

    private void Start()
    {
        
    }

    void Update()
    {

    }

    FolderBrowserDialog fbd = new FolderBrowserDialog();

    string f_FilePath;

    public void OnFBXButtonOpenFolder() // 버튼에 추가할 메서드
    {
        if (fileName == null)
            return;

        if (fbd.ShowDialog() == DialogResult.OK)
        {
            f_FilePath = fbd.SelectedPath;
        }

        if (f_FilePath.Length > 0)
        {
            OpenFolder();
        }
    }

    void OpenFolder()
    {
        if (fileName == null)
            return;

        DirectoryInfo di = new DirectoryInfo(f_FilePath);
        Directory.Delete(UnityEngine.Application.dataPath + "/LocalServer", true);
        foreach (FileInfo file in di.GetFiles())
        {
            Directory.CreateDirectory(UnityEngine.Application.dataPath + "/LocalServer/" + FileName);
            string path = UnityEngine.Application.dataPath + "/LocalServer/" + FileName + "/" + file.Name;
            byte[] data = File.ReadAllBytes(file.FullName);

            File.WriteAllBytes(path, data);
        }
        string zipPath = UnityEngine.Application.dataPath + "/LocalServer/" + FileName + "/";
        ZipManager.ZipFiles(zipPath, zipPath + FileName + ".zip", "", false);

        StartCoroutine(WaitForZipFile(zipPath + FileName + ".zip"));
    }

    IEnumerator WaitForZipFile(string path)
    {
        while (!File.Exists(path))
        {
            yield return null;
        }

        var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        AssetLoaderZip.LoadModelFromZipFile(path, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
    }

    #region Trilib
    /// <summary>
    /// Called when the the Model begins to load.
    /// </summary>
    /// <param name="filesSelected">Indicates if any file has been selected.</param>
    private void OnBeginLoad(bool filesSelected)
    {
        
    }

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

        obj = assetLoaderContext.RootGameObject.transform.GetChild(0);

        MaterialLoader.Instance.ChangeMat(obj, UnityEngine.Application.dataPath + "/LocalServer/" + fileName);


        GameObject.Find("CamPos").GetComponent<FBXCamController>().target = obj.gameObject;
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
