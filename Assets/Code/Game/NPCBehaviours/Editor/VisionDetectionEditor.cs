using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Game
{
    [CustomEditor (typeof (VisionDetection))]
	public class VisionDetectionEditor : Editor 
	{
        private void OnSceneGUI()
        {
            VisionDetection visionDector = (VisionDetection)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(visionDector.transform.position, Vector3.up, Vector3.forward, 360, visionDector.ViewRadius);
            Vector3 viewAngleA = visionDector.DirectionFromAngle(-visionDector.ViewAngle / 2, false);
            Vector3 viewAngleB = visionDector.DirectionFromAngle(visionDector.ViewAngle / 2, false);
            Handles.DrawLine(visionDector.transform.position, visionDector.transform.position + viewAngleA * visionDector.ViewRadius);
            Handles.DrawLine(visionDector.transform.position, visionDector.transform.position + viewAngleB * visionDector.ViewRadius);
            Handles.color = Color.red;

            foreach (Transform visibleTarget in visionDector.VisibleTargets)
            {
                Handles.DrawLine(visionDector.transform.position, visibleTarget.position);
            }
        }
    }
}
