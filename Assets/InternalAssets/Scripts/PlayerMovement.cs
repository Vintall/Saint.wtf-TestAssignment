using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField, Range(0.5f, 2f)] float movingSpeed = 0.5f;

    void FixedUpdate() => HandleMovement(CustomInputManager.HorizontalMovementAxis, CustomInputManager.VerticalMovementAxis);
    void HandleMovement(float x_offset, float y_offset)
    {
        if (!rb)
            Debug.LogError("PlayerMovement does hot have the rigidbody reference");
        
        rb.AddForce(new Vector3(x_offset, 0, y_offset) * movingSpeed, ForceMode.Impulse);

        transform.LookAt(new Vector3(x_offset, 0, y_offset) + transform.position);
    }
}
