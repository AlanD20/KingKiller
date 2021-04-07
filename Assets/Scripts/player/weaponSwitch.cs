using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class weaponSwitch : MonoBehaviourPunCallbacks
{
    public static int selectedWeapon = 0;
    Animator ani;
    PhotonView PlayerSwitchWeaponPV;
    public weaponUI weapUI;
    void Awake()
    {
        ani = GetComponentInParent<Animator>();
        PlayerSwitchWeaponPV = GetComponentInParent<PhotonView>();
    }
    void Start()
    {
        selectWeapon(selectedWeapon);
        weapUI.changeName("Assault Rifle");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSwitchWeaponPV.IsMine && !GameUI.GamePaused)
        {
            SwitchWeapons();
            SwitchWeaponWithNumbers();
            HUDUpdate(selectedWeapon);
            WeaponAmmo.wp = GetComponentInChildren<weapon>();
            ExitGames.Client.Photon.Hashtable hashTbl = new ExitGames.Client.Photon.Hashtable();
            hashTbl.Add("weaponIndex", selectedWeapon);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashTbl);
        }
    }
    void SwitchWeapons()
    {
        int previousSelectedWeapon = selectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = Convert.ToByte(transform.childCount - 1);
            else selectedWeapon--;
        }
        if (previousSelectedWeapon != selectedWeapon)
            selectWeapon(selectedWeapon);

    }
    void selectWeapon(int weaponIndex)
    {

        byte i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == weaponIndex) weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
    void SwitchWeaponWithNumbers()
    {
        int previousSelectedWeapon = selectedWeapon;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
            HUDUpdate(0);
            weapUI.changeName("Assault Rifle");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
            HUDUpdate(1);
            weapUI.changeName("Pistol");
        }
        if (previousSelectedWeapon != selectedWeapon)
            selectWeapon(selectedWeapon);
    }
    void HUDUpdate(int weaponIndex)
    {
        if (!PlayerSwitchWeaponPV.IsMine) return;
        if (weaponIndex == 1)
        {
            ani.SetLayerWeight(1, 1f);
            // if (PlayerSwitchWeaponPV.IsMine)
            weapUI.changeName("Pistol");
        }
        if (weaponIndex == 0)
        {
            ani.SetLayerWeight(1, 0f);
            // if (PlayerSwitchWeaponPV.IsMine)
            weapUI.changeName("Assault Rifle");
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PlayerSwitchWeaponPV.IsMine && targetPlayer == PlayerSwitchWeaponPV.Owner && changedProps["weaponIndex"] != null)
        {
            int otherIndex = (int)changedProps["weaponIndex"];
            selectWeapon(otherIndex);
            HUDUpdate(otherIndex);
        }
    }
}
