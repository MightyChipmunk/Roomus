using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SignInManager : MonoBehaviour
{
    // ID
    public InputField idInput;
    // PW
    public InputField pwInput;

    string id;
    string pw;

    // Start is called before the first frame update
    void Start()
    {
        idInput.onEndEdit.AddListener(UpdateID);
        pwInput.onEndEdit.AddListener(UpdatePW);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateID(string input)
    {
        id = input;
    }

    void UpdatePW(string input)
    {
        pw = input;
    }

    public void OnSignInClick()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnSignUpClick()
    {
        SceneManager.LoadScene("SignUp");
    }

    
    
}
