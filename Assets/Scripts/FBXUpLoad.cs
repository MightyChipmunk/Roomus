using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Ookii.Dialogs;
using UnityEngine;
using UnityEngine.UI;

public class FBXUpLoad : MonoBehaviour
{
    private VistaOpenFileDialog m_OpenFileDialog = new VistaOpenFileDialog();

    [SerializeField]
    private string[] m_FilePaths; // 파일 패스

    int idx = 0;
    string fileName;
    GameObject obj;

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    string fileName = "sofa.fbx";
        //    string fileNameTex = fileName + "Tex";
        //    string path = UnityEngine.Application.streamingAssetsPath + "/" + fileName;
        //    string pathTex = UnityEngine.Application.streamingAssetsPath + "/" + fileNameTex;
        //    Directory.CreateDirectory(UnityEngine.Application.streamingAssetsPath + fileName);
        //    byte[] data = File.ReadAllBytes(path);
        //    byte[] dataTex = File.ReadAllBytes(pathTex);

        //    path = UnityEngine.Application.dataPath + "/Resources/" + fileName;
        //    pathTex = UnityEngine.Application.dataPath + "/Resources/" + fileNameTex;
        //    File.WriteAllBytes(path, data);
        //    File.WriteAllBytes(pathTex, dataTex);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    //모델 로드하고
        //    GameObject obj = Instantiate(Resources.Load("Test") as GameObject);

        //    ChangeMat(obj.transform, Resources.Load("TestTex") as Texture);
        //}


    }

    public void OnFBXButtonOpenFile() // 버튼에 추가할 메서드
    {
        SetOpenFBXFileDialog();
        m_FilePaths = FileOpen(m_OpenFileDialog);

        if (m_FilePaths.Length > 0)
            OpenFBXFile();
    }

    public void OnImageButtonOpenFile() // 버튼에 추가할 메서드
    {
        if(fileName != null)
        {
            SetOpenImageFileDialog();
            m_FilePaths = FileOpen(m_OpenFileDialog);

            if (m_FilePaths.Length > 0)
                OpenImageFile();
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
        fileName = Path.GetFileName(m_FilePaths[0]).Substring(0, Path.GetFileName(m_FilePaths[0]).Length - 4);
        string path = UnityEngine.Application.streamingAssetsPath + "/" + fileName + ".fbx";
        byte[] data = File.ReadAllBytes(m_FilePaths[0]);

        File.WriteAllBytes(path, data);

        path = UnityEngine.Application.dataPath + "/Resources/" + fileName + ".fbx";
        File.WriteAllBytes(path, data);

        StartCoroutine(WaitForFile(path)); 
    }

    public void OpenImageFile()
    {
        if (obj.transform.childCount <= idx)
            return;
        string path = UnityEngine.Application.streamingAssetsPath + "/" + fileName + "Tex" + idx.ToString() + ".jpg";
        byte[] data = File.ReadAllBytes(m_FilePaths[0]);

        File.WriteAllBytes(path, data);

        path = UnityEngine.Application.dataPath + "/Resources/" + fileName + "Tex" + idx.ToString() + ".jpg";
        File.WriteAllBytes(path, data);

        StartCoroutine(WaitForImage(path)); 
    }

    void ChangeMat(Transform obj, Texture texture)
    {
       obj.GetChild(idx).GetComponent<Renderer>().material.mainTexture = texture;
    }

    IEnumerator WaitForFile(string path)
    {
        while(true)
        {
            if (Resources.Load(fileName))
                break;

            yield return null;
        }

        obj = Instantiate(Resources.Load(fileName) as GameObject);
        obj.name = fileName;
    }

    IEnumerator WaitForImage(string path)
    {
        while (true)
        {
            if (Resources.Load(fileName + "Tex" + idx.ToString()))
                break;

            yield return null;
        }

        ChangeMat(GameObject.Find(fileName).transform, Resources.Load(fileName + "Tex" + idx.ToString()) as Texture);
        idx++;
    }
}
