using System.IO;
using UnityEngine;
using Photon.Pun;
public class playerDetails
{
    public string username { get; set; }
    public short kills { get; set; }
    public string PlayerID { get; set; }
    public short deaths { get; set; }
    public playerDetails()
    {
        ExitGames.Client.Photon.Hashtable tbl = new ExitGames.Client.Photon.Hashtable();
        tbl.Add("kills", 0);
        tbl.Add("deaths", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(tbl);
        this.username = (string)PhotonNetwork.LocalPlayer.NickName;
        this.PlayerID = (string)PhotonNetwork.LocalPlayer.UserId;
        this.kills = 0;
        this.deaths = 0;
    }
    public short addKill()
    {
        return this.kills++;
    }
    public short addDeath()
    {
        return this.deaths++;
    }

}