using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextControl : MonoBehaviour {

	private FloatingText damageHP;
	private FloatingText damageShield;
	private FloatingText healHP;
	private FloatingText healShield;

	void Awake () {
		damageHP = Resources.Load<FloatingText> ("Prefabs/UI/PlayerInformation/DamageHPTextRootNode");
		damageShield = Resources.Load<FloatingText> ("Prefabs/UI/PlayerInformation/DamageShieldTextRootNode");
		healHP = Resources.Load<FloatingText> ("Prefabs/UI/PlayerInformation/HealHPTextRootNode");
		healShield = Resources.Load<FloatingText> ("Prefabs/UI/PlayerInformation/HealShieldTextRootNode");
	}

	public void spawnDamageHPFloatingText (string text, Transform location) {
		instantiateFloatingText (damageHP, text, location);
	}

	public void spawnDamageShieldFloatingText (string text, Transform location) {
		instantiateFloatingText (damageShield, text, location);
	}

	public void spawnHealHPFloatingText (string text, Transform location) {
		instantiateFloatingText (healHP, text, location);
	}

	public void spawnHealShieldFloatingText (string text, Transform location) {
		instantiateFloatingText (healShield, text, location);
	}

	private void instantiateFloatingText (FloatingText floatingText, string text, Transform location) {
		FloatingText instance = Instantiate (floatingText);
		instance.transform.SetParent (transform, false);
		instance.setText (text);
	}
}