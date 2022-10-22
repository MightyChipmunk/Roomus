using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Deco_UIManager : MonoBehaviour
{
    public static Deco_UIManager Instance;

    GameObject library;
    public GameObject furnitItem;
    RectTransform trContent;

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
        trContent = (RectTransform)library.transform.Find("Viewport").transform.Find("Content");

        foreach (GameObject go in Deco_Json.Instance.objects.datas)
        {
            AddContent(go.GetComponent<Deco_Idx>().Idx);
        }

        library.SetActive(false);
    }

    public void OnLoadLibrary()
    {
        if (library.activeSelf)
            library.SetActive(false);
        else
            library.SetActive(true);    
    }

    void AddContent(int id)
    {
        GameObject item = Instantiate(furnitItem, trContent);
        item.name = id.ToString();
        item.GetComponentInChildren<Text>().text = id.ToString();
    }
}
