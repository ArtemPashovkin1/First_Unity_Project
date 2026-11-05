using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Конфигурация")]
    [SerializeField] private HealthConfig config;

    public float CurrentHealth { get; private set; }
    public float CurrentHunger { get; private set; }
    public float CurrentThirst { get; private set; }
    public float CurrentEnergy { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnHungerChanged;
    public event Action<float, float> OnThirstChanged;
    public event Action<float, float> OnEnergyChanged;
    public event Action OnDeath;

    void Awake()
    {
        if (config == null)
        {
            Debug.LogError($"HealthConfig не назначен на объекте {gameObject.name}");
            return;
        }

        CurrentHealth = config.MaxHealth;
        CurrentHunger = config.MaxHunger;
        CurrentThirst = config.MaxThirst;
        CurrentEnergy = config.MaxEnergy;
    }

    private void Update()
    {
        if (config == null) return;

        DecreaseHunger(config.HungerDecayPerSec * Time.deltaTime);
        DecreaseThirst(config.ThirstDecayPerSec * Time.deltaTime);
        DecreaseEnergy(config.EnergyDecayPerSec * Time.deltaTime);

        if (CurrentHunger <= 0f || CurrentThirst <= 0f)
            TakeDamage(config.HealthDecayPerSec * Time.deltaTime);
        else if (CurrentHunger > config.MaxHunger * 0.5f && CurrentThirst > config.MaxThirst * 0.5f)
            Heal(config.HealthRegenPerSec * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        ChangeHealth(-damage);
    }

    public void Heal(float amount)
    {
        ChangeHealth(amount);
    }

    private void ChangeHealth(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, config.MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, config.MaxHealth);

        if (CurrentHealth <= 0)
            Die();
    }

    public void IncreaseHunger(float amount)
    {
        ChangeHunger(amount);
    }

    public void DecreaseHunger(float amount)
    {
        ChangeHunger(-amount);
    }

    private void ChangeHunger(float amount)
    {
        CurrentHunger = Mathf.Clamp(CurrentHunger + amount, 0, config.MaxHunger);
        OnHungerChanged?.Invoke(CurrentHunger, config.MaxHunger);
    }

    public void IncreaseThirst(float amount)
    {
        ChangeThirst(amount);
    }

    public void DecreaseThirst(float amount)
    {
        ChangeThirst(-amount);
    }

    private void ChangeThirst(float amount)
    {
        CurrentThirst = Mathf.Clamp(CurrentThirst + amount, 0, config.MaxThirst);
        OnThirstChanged?.Invoke(CurrentThirst, config.MaxThirst);
    }

    public void IncreaseEnergy(float amount)
    {
        ChangeEnergy(amount);
    }

    public void DecreaseEnergy(float amount)
    {
        ChangeEnergy(-amount);
    }

    private void ChangeEnergy(float amount)
    {
        CurrentEnergy = Mathf.Clamp(CurrentEnergy + amount, 0, config.MaxEnergy);
        OnEnergyChanged?.Invoke(CurrentEnergy, config.MaxEnergy);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Debug.Log($"{gameObject.name} умер");
    }
}
