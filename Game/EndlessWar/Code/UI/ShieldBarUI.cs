using UnityEngine;
using UnityEngine.UI;

public class ShieldBarUI : MonoBehaviour
{
    public int shield, maxShield;

    [SerializeField]
    private Slider _shieldBar;

    public void SetInit(int maxShield)
    {

        this.maxShield = maxShield;
        _shieldBar.maxValue = maxShield;
        _shieldBar.value = maxShield;
    }

    public void SetShield(int shield)
    {
        this.shield = shield;
        _shieldBar.value = this.shield;
    }

}
