using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSteady : MonoBehaviour
{
    [SerializeField]
    private float PositionFixSpeed;
    [SerializeField]
    private float RotationFixSpeed;

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * PositionFixSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * RotationFixSpeed);
    }
}
