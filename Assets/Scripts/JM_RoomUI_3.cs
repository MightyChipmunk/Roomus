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
        originCommentPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCommentShow)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, commentMovePos.localPosition, Time.deltaTime * 4);
            if (Vector3.Distance(transform.localPosition, commentMovePos.localPosition) < 0.5f)
            {
                transform.localPosition = commentMovePos.localPosition;
            }
        }
        else if (!isCommentShow)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originCommentPos, Time.deltaTime * 4);
            if (Vector3.Distance(transform.localPosition, originCommentPos) < 0.5f)
            {
                transform.localPosition = originCommentPos;
            }
        }
    }
}
