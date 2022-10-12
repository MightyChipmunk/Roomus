using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_ChangeView : MonoBehaviour
{
    public static Deco_ChangeView Instance { get; private set; }

    public enum ViewState
    {
        Second_Demen,
        Third_Demen,
        First,
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log(touch.position);
                // 터치한 곳으로 Ray를 쏴서 그곳으로 배치
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, LayerMask.NameToLayer("Floor")))
                {
                    FirstPos = hit.transform.position;
                }
            }
        }
        
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
            Camera.main.transform.eulerAngles = new Vector3(90, 0, 0);
            transform.position = new Vector3(transform.position.x, 15.0f, transform.position.z);
        }
        else if (viewState == ViewState.Third_Demen)
            StartCoroutine("OnPosClick");
    }

    IEnumerator OnPosClick()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    Debug.Log(touch.position);
                    // 터치한 곳으로 Ray를 쏴서 그곳으로 배치
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 30f, LayerMask.NameToLayer("Floor"))) 
                    {
                        FirstPos = hit.transform.position;
                    }
                    viewState = ViewState.First;
                    break;
                }
            }
            yield return null;
        }
    }
}
