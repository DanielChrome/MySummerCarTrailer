using UnityEngine;
using System.Collections;

public class CarretinhaScript : MonoBehaviour {

    public WheelCollider[] wheelColliders = new WheelCollider[2];
    public Transform[] tireMeshes = new Transform[2];
    public Transform centerOfMass;
    private Rigidbody ridgbody;

    private void Start()
    {
        ridgbody = GetComponent<Rigidbody>();
        ridgbody.centerOfMass = centerOfMass.localPosition;
    }

    private void Update()
    {
        UpdateWhellsRotation();
    }

    private void UpdateWhellsRotation()
    {
        for(int i = 0; i < 2; i++)
        {
            Quaternion quat;
            Vector3 pos;
            wheelColliders[i].GetWorldPose(out pos, out quat);
            tireMeshes[i].position = pos;
            tireMeshes[i].rotation = quat;
        }
    }
}
