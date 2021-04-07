using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class lobbyListItem : MonoBehaviour
{
    [SerializeField] Text lobbyText;
    public RoomInfo _info;
    public void setupLobby(RoomInfo info)
    {
        _info = info;
        lobbyText.text = info.Name + "    Max: " + info.MaxPlayers.ToString();
    }
    public void joinExistingLobby()
    {
        StartMenuScript.Instance.JoinLobby(_info);
    }
}
