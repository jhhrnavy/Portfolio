using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] PlayerController target;
    [SerializeField] Transform Weapon;

    private void Start()
    {
        // LocalPlayer가 아니라면 플레이어 카메라 제거
        if(!target.photonView.IsMine)
        {
            Weapon.SetParent(target.transform);
            Destroy(gameObject);
        }
    }
}
