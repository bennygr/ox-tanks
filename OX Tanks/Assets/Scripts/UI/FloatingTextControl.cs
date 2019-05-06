using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextControl : MonoBehaviour {

    private FloatingText damageHP;
    private FloatingText damageShield;
    private FloatingText healHP;
    private FloatingText healShield;
    private FloatingText damageMultiplier;

    void Awake() {
        damageHP = Resources.Load<FloatingText>("Prefabs/UI/PlayerInformation/DamageHPTextRootNode");
        damageShield = Resources.Load<FloatingText>("Prefabs/UI/PlayerInformation/DamageShieldTextRootNode");
        healHP = Resources.Load<FloatingText>("Prefabs/UI/PlayerInformation/HealHPTextRootNode");
        healShield = Resources.Load<FloatingText>("Prefabs/UI/PlayerInformation/HealShieldTextRootNode");
        damageMultiplier = Resources.Load<FloatingText>("Prefabs/UI/PlayerInformation/DamageMultiplierTextRootNode");
    }

    public void spawnDamageMultiplierFloatingText(string text) {
        instantiateFloatingText(damageMultiplier, text);
    }

    public void spawnDamageHPFloatingText(string text) {
        instantiateFloatingText(damageHP, text);
    }

    public void spawnDamageShieldFloatingText(string text) {
        instantiateFloatingText(damageShield, text);
    }

    public void spawnHealHPFloatingText(string text) {
        instantiateFloatingText(healHP, text);
    }

    public void spawnHealShieldFloatingText(string text) {
        instantiateFloatingText(healShield, text);
    }

    private void instantiateFloatingText(FloatingText floatingText, string text) {
        FloatingText instance = Instantiate(floatingText);
        float x = instance.transform.position.x;
        float y = instance.transform.position.y;
        float z = instance.transform.position.z;
        instance.transform.position = new Vector3(x + Random.Range(-0.5f, 0.5f), y + Random.Range(-0.5f, 0.5f), z + Random.Range(-0.5f, 0.5f));
        instance.transform.SetParent(transform, false);
        instance.setText(text);
    }
}