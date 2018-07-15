using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollect : AbstractCollect {

	private const float HP = 25;

	protected override void heal (TankVitals tankVitals) {
		tankVitals.healHP (HP);
	}
}