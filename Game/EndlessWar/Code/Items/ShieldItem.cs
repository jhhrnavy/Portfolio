using UnityEngine;

public class ShieldItem : MonoBehaviour
{
    public int shieldAmount = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GetShield(shieldAmount);
            Destroy(gameObject);
        }
    }
}
