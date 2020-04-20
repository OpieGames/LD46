using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonLook : MonoBehaviour
{
        public Transform HorizontalRotate;
        public Transform VerticalRotate;

        private Transform piz;
    // Start is called before the first frame update
    void Start()
    {
        piz = GameObject.FindGameObjectWithTag("Pizza").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (piz.position - HorizontalRotate.position).magnitude;
        if (gameObject.GetComponent<Tower>().Range > distance)
        {
            HorizontalRotate.LookAt(piz);
            HorizontalRotate.rotation = Quaternion.Euler( new Vector3(0, HorizontalRotate.rotation.eulerAngles.y, 0) );

            VerticalRotate.LookAt(piz);
            // VerticalRotate.rotation = Quaternion.Euler ( new Vector3(VerticalRotate.rotation.eulerAngles.x, 0, 0));
            // VerticalRotate.rotation = Quaternion.Euler( new Vector3(eulerAngles.x, 0, 0) );
        }
        
    }
}
