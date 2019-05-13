using System;
using UnityEngine;

public class EasterEggManager : MonoBehaviour {

    [SerializeField]
    private Light light;

    void Start() {
        if (light == null) {
            Debug.Log("No light source assigned. Easter egg is disableds");
        }
    }

    public void toggleDarkMode() {
        light.enabled = !light.enabled;
    }
}
