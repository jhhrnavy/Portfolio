using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public int healAmount = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
