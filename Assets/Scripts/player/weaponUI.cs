using UnityEngine;
using TMPro;
using Photon.Pun;
public class weaponUI : MonoBehaviour
{
    public TMP_Text weaponName;
    public TMP_Text weaponAmmo;
    PhotonView SelfHUDWeaponPV;
    void Awake()
    {
        SelfHUDWeaponPV = GetComponentInParent<PhotonView>();
    }
    public void changeName(string weapon)
    {
        if (!SelfHUDWeaponPV.IsMine) return;
        weaponName.text = weapon;
    }
    public void changeAmmo(int ammo, int maxAmmo)
    {
        if (!SelfHUDWeaponPV.IsMine) return;
        weaponAmmo.text = ammo + "/" + maxAmmo;
    }
    public void EmptyAmmo(string text)
    {
        if (!SelfHUDWeaponPV.IsMine) return;
        weaponAmmo.text = text;
    }
}
