using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A simple tank movement script to controll the tank. It should
/// be applied to the <code>PlayerRig</code> prefab.
/// </summary>
public class TankMovement : MonoBehaviour
{

    // Initial moving speed
    [SerializeField]
    private float movingSpeed = 5f;
    // Initial turning speed
    [SerializeField]
    private float turningSpeed = 100f;
    // Initial speed multiplier (in case a speed buff is applied)
    [SerializeField]
    private float speedMultiplier = 1f;

    //The tank vitals
    private TankVitals vitals;

    // The rigid body of the tank
    private Rigidbody rigidBody;

    private const string verticalAxisName = "Vertical Player {0}";
    private const string horizontalAxisName = "Horizontal Player {0}";

    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        vitals = GetComponent<TankVitals>();
    }

    /// <summary>
    /// Since movement is applied at the rigid body with the physics engine,
    /// the calculates must happen in fixed update.
    /// </summary>
    void FixedUpdate()
    {
        if (!PauseScreenHandler.IsPaused)
        {
            Move();
            Turn();
        }
    }


    /// <summary>
    /// Moves the tank to the new position dictated by the
    /// amount of movement registered from the vertical input axes
    /// of the controller (e.g. Keyboard: up/down)
    /// </summary>
    private void Move() {
        string axis = string.Format(verticalAxisName, vitals.PlayerNumber);
        float vertical = Input.GetAxis(axis);
        Vector3 movement = transform.forward * vertical * movingSpeed * speedMultiplier * Time.deltaTime;
        rigidBody.MovePosition(rigidBody.position + movement);
    }

    /// <summary>
    /// Turns/rotates the tank to a new angle dicated by the
    /// amount of the rotation registered from the horizontal input axes
    /// of the controller. (e.g. Keyboard: left/right)
    /// </summary>
    private void Turn() {
        string axis = string.Format(horizontalAxisName, vitals.PlayerNumber);
        float horizontal = Input.GetAxis(axis);
        float turn = horizontal * turningSpeed * speedMultiplier * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
        rigidBody.MoveRotation(rigidBody.rotation * turnRotation);
    }

    /// <summary>
    /// Sets the speed multiplier.
    /// </summary>
    /// <param name="speedMultiplier">Speed multiplier.</param>
    public void setSpeedMultiplier(float speedMultiplier) {
        this.speedMultiplier = speedMultiplier;
    }
}
