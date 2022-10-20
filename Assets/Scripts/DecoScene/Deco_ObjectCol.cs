using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_ObjectCol : MonoBehaviour
{
    bool isCollide = false;
    public bool IsCollide
    {
        get { return isCollide; }
        private set { isCollide = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Floor"))
        {
            IsCollide = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Floor"))
            IsCollide = false;
    }
}
