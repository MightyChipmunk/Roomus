using UnityEngine;
using System.Windows.Forms;
using Ookii.Dialogs;
using System.IO;
using UnityEngine.UI;

public class ImageLoad : MonoBehaviour
{
    private VistaOpenFileDialog m_OpenFileDialog
        = new VistaOpenFileDialog();

    [SerializeField]
    private string[] m_FilePaths; // 파일 패스


    Texture2D texture;
    public Image image;
    Sprite sprite;

    public void Update()
    {
        if (m_FilePaths.Length > 0)
            OpenFile();
    }

    public void OnButtonOpenFile() // 버튼에 추가할 메서드
    {
        SetOpenFileDialog();
        m_FilePaths = FileOpen(m_OpenFileDialog);
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
        m_OpenFileDialog.Filter
            = "사진 파일 |*.png; *.jpg" +
            "|오디오 파일 |*.mp3; *.wav" +
            "|비디오 파일 |*.mp4; *.avi" +
            "|모든 파일|*.*";
        m_OpenFileDialog.FilterIndex = 1;
        m_OpenFileDialog.Multiselect = true;
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
}