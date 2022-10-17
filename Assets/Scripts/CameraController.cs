using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 100f;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }
    float mx;
    float my;
    // Update is called once per frame
    void LateUpdate()
    {
        // ���콺 ��ġ�� ���� ���� ����
        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");
        mx += mh * rotSpeed * Time.deltaTime;
        my += mv * rotSpeed * Time.deltaTime;
        my = Mathf.Clamp(my, -60, 60);
        transform.eulerAngles = new Vector3(-my, mx, 0);

        transform.position = player.transform.position; 
    }
}