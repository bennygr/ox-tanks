using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollect : AbstractCollect {

    private const int HP = 25;

    public override void applyPowerUp(TankVitals tankVitals) {
        tankVitals.healHP(HP);
    }
}