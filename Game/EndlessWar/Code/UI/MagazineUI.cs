using TMPro;
using UnityEngine;

public class MagazineUI : MonoBehaviour
{
    public int currentAmmo; // ���� źâ�� ���� �Ѿ� ����
    public int magazineSize; // źâ ũ��
    public int reserveAmmo; // �����ִ� �Ѿ�

    public TextMeshProUGUI currentAmmoText;
    public TextMeshProUGUI magazineSizeText;
    public TextMeshProUGUI reserveAmmoText;

    public void SetAmmo(int currentAmmo, int magazineSize, int reserveAmmo)
    {
        this.currentAmmo = currentAmmo;
        this.magazineSize = magazineSize;
        this.reserveAmmo = reserveAmmo;

        UpdateUI();
    }

    public void UpdateUI()
    {
        currentAmmoText.text = currentAmmo.ToString();
        magazineSizeText.text = magazineSize.ToString();
        reserveAmmoText.text = reserveAmmo.ToString();
    }
}
