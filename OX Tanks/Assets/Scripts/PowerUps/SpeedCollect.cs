using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCollect : AbstractCollect {

    private const int speedFactorTime = 10;
    private const int speedFactor = 2;

    public override void applyPowerUp(TankVitals tankVitals) {
        tankVitals.setSpeedMultiplier(speedFactor, speedFactorTime);
    }
}