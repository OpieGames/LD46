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
        HorizontalRotate.LookAt(piz.position);
    }
}
