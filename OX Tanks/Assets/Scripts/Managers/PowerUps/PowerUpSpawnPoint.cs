using System;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpSpawnPoint : MonoBehaviour {

    private bool hasPowerUp = false;

    public bool HasPowerUp {
        get {
            return hasPowerUp;
        }

        set {
            hasPowerUp = value;
        }
    }
}
