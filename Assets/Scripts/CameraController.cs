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
        // ���콺 ��ġ�� ���� ���� ����
        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");

        if (Input.GetKey(KeyCode.Z))
        {
            mx += mh * rotSpeed * Time.deltaTime;
            my += mv * rotSpeed * Time.deltaTime;
            my = Mathf.Clamp(my, -60, 60);
        }
            
        transform.eulerAngles = new Vector3(-my, mx, 0);

        transform.position = player.transform.position; 
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
