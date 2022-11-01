using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    public GameObject image;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine("capture");
            //image.SetActive(true);

        }
    }

    IEnumerator capture()
    {
        yield return new WaitForEndOfFrame();

        byte[] imgBytes;
        string path = @"C:\Users\HP\Desktop\ScreenShot.png";

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture.Apply();

        imgBytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, imgBytes);

        Debug.Log(path + "¿˙¿Â");

    }
}