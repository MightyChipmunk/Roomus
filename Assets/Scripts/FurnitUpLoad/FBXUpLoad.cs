using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Ookii.Dialogs;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

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

    Texture currentTex;

    public List<Button> buttons = new List<Button>();

    public static FBXUpLoad Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);    
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
            currentTex = Resources.Load("Pallet" + buttonIdx.ToString()) as Texture;
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
        string path = UnityEngine.Application.streamingAssetsPath + "/" + fileName + ".fbx";
        byte[] data = File.ReadAllBytes(m_FilePaths[0]);

        File.WriteAllBytes(path, data);

        path = UnityEngine.Application.dataPath + "/Resources/" + fileName + ".fbx";
        File.WriteAllBytes(path, data);

        StartCoroutine(WaitForFile(path)); 
    }

    public void OpenImageFile(int buttonIdx)
    {
        //if (obj.transform.childCount <= idx)
        //    return;
        //string path = UnityEngine.Application.streamingAssetsPath + "/" + fileName + "Pallet" + buttonIdx.ToString() + ".jpg";
        byte[] data = File.ReadAllBytes(m_FilePaths[0]);
        //string path = UnityEngine.Application.dataPath + "/Resources/" + Path.GetFileName(m_FilePaths[0]);
        string path = UnityEngine.Application.dataPath + "/Resources/" + "Pallet" + buttonIdx.ToString() + ".jpg";

        File.WriteAllBytes(path, data);

        StartCoroutine(WaitForImage(path, buttonIdx));
    }

    void ChangeMat(Transform obj, Texture texture)
    {
        obj.GetComponent<Renderer>().material.mainTexture = texture;
        string path = UnityEngine.Application.dataPath + "/Resources/" + "Pallet" + idx.ToString() + ".jpg";
        byte[] data = File.ReadAllBytes(path);
        int objIdx = obj.GetSiblingIndex();
        path = UnityEngine.Application.streamingAssetsPath + "/" + fileName + "Tex" + objIdx.ToString() + ".jpg";
        File.WriteAllBytes(path, data);
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

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Rigidbody rig = obj.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            rig.useGravity = false;
            rig.isKinematic = true;
            MeshCollider col = obj.transform.GetChild(i).gameObject.AddComponent<MeshCollider>();
        }

        GameObject.Find("CamPos").GetComponent<FBXCamController>().target = obj;
    }

    IEnumerator WaitForImage(string path, int buttonIdx)
    {
        while (true)
        {
            if (Resources.Load("Pallet" + buttonIdx.ToString()))
                break;

            yield return null;
        }

        Texture2D tex = Resources.Load("Pallet" + buttonIdx.ToString()) as Texture2D;
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        buttons[buttonIdx].GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0.3f, 0.3f));
        buttons[buttonIdx].transform.GetChild(0).gameObject.SetActive(false);
    }
}
