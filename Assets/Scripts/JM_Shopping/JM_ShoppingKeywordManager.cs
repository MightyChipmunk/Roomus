using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_ShoppingKeywordManager : MonoBehaviour
{
    public GameObject shopping_Keyword;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shopping_Keyword.GetComponent<Text>().text = "search: " + GameObject.Find("KeyWord").GetComponent<Text>().text;
    }
}
