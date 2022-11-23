using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_MyRoomAnalysisManager : MonoBehaviour
{

    public GameObject bg;
    float test;
    bool isExtend;
    bool isSizeChange;
    bool isTxtMove;
    float extendHeight;
    float height;
    float originHeight;
    public GameObject txt;
    public Transform txtMovePos;
    public Transform txtOriginPos;

    public GameObject likes;
    public Transform likesMovePos;
    public Transform likesOriginPos;

    public GameObject picks;
    public Transform picksMovePos;
    public Transform picksOriginPos;

    float currentTime;

    bool isGraphChange;
    bool isGraphP1;
    bool isGraphP2;

    public GameObject first;
    public GameObject second;
    public GameObject third;
    public GameObject fourth;
    public GameObject fifth;
    [SerializeField]
    List<float> likesList = new List<float>();
    [SerializeField]
    List<float> valList = new List<float>();

    public List<GameObject> graphList = new List<GameObject>();
    public List<GameObject> p2Txt = new List<GameObject>();

    float firstLikes;
    float secondLikes;
    float thirdLikes;
    float fourthLikes;
    float fifthLikes;
    float sixthLikes;
    float seventhLikes;
    float eighthLikes;
    float ninethLikes;
    float tenthLikes;

    float firstVal;
    float secondVal;
    float thirdVal;
    float fourthVal;
    float fifthVal;



    // Start is called before the first frame update
    void Start()
    {       
        originHeight = 465;
        height = originHeight;
        extendHeight = 980;

        CheckTopLikes();

        for (int i = 0; i < p2Txt.Count; i++)
        {
            p2Txt[i].SetActive(false);
        }

        for (int i = 0; i < graphList.Count; i++)
        {
            graphList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(graphList[i].GetComponent<RectTransform>().sizeDelta.x, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isSizeChange || isTxtMove)
        {
            if (isExtend)
            {
                /*
                txt.transform.position = Vector3.Lerp(txt.transform.position, txtMovePos.position, Time.deltaTime * 4);
                if (Vector3.Distance(txt.transform.position, txtMovePos.position) < 0.5f)
                {
                    txt.transform.position = txtMovePos.position;
                    isTxtMove = false;
                }
                */
                
                height = Mathf.Lerp(height, extendHeight, Time.deltaTime * 4);
                if (extendHeight - height < 0.5f)
                {
                    height = extendHeight;
                    isSizeChange = false;
                }

                likes.transform.position = Vector3.Lerp(likes.transform.position, likesMovePos.position, Time.deltaTime * 4);
                if (Vector3.Distance(likes.transform.position, likesMovePos.position) < 0.5f)
                {
                    likes.transform.position = likesMovePos.position;
                }

                picks.transform.position = Vector3.Lerp(picks.transform.position, picksMovePos.position, Time.deltaTime * 4);
                if (Vector3.Distance(picks.transform.position, picksMovePos.position) < 0.5f)
                {
                    picks.transform.position = picksMovePos.position;
                }
            }
            else
            {
                /*
                txt.transform.position = Vector3.Lerp(txt.transform.position, txtOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(txt.transform.position, txtOriginPos.position) < 0.5f)
                {
                    txt.transform.position = txtOriginPos.position;
                    isTxtMove = false;
                }
                */
                height = Mathf.Lerp(height, originHeight, Time.deltaTime * 4);
                if (height - originHeight <= 0.5f)
                {
                    height = originHeight;
                    isSizeChange = false;
                }

                likes.transform.position = Vector3.Lerp(likes.transform.position, likesOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(likes.transform.position, likesOriginPos.position) < 0.5f)
                {
                    likes.transform.position = likesOriginPos.position;
                }

                picks.transform.position = Vector3.Lerp(picks.transform.position, picksOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(picks.transform.position, picksOriginPos.position) < 0.5f)
                {
                    picks.transform.position = picksOriginPos.position;
                }
            }
            
        }
        bg.GetComponent<RectTransform>().sizeDelta = new Vector2(bg.GetComponent<RectTransform>().sizeDelta.x, height);

        if (!isGraphP1) currentTime += Time.deltaTime;
        if (currentTime > 1)
        {
            isGraphP1 = true;
            for (int i = 0; i < 5; i++)
            {
                p2Txt[i].SetActive(true);
            }
            currentTime = 0;
        }

        if (isGraphP1)
        {
            for (int i = 0; i < 5; i++)
            {
                valList[i] = Mathf.Lerp(valList[i], likesList[i], Time.deltaTime * 4f);
                if (likesList[i] - valList[i] < 0.5f)
                {
                    valList[i] = likesList[i];
                }
                graphList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(first.GetComponent<RectTransform>().sizeDelta.x, valList[i]);
            }
            /*
            firstVal = Mathf.Lerp(firstVal, firstLikes, Time.deltaTime);
            if (firstLikes - firstVal < 0.5f)
            {
                firstVal = firstLikes;
            }
            secondVal = Mathf.Lerp(secondVal, secondLikes, Time.deltaTime);
            if (secondLikes - secondVal < 0.5f)
            {
                secondVal = secondLikes;
            }
            */
        }

        if (isGraphChange)
        {
            if (isGraphP2)
            {
                for (int i = 0; i < p2Txt.Count; i++)
                {                   
                    p2Txt[i].SetActive(true);
                }

                for (int i = 5; i < valList.Count; i++)
                {
                    valList[i] = Mathf.Lerp(valList[i], likesList[i], Time.deltaTime * 4f);
                    if (likesList[i] - valList[i] < 1f)
                    {
                        valList[i] = likesList[i];
                    }
                    graphList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(first.GetComponent<RectTransform>().sizeDelta.x, valList[i]);
                }
            }
            else
            {
                for (int i = 0; i < p2Txt.Count; i++)
                {
                    p2Txt[i].SetActive(false);
                }

                for (int i = 5; i < valList.Count; i++)
                {
                    valList[i] = Mathf.Lerp(valList[i], 0, Time.deltaTime * 4f);
                    if (valList[i] < 1f)
                    {
                        valList[i] = 0;
                    }
                    graphList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(first.GetComponent<RectTransform>().sizeDelta.x, valList[i]);
                }

            }

        }


        //first.GetComponent<RectTransform>().sizeDelta = new Vector2(first.GetComponent<RectTransform>().sizeDelta.x, firstVal);


        //second.GetComponent<RectTransform>().sizeDelta = new Vector2(second.GetComponent<RectTransform>().sizeDelta.x, secondVal);

    }

    public void OnClickRoomAnalysis()
    {
        if (height == originHeight)
        {
            isExtend = true;
        }
        if (height == extendHeight)
        {
            isExtend = false;
        }
        isSizeChange = true;
        isTxtMove = true;
        isGraphChange = true;
        if (isExtend) isGraphP2 = true;
        if (!isExtend) isGraphP2 = false;
    }

    public void CheckTopLikes()
    {
        // test
        firstLikes = 197;
        secondLikes = 180;
        thirdLikes = 152;
        fourthLikes = 115;
        fifthLikes = 98;
        sixthLikes = 74;
        seventhLikes = 42;
        eighthLikes = 41;
        ninethLikes = 39;
        tenthLikes = 27;

        likesList.Add(firstLikes);
        likesList.Add(secondLikes);
        likesList.Add(thirdLikes);
        likesList.Add(fourthLikes);
        likesList.Add(fifthLikes);
        likesList.Add(sixthLikes);
        likesList.Add(seventhLikes);
        likesList.Add(eighthLikes);
        likesList.Add(ninethLikes);
        likesList.Add(tenthLikes);

        for (int i = 0; i < 10; i++)
        {
            valList.Add(0);
        }        
    }
}
