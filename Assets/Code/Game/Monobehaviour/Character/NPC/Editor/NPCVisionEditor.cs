using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NPCVision))]
public class NPCVisionEditor : Editor
{
    private void OnSceneGUI()
    {
        NPCVision visionDector = (NPCVision)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(visionDector.transform.position, Vector3.up, Vector3.forward, 360, visionDector.ViewRadius);
        Vector3 viewAngleA = visionDector.DirectionFromAngle(-visionDector.ViewAngle / 2, false);
        Vector3 viewAngleB = visionDector.DirectionFromAngle(visionDector.ViewAngle / 2, false);
        Handles.DrawLine(visionDector.transform.position, visionDector.transform.position + viewAngleA * visionDector.ViewRadius);
        Handles.DrawLine(visionDector.transform.position, visionDector.transform.position + viewAngleB * visionDector.ViewRadius);
        Handles.color = Color.red;
    }
}
