using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorCollect : AbstractCollect {

	private const int HP = 25;

    public override void applyPowerUp(TankVitals tankVitals) {
        tankVitals.healArmor(HP);
    }
}