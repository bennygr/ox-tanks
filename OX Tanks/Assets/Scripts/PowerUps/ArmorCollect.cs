using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorCollect : AbstractCollect {

	private const float HP = 25;

	protected override void heal (TankVitals tankVitals) {
		tankVitals.healArmor (HP);
	}
}