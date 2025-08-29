using UnityEngine;

public interface IDamageable
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }

    void DamageTaken(float damageAmount);
}
