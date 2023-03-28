using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField, Range(5f, 20f)] float movingSpeed = 5f;

    void FixedUpdate() => HandleMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    void HandleMovement(float x_offset, float y_offset)
    {
        if (!rb)
            Debug.LogError("PlayerMovement does hot have the rigidbody reference");

        rb.AddForce(new Vector3(x_offset, 0, y_offset), ForceMode.Impulse);
    }
}
