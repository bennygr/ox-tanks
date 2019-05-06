using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollect : AbstractCollect {

    private const int damageFactorTime = 10;
    private const int damageFactor = 2;

    public override void applyPowerUp(TankVitals tankVitals) {
        tankVitals.setDamageMultiplier(damageFactor, damageFactorTime);
    }
}