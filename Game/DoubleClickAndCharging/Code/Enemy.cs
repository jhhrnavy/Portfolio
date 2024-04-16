using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Reference

    private GameObject _target;
    [SerializeField] private GameObject _explosionEffect;

    #endregion

    #region Fields
    [Header("[ 이동 속도 ]"), SerializeField]
    float _speed = 3f;
    [Header("[ 회전 속도 ]"), SerializeField]
    float _rotateSpeed = 5f;
    [Header("[ 체력 ]"), SerializeField]
    int _hp = 10;

    #endregion

    #region Start and Update

    void Start()
    {
        _target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    #endregion

    #region Custom Functions

    public void GetDamaged(int damage)
    {
        _hp -= damage;
        ShowVfx();

        if (_hp <= 0)
        {
            ++_target.GetComponent<PlayerCtrl>().DestroyEnemyCount;
            ShowVfx();

            // 플레이어가 죽여아 할 남은 마리수 감소
            GameManager.Instance.ChangeLeftEnemyCount(1);
            Destroy(this.gameObject);
        }
    }

    private void Move()
    {
        Vector3 dir = _target.transform.position - transform.position;
        transform.position += dir.normalized * _speed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _rotateSpeed);
    }

    // Dead VFX
    private void ShowVfx()
    {
        var explosionVfx = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosionVfx, 0.5f);
    }

    #endregion

    #region Collider

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            GetDamaged(5);
            ShowVfx();
            Destroy(other.gameObject);
        }
    }

    #endregion
}
