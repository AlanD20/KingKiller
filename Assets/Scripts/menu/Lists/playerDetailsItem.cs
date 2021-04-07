using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class playerDetailsItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text userText;
    [SerializeField] TMP_Text killText;
    [SerializeField] TMP_Text deathText;
    playerDetails _playerDetail;
    Player _player;
    PhotonView PV;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        TMP_Text[] col = GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text tt in col)
        {
            if (tt.name == "username")
                userText = tt;
            else if (tt.name == "kills")
                killText = tt;
            else if (tt.name == "deaths")
                deathText = tt;
        }
    }
    public void updateLeaderboard(playerDetails ppl)
    {
        userText.text = ppl.username;
        killText.text = ppl.kills.ToString();
        deathText.text = ppl.deaths.ToString();
    }
    public void NewPlayerLeaderboard(Player _pl)
    {
        _player = _pl;
        userText.text = _pl.NickName;
        string playerkills = (string)_player.CustomProperties["kills"];
        killText.text = playerkills != null ? playerkills : "0";
        string playerdeaths = (string)_player.CustomProperties["deaths"];
        deathText.text = playerdeaths != null ? playerdeaths : "0";
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