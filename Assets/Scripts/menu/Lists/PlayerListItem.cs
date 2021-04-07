using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Text playerName;
    Player _player;
    public void setupPlayer(Player player)
    {
        _player = player;
        playerName.text = _player.NickName;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (_player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
