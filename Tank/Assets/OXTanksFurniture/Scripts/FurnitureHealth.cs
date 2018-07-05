using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureHealth : AbstractGameHealth {

    private void Awake()
    {
        base.Init();
    }

    public void TakeDamage(float amount)
    {
        base.ApplyDamage(amount);
        base.CalculateDeath();
    }

    private void OnDeath()
    {
        base.ApplyDeath();
    }

    private void OnEnable()
    {
        base.Enable();
    }

}
