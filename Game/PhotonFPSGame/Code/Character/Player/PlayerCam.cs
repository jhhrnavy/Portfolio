using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] PlayerController target;
    [SerializeField] Transform Weapon;

    private void Start()
    {
        // LocalPlayer�� �ƴ϶�� �÷��̾� ī�޶� ����
        if(!target.photonView.IsMine)
        {
            Weapon.SetParent(target.transform);
            Destroy(gameObject);
        }
    }
}
