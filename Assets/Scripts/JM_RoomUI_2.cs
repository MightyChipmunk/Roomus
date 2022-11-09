using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_RoomUI_2 : MonoBehaviour
{
    public bool isCommentShow;
    [SerializeField]
    bool isMove;
    public bool isGoBack;
    public Transform movePos;
    public Vector3 originalPos;

    public GameObject first;
    JM_RoomUI firstCode;

    public GameObject comment;
    JM_RoomUI_3 commentCode;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        firstCode = first.GetComponent<JM_RoomUI>();
        commentCode = comment.GetComponent<JM_RoomUI_3>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(isMove);
        //print(Vector3.Distance(transform.position, movePos.position));
        if (isMove)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, movePos.localPosition, Time.deltaTime * 4);
            commentCode.isCommentShow = true;
            if (Vector3.Distance(transform.localPosition, movePos.localPosition) < 0.5f)
            {
                transform.localPosition = movePos.localPosition;
                isMove = false;
            }
        }
        if (isGoBack)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * 4);
            commentCode.isCommentShow = false;
            if (Vector3.Distance(transform.localPosition, originalPos) < 0.5f)
            {
                transform.localPosition = originalPos;
                isGoBack = false;
            }
        }
        
    }

    public void OnClickComment()
    {
        if (!commentCode.isCommentShow)
        {
            isMove = true;
        }
        if (commentCode.isCommentShow)
        {
            isGoBack = true;
        }
    }
}
