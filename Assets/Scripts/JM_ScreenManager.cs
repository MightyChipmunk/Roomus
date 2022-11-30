using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_ScreenManager : MonoBehaviour
{
    public static JM_ScreenManager instance;

    public GameObject screen;
    public bool isDark;
    public bool isStart;
    public float alpha;

    Color screenColor;

    public GameObject toolBar;

    public bool isSceneChange;
    public bool isTabChange;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        isDark = true;
        isStart = true;
        alpha = 1;
    }

    float currentTime;

    // Update is called once per frame
    void Update()
    {
        // if darkening / brightening starts
        if (isStart)
        {
            toolBar.SetActive(false);

            if (isDark)
            {
                alpha -= Time.deltaTime;
                alpha = Mathf.Clamp(alpha, 0, 1);
                screenColor.a = alpha;
                screen.GetComponent<Image>().color = screenColor;
                if (alpha <= 0)
                {
                    //isDark = false;
                    isStart = false;
                    screen.SetActive(false);
                    toolBar.SetActive(true);
                }
            }
            else if (!isDark)
            {
                alpha += Time.deltaTime;
                alpha = Mathf.Clamp(alpha, 0, 1);
                screenColor.a = alpha;
                screen.GetComponent<Image>().color = screenColor;
                if (alpha >= 1)
                {
                    if (isTabChange)
                    {
                        Deco_UIManager.Instance.ChangeTabToPost();
                        currentTime += Time.deltaTime;
                        if (currentTime >= 1)
                        {
                            isDark = true;
                            currentTime = 0;
                            isTabChange = false;
                        }
                    }
                    else
                    {
                        isStart = false;
                        isSceneChange = true;
                    }

                    //isTabChange = true;
                }
            }
           
            /*
            else
            {
                alpha += Time.deltaTime;
                alpha = Mathf.Clamp(alpha, 0, 1);
                screenColor.a = alpha;
                screen.GetComponent<Image>().color = screenColor;
                if (alpha >= 1)
                {
                    isDark = true;
                    //screen.SetActive(false);
                }


            }
            */
        }                       
    }

    public void Darken()
    {
        isStart = true;
        isDark = false;
        screen.SetActive(true);
    }

    public void Brighten()
    {
        isStart = true;
        screen.SetActive(true);
    }

    public void TabChange()
    {
        isTabChange = true;
    }

}
