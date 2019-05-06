using System;

public interface PowerUpCollectable {

    /// <summary>
    /// Applies the specified power up to the specified TankVitals instance
    /// </summary>
    /// <param name="tankVitals">The Tank vitals instance</param>
    void applyPowerUp(TankVitals tankVitals);
}