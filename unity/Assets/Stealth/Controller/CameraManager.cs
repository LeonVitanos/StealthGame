using System.Collections;
using UnityEngine;
using Stealth.Objects;
using UnityEditor;
using UnityEngine.UI;
using System;

namespace Stealth
{
    /// <summary>
    /// Manages the <see cref="GalleryCamera"/>s in the scene.
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        private Text cameraText;

        [SerializeField]
        private int _disabledLimit = -1;

        public static int DisabledCamerasAmount
        {
            get
            {
                int amount = 0;
                foreach (GalleryCamera camera in cameras)
                {
                    if (camera.Disabled) amount++;
                }
                return amount;
            }
        }

        public int DisabledLimit
        {
            get => _disabledLimit;
            set => _disabledLimit = value;
        }

        private static GalleryCamera[] cameras;

        private void Awake()
        {
            cameras = FindObjectsOfType<GalleryCamera>();
            cameraText.text = $"Deactivated Cameras: {DisabledCamerasAmount} / {_disabledLimit}";
        }

        private void OnEnable()
        {
            foreach (GalleryCamera camera in cameras)
            {
                camera.CameraClicked += OnCameraDisabledChanged;
            }
        }

        private void OnDisable()
        {
            foreach (GalleryCamera camera in cameras)
            {
                camera.CameraClicked -= OnCameraDisabledChanged;
            }
        }

        private void OnCameraDisabledChanged(GalleryCamera cam)
        {
            cameraText.text = $"Deactivated Cameras: {DisabledCamerasAmount} / {_disabledLimit}";
        }

        //[MenuItem("Game/Update vision all cameras _F10")]
        public static void UpdateVisionCameras()
        {
            if (!Application.isPlaying)
            {
                cameras = FindObjectsOfType<GalleryCamera>();
            }
            foreach (GalleryCamera cam in cameras)
            {
                cam.ComputeVisionArea();
            }
        }

        public static bool IsPointVisible(Vector2 point)
        {
            foreach (GalleryCamera camera in cameras)
            {
                if (camera.IsPointVisible(point))
                {
                    return true;
                }
            }
            return false;
        }

        public static void EnableAllCameras()
        {
            foreach (GalleryCamera camera in cameras)
            {
                camera.Disabled = false;
            }
        }
    }
}