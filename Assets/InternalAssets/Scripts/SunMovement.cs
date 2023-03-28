using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float rotationSpeed;
    void FixedUpdate() => HandleSunMovement();

    void HandleSunMovement() => transform.Rotate(Vector3.up * rotationSpeed, Space.World);
}
