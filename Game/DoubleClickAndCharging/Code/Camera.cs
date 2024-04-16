using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    GameObject _target;

    [SerializeField]
    Vector3 _offSet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.transform.position + _offSet;
        transform.LookAt(_target.transform);
    }
}
