using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JM_LightUIManager : MonoBehaviour
{
    public GameObject lightSelectionUI;
    public GameObject sptLightUI;
    public GameObject ptLightUI;
    public GameObject sptlightLibUI;
    public GameObject ptlightLibUI;

    public Transform lightSelectionUIShowPos;
    public Transform lightSelectionUIOriginPos;
    public Transform lightUIShowPos;
    public Transform lightUIOriginPos;
    public Transform lightLibShowPos;
    public Transform lightLibOriginPos;


    bool isLightSelectionUIShow;
    bool isLightSelectionUIMove;

    [SerializeField]
    bool isSptLightUIShow;
    bool isSptLightUIMove;

    [SerializeField]
    bool isPtLightUIShow;
    bool isPtLightUIMove;

    bool isSptLibUIMove;
    bool isSptLibUIShow;

    bool isPtLibUIMove;
    bool isPtLibUIShow;

    // Start is called before the first frame update
    void Start()
    {
        lightSelectionUI.transform.position = lightSelectionUIOriginPos.position;
        sptLightUI.transform.position = lightUIOriginPos.position;
        ptLightUI.transform.position = lightUIOriginPos.position;
        sptlightLibUI.transform.position = lightLibOriginPos.position;
        ptlightLibUI.transform.position = lightLibOriginPos.position;
        lightSelectionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLightSelectionUIMove)
        {
            if (isLightSelectionUIShow)
            {
                lightSelectionUI.SetActive(true);
                lightSelectionUI.transform.position = Vector3.Lerp(lightSelectionUI.transform.position, lightSelectionUIShowPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(lightSelectionUI.transform.position, lightSelectionUIShowPos.position) < 0.5f)
                {
                    lightSelectionUI.transform.position = lightSelectionUIShowPos.position;
                    isLightSelectionUIMove = false;
                }
            }
            if (!isLightSelectionUIShow)
            {
                lightSelectionUI.transform.position = Vector3.Lerp(lightSelectionUI.transform.position, lightSelectionUIOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(lightSelectionUI.transform.position, lightSelectionUIOriginPos.position) < 0.5f)
                {
                    lightSelectionUI.transform.position = lightSelectionUIOriginPos.position;
                    lightSelectionUI.SetActive(false);
                    isLightSelectionUIMove = false;
                }
            }
        }

        if (isSptLightUIMove)
        {
            if (isSptLightUIShow)
            {
                sptLightUI.transform.position = Vector3.Lerp(sptLightUI.transform.position, lightUIShowPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(sptLightUI.transform.position, lightUIShowPos.position) < 0.5f)
                {
                    sptLightUI.transform.position = lightUIShowPos.position;
                    isSptLightUIMove = false;
                }
            }
            if (!isSptLightUIShow)
            {
                sptLightUI.transform.position = Vector3.Lerp(sptLightUI.transform.position, lightUIOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(sptLightUI.transform.position, lightUIOriginPos.position) < 0.5f)
                {
                    sptLightUI.transform.position = lightUIOriginPos.position;
                    isSptLightUIMove = false;
                }
            }
        }
        if (isPtLightUIMove)
        {
            if (isPtLightUIShow)
            {
                ptLightUI.transform.position = Vector3.Lerp(ptLightUI.transform.position, lightUIShowPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(ptLightUI.transform.position, lightUIShowPos.position) < 0.5f)
                {
                    ptLightUI.transform.position = lightUIShowPos.position;
                    isPtLightUIMove = false;
                }
            }
            if (!isPtLightUIShow)
            {
                ptLightUI.transform.position = Vector3.Lerp(ptLightUI.transform.position, lightUIOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(ptLightUI.transform.position, lightUIOriginPos.position) < 0.5f)
                {
                    ptLightUI.transform.position = lightUIOriginPos.position;
                    isPtLightUIMove = false;
                }
            }
        }
        if (isSptLibUIMove)
        {
            if (isSptLibUIShow)
            {
                sptlightLibUI.transform.position = Vector3.Lerp(sptlightLibUI.transform.position, lightLibShowPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(sptlightLibUI.transform.position, lightLibShowPos.position) < 0.5f)
                {
                    sptlightLibUI.transform.position = lightLibShowPos.position;
                    isSptLibUIMove = false;
                }
            }
            if (!isSptLibUIShow)
            {
                sptlightLibUI.transform.position = Vector3.Lerp(sptlightLibUI.transform.position, lightLibOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(sptlightLibUI.transform.position, lightLibOriginPos.position) < 0.5f)
                {
                    sptlightLibUI.transform.position = lightLibOriginPos.position;
                    isSptLibUIMove = false;
                }
            }
        }
        if (isPtLibUIMove)
        {
            if (isPtLibUIShow)
            {
                ptlightLibUI.transform.position = Vector3.Lerp(ptlightLibUI.transform.position, lightLibShowPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(ptlightLibUI.transform.position, lightLibShowPos.position) < 0.5f)
                {
                    ptlightLibUI.transform.position = lightLibShowPos.position;
                    isPtLibUIMove = false;
                }
            }
            if (!isPtLibUIShow)
            {
                ptlightLibUI.transform.position = Vector3.Lerp(ptlightLibUI.transform.position, lightLibOriginPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(ptlightLibUI.transform.position, lightLibOriginPos.position) < 0.5f)
                {
                    ptlightLibUI.transform.position = lightLibOriginPos.position;
                    isPtLibUIMove = false;
                }
            }
        }
        

    }

    public void OnClickGenLighting()
    {
        isLightSelectionUIMove = true;
        isLightSelectionUIShow = true;
    }

    public void OnClickSptLight()
    {
        isSptLightUIMove = true;
        isSptLightUIShow = true;
        if (isPtLightUIShow)
        {
            isPtLightUIMove = true;
            isPtLightUIShow = false;
        }
        if (isPtLibUIShow)
        {
            isPtLibUIMove = true;
            isPtLibUIShow = false;
        }
    }
    public void OnClickPtLight()
    {
        isPtLightUIMove = true;
        isPtLightUIShow = true;
        if (isSptLightUIShow)
        {
            isSptLightUIMove = true;
            isSptLightUIShow = false;
        }
        if (isSptLibUIShow)
        {
            isSptLibUIMove = true;
            isSptLibUIShow = false;
        }
    }

    public void OnClickLightLib()
    {
        if (isSptLightUIShow)
        {
            isSptLibUIMove = true;
            if (sptlightLibUI.transform.position == lightLibOriginPos.position)
                isSptLibUIShow = true;
            else
                isSptLibUIShow = false;
        }
        if (isPtLightUIShow)
        {
            isPtLibUIMove = true;
            if (ptlightLibUI.transform.position == lightLibOriginPos.position)
                isPtLibUIShow = true;
            else
                isPtLibUIShow = false;
        }
    }


    public void OnClickSptDone()
    {
        isLightSelectionUIMove = true;
        isLightSelectionUIShow = false;
        isSptLightUIMove = true;
        isSptLightUIShow = false;
        isSptLibUIMove = true;
        isSptLibUIShow = false;
        JM_LibraryManager.instance.isLibraryMove = true;
        JM_LibraryManager.instance.isLibShow = true;
    }

    public void OnClickPtDone()
    {
        isLightSelectionUIMove = true;
        isLightSelectionUIShow = false;
        isPtLightUIMove = true;
        isPtLightUIShow = false;
        isPtLibUIMove = true;
        isPtLibUIShow = false;
        JM_LibraryManager.instance.isLibraryMove = true;
        JM_LibraryManager.instance.isLibShow = true;
    }
}
