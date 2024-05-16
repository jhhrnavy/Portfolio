using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public int health, maxHealth;

    [SerializeField]
    private Slider _healthBar;

    public void SetInit(int maxHealth)
    {
        this.maxHealth = maxHealth;
        _healthBar.maxValue = maxHealth;
        _healthBar.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        this.health = health;
        _healthBar.value = this.health;
    }
}
