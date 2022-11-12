using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_LibSearchManager : MonoBehaviour
{
    public InputField searchInput;
    string searchKeyWord;

    // Start is called before the first frame update
    void Start()
    {
        searchInput.onSubmit.AddListener(SubmitSearchKeyword);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SubmitSearchKeyword(string input)
    {
        searchKeyWord = input;

        // test
        print(searchKeyWord);

    }

    void Refresh()
    {
        searchInput.Select();
        searchInput.text = "";
    }
}
