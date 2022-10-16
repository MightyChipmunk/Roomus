using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;
    Animator anim;
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float rotSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>(); 
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = h * Camera.main.transform.right + v * Camera.main.transform.forward;
        dir.y = 0;
        dir.Normalize();

        if (dir.magnitude > 0.1f)
        {
            anim.SetBool("IsMove", true);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
        }
        else
            anim.SetBool("IsMove", false);

        cc.Move(speed * dir * Time.deltaTime);
    }
}
