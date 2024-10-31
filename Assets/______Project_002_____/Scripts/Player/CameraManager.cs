using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; set; }

        public CinemachineVirtualCamera tpsCamera;
        public float TargetFOV { get; set; } = 60.0f;
        public float zoomSpeed = 5.0f;



        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        private void LateUpdate()
        {
            tpsCamera.m_Lens.FieldOfView = Mathf.Lerp(tpsCamera.m_Lens.FieldOfView, TargetFOV, zoomSpeed * Time.deltaTime);
        }
    }
}