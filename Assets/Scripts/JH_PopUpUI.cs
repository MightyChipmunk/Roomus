using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JH_PopUpUI : MonoBehaviour
{
    Text title;
    Text desc;
    Button button;

    GameObject loadingImage;
    GameObject rotateImage;

    GameObject putLoadingImage;
    GameObject putRotateImage;

    string sceneName;

    public static JH_PopUpUI Instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);

        DontDestroyOnLoad(gameObject);

        title = transform.Find("PopUpUI").transform.Find("Title").GetComponent<Text>();
        desc = transform.Find("PopUpUI").transform.Find("Description").GetComponent<Text>();
        button = transform.Find("PopUpUI").transform.Find("Button").GetComponent<Button>();

        loadingImage = transform.Find("Loading").gameObject;
        rotateImage = transform.Find("Loading").transform.Find("RotateImage").gameObject;

        putLoadingImage = transform.Find("PutLoading").gameObject;
        putRotateImage = transform.Find("PutLoading").transform.Find("RotateImage").gameObject;

        button.onClick.AddListener(OnClick);

        transform.Find("PopUpUI").transform.localScale = Vector3.zero;

        loadingImage.SetActive(false);
        putLoadingImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        rotateImage.transform.Rotate(0, 0, 15 * Time.deltaTime);
        putRotateImage.transform.Rotate(0, 0, 15 * Time.deltaTime);
    }

    public void PutLoadingUIUp()
    {
        putLoadingImage.SetActive(true);
    }

    public void PutLoadingUIDown()
    {
        putLoadingImage.SetActive(false);
    }

    public void LoadingUIUp()
    {
        loadingImage.SetActive(true);
    }

    public void LoadingUIDown()
    {
        loadingImage.SetActive(false);
    }

    public void SetUI(string title, string desc, bool auto = true, float time = 0.5f, string sceneName = "")
    {
        this.title.text = title;
        this.desc.text = desc;
        this.sceneName = sceneName;
        iTween.ScaleTo(transform.Find("PopUpUI").gameObject, iTween.Hash("x", 1, "y", 1, "time", time, "easetype", iTween.EaseType.easeOutQuint));
        if (auto)
        {
            iTween.ScaleTo(transform.Find("PopUpUI").gameObject, iTween.Hash("x", 0, "y", 0, "time", time, "delay", time * 2, "easetype", iTween.EaseType.easeOutQuint));
        }
    }

    void OnClick()
    {
        iTween.ScaleTo(transform.Find("PopUpUI").gameObject, iTween.Hash("x", 0, "y", 0, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        if (sceneName.Length > 0)
        {
            SceneManager.LoadScene(sceneName); 
        }
    }
}
