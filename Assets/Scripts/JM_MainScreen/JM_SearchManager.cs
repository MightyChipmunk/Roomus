using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class JM_SearchManager : MonoBehaviour
{

    public InputField searchInput;
    public GameObject keyWordObj;
    string searchKeyWord;

    // Start is called before the first frame update
    void Start()
    {
        searchInput.onEndEdit.AddListener(UpdateSearchText);
        searchInput.onSubmit.AddListener(Search);

    }

    // Update is called once per frame
    void Update()
    {
    }

    void UpdateSearchText(string input)
    {
        searchKeyWord = input;
    }

    void Search(string input)
    {
        searchKeyWord = input;
        SceneManager.LoadScene("Shopping");
        //GameObject.Find("Shopping_SearchWord").GetComponent<Text>().text = "Searching word is " + searchKeyWord;
        keyWordObj.GetComponent<Text>().text = searchKeyWord;
        DontDestroyOnLoad(keyWordObj);

    }
}
