using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_ChangeView : MonoBehaviour
{
    Transform room;
    Transform thirdCamPos;

    public static Deco_ChangeView Instance { get; private set; }

    public enum ViewState
    {
        Second_Demen,
        Third_Demen,
        First,
        Third_First
    }
    [SerializeField]
    ViewState _viewState;
    public ViewState viewState
    {
        get { return _viewState; }
        set { _viewState = value; }
    }

    public Vector3 FirstPos;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        viewState = ViewState.Third_Demen;

        room = GameObject.Find("Room").transform;
        thirdCamPos = GameObject.Find("ThirdCamPos").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void On2DClicked()
    {
        StopAllCoroutines();
        viewState = ViewState.Second_Demen;
    }

    public void On3DClicked()
    {
        StopAllCoroutines();
        viewState = ViewState.Third_Demen;
    }

    public void OnFirstClicked()
    {
        StopAllCoroutines();
        if (viewState == ViewState.First)
            return;
        else if (viewState == ViewState.Second_Demen)
        {
            viewState = ViewState.Third_Demen;
            thirdCamPos.eulerAngles = new Vector3(90, 0, 0);
            thirdCamPos.position = new Vector3(room.position.x, 15.0f, room.position.z);
        }
        else if (viewState == ViewState.Third_Demen)
        {
            thirdCamPos.eulerAngles = new Vector3(90, 0, 0);
            thirdCamPos.position = new Vector3(room.position.x, 15.0f, room.position.z);
            StartCoroutine("OnPosClick");
        }
    }

    IEnumerator OnPosClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 터치한 곳으로 Ray를 쏴서 그곳으로 배치
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 30f, LayerMask.GetMask("Floor"))) 
                {
                    FirstPos = hit.point;
                    break;
                }
            }
            yield return null;
        }
        viewState = ViewState.Third_First;
        while (Vector3.Distance(Camera.main.transform.position, FirstPos + Vector3.up * 1.7f) > 0.1f)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, FirstPos + Vector3.up * 1.7f, Time.deltaTime * 8.0f);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.identity, Time.deltaTime * 8.0f);
            yield return null;  
        }
        viewState = ViewState.First;
    }
}
