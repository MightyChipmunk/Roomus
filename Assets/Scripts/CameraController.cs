using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CameraController : MonoBehaviourPun
{
    [SerializeField]
    float rotSpeed = 100f;

    Deco_Idx furnitInfo;
    GameObject player;
    GameObject infoUI;

    bool thirdPers = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find(PhotonNetwork.NickName);
        if (!photonView.IsMine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        infoUI = GameObject.Find("Canvas").transform.Find("FurnitInfo").gameObject;
    }
    float mx;
    float my;
    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            thirdPers = !thirdPers;
        }

        // 마우스 위치에 따른 각도 조절
        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");

        if (Input.GetKey(KeyCode.Z))
        {
            mx += mh * rotSpeed * Time.deltaTime;
            my += mv * rotSpeed * Time.deltaTime;
            my = Mathf.Clamp(my, -60, 60);
        }

        transform.eulerAngles = new Vector3(-my, mx, 0);
        transform.position = Vector3.Lerp(transform.position, player.transform.position + Vector3.up * 1.5f, Time.deltaTime * 15);

        if (thirdPers)
        {
            transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, new Vector3(0, -0.25f, -1.5f), Time.deltaTime * 5);
            Camera.main.cullingMask = -1;
        }
        else
        {
            transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, new Vector3(0, 0, 0), Time.deltaTime * 5);
            Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Player"));
        }
    }

    Vector3 UIPos;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 15) && hit.transform.parent != null && Input.GetMouseButtonDown(0))
        {
            if (hit.transform.parent.TryGetComponent<Deco_Idx>(out furnitInfo))
            {
                infoUI.gameObject.SetActive(true);
                infoUI.transform.position = Input.mousePosition + Vector3.down * 150;
                UIPos = hit.transform.position;
                infoUI.transform.Find("FurnitName").GetComponent<Text>().text = furnitInfo.Name;
                infoUI.transform.Find("Price").GetComponent<Text>().text = furnitInfo.Price.ToString();
                infoUI.transform.Find("Category").GetComponent<Text>().text = furnitInfo.Category;
            }
        }
        else if (hit.transform != null && Input.GetMouseButtonDown(0))
            infoUI.gameObject.SetActive(false);

        if (infoUI.gameObject.activeSelf && Vector3.Angle(UIPos - Camera.main.transform.position, Camera.main.transform.forward) > 30f)
        {
            infoUI.gameObject.SetActive(false);
        }
    }
}
