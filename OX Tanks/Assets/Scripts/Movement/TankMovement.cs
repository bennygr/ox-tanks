using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A simple tank movement script to controll the tank. It should
/// be applied to the <code>PlayerRig</code> prefab.
/// </summary>
public class TankMovement : MonoBehaviour {

    // Initial moving speed
    [SerializeField]
    private float movingSpeed = 5f;
    // Initial turning speed
    [SerializeField]
    private float turningSpeed = 100f;
    // Initial speed multiplier (in case a speed buff is applied)
    [SerializeField]
    private float speedMultiplier = 1f;

    [SerializeField]
    private AudioSource audioSource;

    public AudioSource AudioSource {
        get {
            return audioSource;
        }
    }

    [SerializeField]
    private AudioClip idle;

    [SerializeField]
    private AudioClip drive;

    //The tank vitals
    private TankVitals vitals;

    // The rigid body of the tank
    private Rigidbody rigidBody;

    private float originalPitch = 1f;
    private float pitchRange = 0.2f;

    private const string verticalAxisName = "Vertical Player {0}";
    private const string horizontalAxisName = "Horizontal Player {0}";

    private float vertical;
    private float horizontal;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        vitals = GetComponent<TankVitals>();
    }

    void Update() {
        string axisV = string.Format(verticalAxisName, vitals.PlayerNumber);
        string axisH = string.Format(horizontalAxisName, vitals.PlayerNumber);
        vertical = Input.GetAxis(axisV);
        horizontal = Input.GetAxis(axisH);
        playMovementAudio();
    }

    /// <summary>
    /// Since movement is applied at the rigid body with the physics engine,
    /// the calculates must happen in fixed update.
    /// </summary>
    void FixedUpdate() {
        if (!PauseScreenHandler.IsPaused) {
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
        Vector3 movement = transform.forward * vertical * movingSpeed * speedMultiplier * Time.deltaTime;
        rigidBody.MovePosition(rigidBody.position + movement);
    }

    /// <summary>
    /// Turns/rotates the tank to a new angle dicated by the
    /// amount of the rotation registered from the horizontal input axes
    /// of the controller. (e.g. Keyboard: left/right)
    /// </summary>
    private void Turn() {
        float turn = horizontal * turningSpeed * speedMultiplier * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
        rigidBody.MoveRotation(rigidBody.rotation * turnRotation);
    }

    /// <summary>
    /// Playes the movement audio.
    /// </summary>
    private void playMovementAudio() {
        if (isMoving()) {
            if (audioSource.clip == idle) {
                audioSource.clip = drive;
                audioSource.pitch = UnityEngine.Random.Range((originalPitch - pitchRange), (originalPitch + pitchRange)) * speedMultiplier;
                audioSource.Play();
            }
        } else {
            if (audioSource.clip == drive) {
                audioSource.clip = idle;
                audioSource.pitch = UnityEngine.Random.Range((originalPitch - pitchRange), (originalPitch + pitchRange)) * speedMultiplier;
                audioSource.Play();
            }
        }
    }

    private bool isMoving() {
        return Math.Abs(vertical) > 0.1f || Math.Abs(horizontal) > 0.1f;
    }

    /// <summary>
    /// Sets the speed multiplier.
    /// </summary>
    /// <param name="speedMultiplier">Speed multiplier.</param>
    public void setSpeedMultiplier(float speedMultiplier) {
        this.speedMultiplier = speedMultiplier;
    }
}
