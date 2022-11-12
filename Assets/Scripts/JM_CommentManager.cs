using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_CommentManager : MonoBehaviour
{
    public InputField commentInput;
    string comment;
    public GameObject commentContent;
    public GameObject commentSubmitUI;

    // Start is called before the first frame update
    void Start()
    {
        commentInput.onSubmit.AddListener(SubmitComment);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SubmitComment(string input)
    {
        comment = input;
        GameObject obj = Instantiate(commentSubmitUI, commentContent.transform);
        obj.transform.Find("Text").GetComponent<Text>().text = input;
        Refresh();
    }

    void Refresh()
    {
        commentInput.Select();
        commentInput.text = "";
    }
}
