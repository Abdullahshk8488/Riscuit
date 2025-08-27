using UnityEngine;

public interface IDamagable
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }

    void DamageTaken(float damageAmount);
}
