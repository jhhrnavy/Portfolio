using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;

    private void Start()
    {
        Camera.main.transform.position = _offset;
        Camera.main.fieldOfView = 26f;
    }
}
