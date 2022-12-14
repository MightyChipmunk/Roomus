using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JM_RoomUI : MonoBehaviour //IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 originPos;
    public Transform infoPos;
    bool isMove;
    public bool isGoBack;
    bool isInfoShowing;
    bool isInfoGoBack;
    public GameObject info;
    public GameObject comment;
    JM_RoomUI_3 commentCode;
    public GameObject firstAndSecond;
    JM_RoomUI_2 firstAndSecondCode;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.localPosition;
        isInfoShowing = false;
        commentCode = comment.GetComponent<JM_RoomUI_3>();
        firstAndSecondCode = firstAndSecond.GetComponent<JM_RoomUI_2>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (isMove)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, infoPos.localPosition, Time.deltaTime * 4);
            isInfoShowing = true;

            if (Vector3.Distance(transform.localPosition, infoPos.localPosition) < 0.5f)
            {
                transform.localPosition = infoPos.localPosition;
                isMove = false;
            }
        }
        
        

        if (isGoBack)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, Time.deltaTime * 4);
            isInfoShowing = false;

            // comment session goes down if it is showing
            // info goes back to original place as well
            //if (commentCode.isCommentShow)
            //{
                //firstAndSecondCode.isGoBack = true;
                //commentCode.isCommentShow = false;
                //isInfoGoBack = true;
            //}

            if (Vector3.Distance(transform.localPosition, originPos) < 0.5f)
            {
                transform.localPosition = originPos;
                isGoBack = false;
            }
        }

        



    }

    public void OnClickMoreInfo()
    {
        if (!isInfoShowing)
        {
            isMove = true;
            isGoBack = false;
        }
        if (isInfoShowing)
        {
            isGoBack = true;
            isMove = false;
        }
        
    }

    /*

    float pivot;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        pivot = Input.mousePosition.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float moveValue = Input.mousePosition.y - pivot;
        moveValue = Mathf.Clamp(moveValue, 0, 360);
        print(moveValue);
        transform.localPosition = originPos + new Vector3(0, moveValue, 0);

        
        float verticalVal = Input.GetAxis("Mouse Y");
        print("vertical " + verticalVal);
        transform.localPosition += verticalVal * 20 * transform.up * Time.deltaTime;
        


        print("2");
        
        if (isMove)
        {
            Vector3 mousePos = new Vector3(originPos.x, Input.mousePosition.y, 0);
            Vector3 objPos = Camera.main.ScreenToViewportPoint(mousePos);
            transform.localPosition = mousePos;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    { 
        print("3");
        isGoBack = true;
    }

*/

}
