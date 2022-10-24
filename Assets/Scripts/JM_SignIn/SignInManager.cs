using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInManager : MonoBehaviour
{
    // ID
    public InputField id;
    // PW
    public InputField pw;

    string idInput;
    string pwInput;

    // Start is called before the first frame update
    void Start()
    {
        id.onSubmit.AddListener(SubmitID);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void typeID(string input)
    {
        idInput = input;
    }

    void SubmitID(string idInput)
    {

    }

    void SubmitPW(string pwInput)
    {

    }
}
