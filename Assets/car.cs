using UnityEngine;
using System.Collections;

public class car : MonoBehaviour {

    public WheelCollider[] wheels = new WheelCollider[4];
    public float torque;

    private void FixedUpdate()
    {
        float steer = Input.GetAxis("Horizontal");
        float acell = Input.GetAxis("Vertical");

        float finalSteer = steer * 45f;

        for(int i = 0; i <2; i++)
        {
            wheels[i].motorTorque = acell * torque;
            if (i < 2)
            {
                wheels[i].steerAngle = finalSteer;
            }
        }
    }
}
