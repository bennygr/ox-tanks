﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractSkill : MonoBehaviour {

    public const float DEFAULT_COOLDOWN = 2;
    public const int MAX_DAMAGE = 100;

    [SerializeField]
    protected GameObject prefab;

    [SerializeField]
    private Image skillCooldown;

    // Identify the different players
    public int playerNumber = 1;
    // Skill cooldown
    public float cooldown = 0;
    protected String fireButton;
    public Transform skillTransform;
    public LinkedList<Transform> skillTransforms = new LinkedList<Transform>();

    [SerializeField]
    private AudioSource audioSource;
    public AudioClip fireClip;

    private DateTime lastTriggered;

    protected void Start() {
        playerNumber = GetComponent<TankVitals>().PlayerNumber;
        fireButton = "Skill Player " + playerNumber;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.clip = prefab.GetComponent<AbstractExplosion>().FireClip;

        postStart();
    }

    void LateUpdate() {
        if (CanTrigger()) {
            GetComponent<TankVitals>().SkillCooldown.enabled = true;
        }
    }

    protected void Triggered() {
        audioSource.Play();
        GetComponent<TankVitals>().SkillCooldown.enabled = false;
        lastTriggered = DateTime.Now;
    }

    protected bool CanTrigger() {
        if (PauseScreenHandler.IsPaused) return false;
        if (cooldown == 0) {
            return true;
        }
        return (DateTime.Now - lastTriggered).Seconds > cooldown;
    }

    public void setSkillTransform(Transform skillTransform) {
        this.skillTransform = skillTransform;
    }

    public void addSkillTransform(Transform skillTransform) {
        this.skillTransforms.AddFirst(skillTransform);
    }

    public Image SkillCooldown {
        get {
            return skillCooldown;
        }

        set {
            skillCooldown = value;
        }
    }

    /// <summary>
    /// Perform any post start-up tasks
    /// </summary>
    protected abstract void postStart();
}
