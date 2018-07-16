using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class TransformControl : NetworkBehaviour {
    [SerializeField]
    private float moveRate;
    public float MoveRate
    {
        get
        {

            return moveRate;
        }

        set
        {
            moveRate = value;
        }
    }
    private Vector3 axisInput;
    public Vector3 AxisInput
    {
        get
        {
            axisInput.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Zedical"), Input.GetAxis("Vertical"));
            return axisInput;
        }

        private set
        {
            axisInput = value;
        }
    }

    private void Update()
    {
        transform.Translate(AxisInput * Time.deltaTime * MoveRate);
    }
}
