using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownControl : MonoBehaviour {
    [SerializeField]
    private Image image;

    private bool isCooldown;

    private float cooldown;

    public float Cooldown {
        get {
            return cooldown;
        }

        set {
            cooldown = value;
        }
    }

    public bool IsCooldown {
        get {
            return isCooldown;
        }

        set {
            isCooldown = value;
        }
    }

    void Update() {
        if (IsCooldown) {
            image.fillAmount += 1f / Cooldown * Time.deltaTime;
        }

        if (image.fillAmount >= 1) {
            image.fillAmount = 0;
            IsCooldown = false;
        }
    }
}
