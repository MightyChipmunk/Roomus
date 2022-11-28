using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTG
{
    public class RuntimeGizmo : MonoBehaviour
    {
        MoveGizmo moveGizmo;
        RotationGizmo rotationGizmo;
        ObjectTransformGizmo objectTransformGizmo;
        public GameObject targetObject;
        public bool isMove;
        bool isRotate;
        public bool isActive;
        //GameObject RTGApp;

        private void Awake()
        {
            //RTGApp = GameObject.Find("RTGApp");
        }
        void Start()
        {
            
        }

        private void Update()
        {
            if (isMove)
            {
                print("ismove is true");
                GizmoMoveON();
                isMove = false;
            }

        }

        public void GizmoRotateON()
        {
            //RTGApp.SetActive(true);

            if (objectTransformGizmo != null) objectTransformGizmo.Gizmo.SetEnabled(false);
            objectTransformGizmo = RTGizmosEngine.Get.CreateObjectRotationGizmo();
            //GameObject targetObject = GameObject.FindWithTag("Light");
            objectTransformGizmo.SetTargetObject(targetObject);
            isActive = true;
            //rotationGizmo = objectTransformGizmo.Gizmo.RotationGizmo;
            //moveGizmo.SetVertexSnapTargetObjects(new List<GameObject> { targetObject });

            //objectTransformGizmo.SetTargetObject(targetObject);
        }

        public void GizmoMoveON()
        {
            //RTGApp.SetActive(true);

            if (objectTransformGizmo != null) objectTransformGizmo.Gizmo.SetEnabled(false);
            objectTransformGizmo = RTGizmosEngine.Get.CreateObjectMoveGizmo();
            //GameObject targetObject = GameObject.FindWithTag("Light");
            objectTransformGizmo.SetTargetObject(targetObject);
            isActive = true;
            //moveGizmo = objectTransformGizmo.Gizmo.MoveGizmo;

            //moveGizmo.SetVertexSnapTargetObjects(new List<GameObject> { targetObject });

            //objectTransformGizmo.SetTargetObject(targetObject);
        }

        public void GizmoOff()
        {
            objectTransformGizmo.Gizmo.SetEnabled(false);
            isActive = false;
            //RTGApp.SetActive(false);
        }
    }
}

