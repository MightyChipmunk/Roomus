using Ookii.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Images
{
    public string url;
}

public class AI_SendImage : MonoBehaviour
{
    private VistaOpenFileDialog m_OpenFileDialog = new VistaOpenFileDialog();

    private string[] m_FilePaths; // 파일 패스

    List<Texture2D> textures = new List<Texture2D>();

    public GameObject cropItem;
    public Transform content;

    public void OnFBXButtonOpenFile() // 버튼에 추가할 메서드
    {
        SetOpenFBXFileDialog();
        m_FilePaths = FileOpen(m_OpenFileDialog);

        if (m_FilePaths.Length > 0)
            OpenImageFile();
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
        m_OpenFileDialog.Filter = "Img 파일| *.jpg";
        m_OpenFileDialog.FilterIndex = 1;
        m_OpenFileDialog.Multiselect = false;
    }

    public void OpenImageFile()
    {
        byte[] data = File.ReadAllBytes(m_FilePaths[0]);

        //post
        StartCoroutine(Post("http://3.20.72.99:5000/image_furniture_detect", data));
    }

    IEnumerator Post(string url, byte[] img)
    {
        WWWForm form = new WWWForm();

        form.AddBinaryData("img", img, "Img", "application/jpg");

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Images[] images = JsonHelper.FromJson<Images>(www.downloadHandler.text);

            for (int i = 0; i < images.Length; i++)
            {
                UnityWebRequest texRequest = UnityWebRequestTexture.GetTexture(images[i].url);

                yield return texRequest.SendWebRequest();

                if (texRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(texRequest.error);
                }
                else
                {
                    //textures.Add(((DownloadHandlerTexture)texRequest.downloadHandler).texture);
                    GameObject go = Instantiate(cropItem, content);
                    go.GetComponent<AI_CropItem>().texture = ((DownloadHandlerTexture)texRequest.downloadHandler).texture;
                    go.GetComponent<AI_CropItem>().category = "Table";
                    Debug.Log("Image Crop complete");

                    texRequest.Dispose();
                }
            }

            Debug.Log("Form upload complete!");

            www.Dispose();
        }
    }
}
