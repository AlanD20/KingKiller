using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;
using System.Linq;

public class WeaponAmmo : MonoBehaviourPun
{
    public static weapon wp;
    public static WeaponAmmo Instance;
    PhotonView PV;
    void Awake()
    {
        Instance = this;
        PV = GetComponentInParent<PhotonView>();
        wp = GetComponentInChildren<weapon>();
    }
    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NCER_Ammo;
    }
    void Disable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NCER_Ammo;
    }
    void NCER_Ammo(EventData obj)
    {
        if (obj.Code == LobbyManager.KILL_CODE_EVENT)
        {
            playerDetails temp = LobbyManager.allPlayers.ElementAt((int)obj.CustomData);
            if (temp.PlayerID == PhotonNetwork.LocalPlayer.UserId)
                AddAmmo(false);
        }

    }
    public void AddAmmo(bool AmmoBox)
    {
        if (!PV.IsMine) return;
        int FWP = AmmoBox ? 27 : 9;
        int SWP = AmmoBox ? 15 : 5;
        if (weaponSwitch.selectedWeapon == 0)
            wp.magAmmo += FWP;
        if (weaponSwitch.selectedWeapon == 1)
            wp.magAmmo += SWP;
    }
}
