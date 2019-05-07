using System;
using UnityEngine;

public class MineExplosion : AbstractExplosion {

    //Activate the mine after n seconds
    private float activeAfter = 1f;

    //The time in seconds the mine exists
    private float timeExists;

    private bool active = false;

    private void Update() {
        timeExists += Time.deltaTime;
        active = timeExists > activeAfter;
    }

    protected new void OnTriggerEnter(Collider other) {
        if (active) {
            base.OnTriggerEnter(other);
        }
    }
}
