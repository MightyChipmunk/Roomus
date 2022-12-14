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
using UnityEngine.UI;

[Serializable]
public class Images
{
    public List<Positions> datas;
}

[Serializable]
public class Positions
{
    //public List<int> crop;
    public int[] crop = new int[3];
    public string category;
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
        foreach (Transform tr in content)
        {
            Destroy(tr.gameObject);
        }

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
        StartCoroutine(Post(UrlInfo.cropUrl + "furniture_detect", data));
    }

    IEnumerator Post(string url, byte[] img)
    {
        WWWForm form = new WWWForm();

        form.AddBinaryData("file", img, "Img", "application/jpg");

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Images images = JsonUtility.FromJson<Images>(www.downloadHandler.text);
                List<Positions> positions = images.datas;

                for (int i = 0; i < positions.Count; i++)
                {
                    Texture2D tex = new Texture2D(1, 1);
                    tex.LoadImage(img);
                    //tex.ReadPixels(new Rect(positions[i].crop[0], positions[i].crop[1], positions[i].crop[2], positions[i].crop[3]), 0, 0, false);
                    tex.Apply();

                    GameObject go = Instantiate(cropItem, content);

                    Rect rect = new Rect(positions[i].crop[0], positions[i].crop[1], positions[i].crop[2], positions[i].crop[3]);
                    //Rect rect = new Rect(0, 0, 100, 100);
                    go.GetComponent<Image>().sprite = Sprite.Create(tex, rect, new Vector2(0, 0));

                    var pixData = tex.GetPixels(positions[i].crop[0], positions[i].crop[1], positions[i].crop[2], positions[i].crop[3]);
                    var newTex = new Texture2D(positions[i].crop[2], positions[i].crop[3]);
                    newTex.SetPixels(pixData);
                    newTex.Apply();
                    go.GetComponent<AI_CropItem>().texture = newTex;
                    //go.GetComponent<AI_CropItem>().texBytes = imgBytes;
                    go.GetComponent<AI_CropItem>().category = positions[i].category;
                }

                Debug.Log("Form upload complete!");
            }
        }
    }
}
