using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSkill : MonoBehaviour {

    public const float DEFAULT_COOLDOWN = 2;
    public const int MAX_DAMAGE = 100;

    // Identify the different players
    public int playerNumber = 1;
    // Skill cooldown
    public float cooldown = 0;
    protected String fireButton;
    public Transform skillTransform;
    public LinkedList<Transform> skillTransforms = new LinkedList<Transform>();

    private DateTime lastTriggered;

    protected void Start() {
        playerNumber = GetComponent<TankVitals>().PlayerNumber;
        fireButton = "Skill Player " + playerNumber;
        postStart();
    }

    protected void Triggered() {
        lastTriggered = DateTime.Now;
    }

    protected bool CanTrigger() {
        if(PauseScreenHandler.IsPaused) return false;
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

    /// <summary>
    /// Perform any post start-up tasks
    /// </summary>
    protected abstract void postStart();
}
