using System;
using UnityEngine;

[Serializable()]
public class Agent 
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    [Range(0f, 1f)]   
    [SerializeField] private float madness;

    public void SetHealth(int newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, maxHealth);
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetMadness(float newMadness)
    {
        madness = Mathf.Clamp01(newMadness);
    }

    public float GetMadness()
    {
        return madness;
    }
}

