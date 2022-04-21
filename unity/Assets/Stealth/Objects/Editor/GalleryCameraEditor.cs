using Stealth.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Stealth
{
    [CustomEditor(typeof(GalleryCamera))]
    public class GalleryCameraEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GalleryCamera cam = (GalleryCamera)target;

            string buttonText = (cam.Vision == null || !cam.Vision.ComputationInProgress) ? "Compute stepwise" : "Advance computation";
            if (GUILayout.Button(buttonText))
            {
                if (cam.Vision == null)
                {
                    cam.ComputeVisionAreaStepwise();
                }
                else
                {
                    cam.AdvanceStepwiseComputation();
                }
            }
        }
    }
}