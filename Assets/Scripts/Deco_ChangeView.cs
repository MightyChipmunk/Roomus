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
        
    }

    public void On2DClicked()
    {
        viewState = ViewState.Second_Demen;
    }

    public void On3DClicked()
    {
        viewState = ViewState.Third_Demen;
    }

    public void OnFirstClicked()
    {
        if (viewState == ViewState.First)
            return;
        else if (viewState == ViewState.Second_Demen)
            viewState = ViewState.Third_Demen;
        else if (viewState == ViewState.Third_Demen)
            StartCoroutine("OnPosClick");
    }

    IEnumerator OnPosClick()
    {
        int touchCount = Input.touchCount;
        while (true)
        {
            if (touchCount < Input.touchCount)
            {
                Touch touch = Input.GetTouch(touchCount - 1);
                if (touch.phase == TouchPhase.Ended)
                {
                    // 터치한 곳으로 Ray를 쏴서 그곳으로 배치
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 30f, LayerMask.NameToLayer("Floor"))) 
                    {
                        FirstPos = hit.point + Vector3.up * 1.5f;
                    }
                    viewState = ViewState.First;
                    break;
                }
            }
            yield return null;
        }
    }
}
