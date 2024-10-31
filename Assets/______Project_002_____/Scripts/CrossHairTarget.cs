using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class CrossHairTarget : MonoBehaviour
    {
        Ray ray;
        RaycastHit hitInfo;
        Camera mainCamera;
        private float range = 100f;
        public LayerMask aimingLayer;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            ray.origin = mainCamera.transform.position;
            ray.direction = mainCamera.transform.forward;
            Physics.Raycast(ray, out hitInfo, range, aimingLayer);
            transform.position = hitInfo.point; 
        }
    }
}
