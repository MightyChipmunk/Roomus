using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Deco_GetXYZ : MonoBehaviour
{
    public static Deco_GetXYZ Instance;

    public InputField xField;
    public InputField yField;
    public InputField zField;

    float x = 10;
    float y = 10;
    float z = 5;
    public float X { get { return x; } }
    public float Y { get { return y; } }
    public float Z { get { return z; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        xField.onValueChanged.AddListener(GetX);
        yField.onValueChanged.AddListener(GetY);
        zField.onValueChanged.AddListener(GetZ);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("RoomDecoScene");
    }

    void GetX(string s)
    {
        x = Int32.Parse(s);
    }

    void GetY(string s)
    {
        y = Int32.Parse(s);
    }

    void GetZ(string s)
    {
        z = Int32.Parse(s);
    }
}
