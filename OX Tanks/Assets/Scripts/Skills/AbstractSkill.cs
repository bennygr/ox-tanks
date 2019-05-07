using System;
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

    private DateTime lastTriggered;

    protected void Start() {
        fireButton = "Skill" + playerNumber;
        postStart();
    }

    protected void Triggered() {
        lastTriggered = DateTime.Now;
    }

    protected bool CanTrigger() {
        if (cooldown == 0) {
            return true;
        }
        return (DateTime.Now - lastTriggered).Seconds > cooldown;
    }

    public void setSkillTransform(Transform skillTransform) {
        this.skillTransform = skillTransform;
    }

    /// <summary>
    /// Perform any post start-up tasks
    /// </summary>
    protected abstract void postStart();
}
