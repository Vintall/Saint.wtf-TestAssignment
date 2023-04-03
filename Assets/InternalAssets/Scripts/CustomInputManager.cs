using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputManager : MonoBehaviour
{
    private static CustomInputManager instance;
    public static CustomInputManager Instance => instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    [SerializeField] Joystick movementJoystick;
    public static float HorizontalMovementAxis => instance.movementJoystick.Horizontal;
    public static float VerticalMovementAxis => instance.movementJoystick.Vertical;
}
