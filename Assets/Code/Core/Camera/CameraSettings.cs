using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Camera Settings")]
public class CameraSettings : ScriptableObject
{
    public Vector3 offset = new Vector3(0f, 0f, 0f);
    public Vector3 tiltAngle = new Vector3(0f, 0f, 0f);
}
