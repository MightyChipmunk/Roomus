using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TriLibCore;
using TriLibCore.URP.Mappers;
using UnityEngine;

public class FolderUpload : MonoBehaviour
{
    FolderBrowserDialog fbd = new FolderBrowserDialog();

    string f_FilePath;

    private void Start()
    {
        
    }


    public void OnFBXButtonOpenFile() // 버튼에 추가할 메서드
    {
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
        List<string> paths = new List<string>();

        DirectoryInfo di = new DirectoryInfo(f_FilePath);
        foreach (FileInfo file in di.GetFiles())
        {
            Directory.CreateDirectory(UnityEngine.Application.dataPath + "/" + di.Name);
            string path = UnityEngine.Application.dataPath + "/" + di.Name + "/" + file.Name;
            paths.Add(path);
            byte[] data = File.ReadAllBytes(file.FullName);

            File.WriteAllBytes(path, data);
        }
        string zipPath = UnityEngine.Application.dataPath + "/" + di.Name + "/";
        ZipManager.ZipFiles(zipPath, zipPath + "zipFile.zip", "", false);

        StartCoroutine(WaitForFile(zipPath + "zipFile.zip"));
    }

    IEnumerator WaitForFile(string path)
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
