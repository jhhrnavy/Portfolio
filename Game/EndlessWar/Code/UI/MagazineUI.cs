using TMPro;
using UnityEngine;

public class MagazineUI : MonoBehaviour
{
    public int currentAmmo; // 현재 탄창에 남은 총알 개수
    public int magazineSize; // 탄창 크기
    public int reserveAmmo; // 남아있는 총알

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
