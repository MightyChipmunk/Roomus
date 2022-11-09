using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_RoomUI_3 : MonoBehaviour
{
    public bool isCommentShow;
    public Transform commentMovePos;
    Vector3 originCommentPos;

    // Start is called before the first frame update
    void Start()
    {
        originCommentPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCommentShow)
        {
            transform.position = Vector3.Lerp(transform.position, commentMovePos.position, Time.deltaTime * 4);
            if (Vector3.Distance(transform.position, commentMovePos.position) < 0.5f)
            {
                transform.position = commentMovePos.position;
            }
        }
        else if (!isCommentShow)
        {
            transform.position = Vector3.Lerp(transform.position, originCommentPos, Time.deltaTime * 4);
            if (Vector3.Distance(transform.position, originCommentPos) < 0.5f)
            {
                transform.position = originCommentPos;
            }
        }
    }
}
