using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_UIManager : MonoBehaviour
{
    public static Deco_UIManager Instance;

    GameObject library;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        library = GameObject.Find("Library");
        library.SetActive(false);
    }

    public void OnLoadLibrary()
    {
        if (library.activeSelf)
            library.SetActive(false);
        else
            library.SetActive(true);    
    }
}
