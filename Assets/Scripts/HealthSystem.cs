using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;
    public float health;
    private float maxHealth;

    public HealthSystem(float startHP)
    {
        this.maxHealth = startHP;
        health = maxHealth;

    }
    public float GetHealth()
    {
        return health;
    }

    public float GetHealthPercent()
    {
        return health / maxHealth;
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0;
            OnDead?.Invoke(this, EventArgs.Empty);
        }
        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        if (health > maxHealth) health = maxHealth;
        OnHealed?.Invoke(this, EventArgs.Empty);
        //if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);
    }
}
