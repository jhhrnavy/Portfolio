using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapon/Gun"), Serializable]
public class Gun : Weapon
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public float bulletSpeed;

    [Space,Header("Gun")]
    public int magazineSize;
    public int reserveAmmo; // ÀÜ¿© ÃÑ¾Ë
    public float fireRate, spread;
    public float reloadTime = 0.5f;
    public bool allowsAutoShot;

    public override void Fire(Vector3 firePosition, Vector3 targetPosition)
    {

        Vector3 direction = targetPosition - firePosition;
        direction.y = 0;

        // Calculate Spread
        Vector3 spread = CalculateSpreadRange();
        direction += spread;

        var bullet = Instantiate(bulletPrefab, firePosition, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
    }

    private Vector3 CalculateSpreadRange()
    {
        float x = UnityEngine.Random.Range(-spread, spread);
        float z = UnityEngine.Random.Range(-spread, spread);

        return new Vector3(x, 0, z);
    }
}

