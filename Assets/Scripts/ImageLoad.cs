using UnityEngine;
using System.Windows.Forms;
using Ookii.Dialogs;
using System.IO;
using UnityEngine.UI;
using Screen = UnityEngine.Screen;
using System.Collections;

public class ImageLoad : MonoBehaviour
{
    private VistaOpenFileDialog m_OpenFileDialog = new VistaOpenFileDialog();

    private string[] m_FilePaths; // 파일 패스

    Texture2D texture;
    public Image image;
    Sprite sprite;

    public void Update()
    {

    }

    public void OnButtonOpenFile() // 버튼에 추가할 메서드
    {
        SetOpenFileDialog();
        m_FilePaths = FileOpen(m_OpenFileDialog);

        if (m_FilePaths.Length > 0)
            OpenFile();
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

    void SetOpenFileDialog()
    {
        m_OpenFileDialog.Title = "파일 열기";
        m_OpenFileDialog.Filter = "Image 파일| *.jpg";
        m_OpenFileDialog.FilterIndex = 1;
        m_OpenFileDialog.Multiselect = false;
    }

    public void OpenFile()
    {
        byte[] byteTexture = File.ReadAllBytes(m_FilePaths[0]);
        if (byteTexture.Length > 0)
        {
            texture = new Texture2D(0, 0);
            texture.LoadImage(byteTexture);
        }

        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
    }

    IEnumerator capture()
    {
        yield return new WaitForEndOfFrame();

        byte[] imgBytes;
        string path = UnityEngine.Application.dataPath + "/ImageServer/";

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture.Apply();

        imgBytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, imgBytes);
    }
}