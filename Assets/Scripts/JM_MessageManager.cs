using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_MessageManager : MonoBehaviour
{

    public InputField msgInput;
    public string message;
    string str;

    // Start is called before the first frame update
    void Start()
    {
        str = "";
        msgInput.onSubmit.AddListener(UpdateMessage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMessage(string input)
    {
        message = input;
        print(message);
        Refresh();
    }

    void Refresh()
    {
        msgInput.Select();
        msgInput.text = "";
    }
}
