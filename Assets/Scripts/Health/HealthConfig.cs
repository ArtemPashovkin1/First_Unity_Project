using UnityEngine;

[CreateAssetMenu(fileName = "HealthConfig", menuName = "Survival/Health Config")]
public class HealthConfig : ScriptableObject
{
    [Header("Здоровье")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Голод")]
    [SerializeField] private float maxHunger = 100f;

    [Header("Жажда")]
    [SerializeField] private float maxThirst = 100f;

    [Header("Энергия")]
    [SerializeField] private float maxEnergy = 100f;

    [Header("Скорости падения в секунду")]
    [SerializeField] private float hungerDecayPerSec = 0.5f;
    [SerializeField] private float thirstDecayPerSec = 0.8f;
    [SerializeField] private float energyDecayPerSec = 0.3f;
    [SerializeField] private float healthDecayPerSec = 1.5f;
    [SerializeField] private float healthRegenPerSec = 0.5f;

    public float MaxHealth => maxHealth;
    public float MaxHunger => maxHunger;
    public float MaxThirst => maxThirst;
    public float MaxEnergy => maxEnergy;
    public float HungerDecayPerSec => hungerDecayPerSec;
    public float ThirstDecayPerSec => thirstDecayPerSec;
    public float EnergyDecayPerSec => energyDecayPerSec;
    public float HealthDecayPerSec => healthDecayPerSec;
    public float HealthRegenPerSec => healthRegenPerSec;
}
