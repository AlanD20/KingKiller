using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Linq;
public class LobbyHolder : MonoBehaviourPun
{
    public static LobbyHolder Instance;
    [SerializeField] GameObject leaderboard;
    [SerializeField] TMP_Text lobbyName;
    PhotonView PV;
    bool isLeaderboardOpen = false;
    void Awake()
    {
        Instance = this;
        PV = GetComponent<PhotonView>();
    }
    void Update()
    {
        ShowLeaderBoard();
    }
    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NCER_PlayerDeath;
        PhotonNetwork.NetworkingClient.EventReceived += NCER_BulletImpact;
        PhotonNetwork.NetworkingClient.EventReceived += NCER_StartGame;
        PhotonNetwork.NetworkingClient.EventReceived += NCER_KillFeed;
    }
    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NCER_PlayerDeath;
        PhotonNetwork.NetworkingClient.EventReceived -= NCER_BulletImpact;
        PhotonNetwork.NetworkingClient.EventReceived -= NCER_StartGame;
        PhotonNetwork.NetworkingClient.EventReceived -= NCER_KillFeed;
    }
    void ShowLeaderBoard()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isLeaderboardOpen)
            {
                isLeaderboardOpen = false;
                leaderboard.SetActive(false);
            }
            else
            {
                isLeaderboardOpen = true;
                leaderboard.SetActive(true);
                lobbyName.text = PhotonNetwork.CurrentRoom.Name;
            }
        }
    }
    void NCER_PlayerDeath(EventData obj)
    {
        if (obj.Code == LobbyManager.KILL_CODE_EVENT)
        {
            playerDetails temp = LobbyManager.allPlayers.ElementAt((int)obj.CustomData);
            temp.addKill();
            LobbyManager.Instance.UpdatePlayerScore();
            return;
        }
        if (obj.Code == LobbyManager.DEATH_CODE_EVENT)
        {
            playerDetails temp = LobbyManager.allPlayers.ElementAt((int)obj.CustomData);
            temp.addDeath();
            LobbyManager.Instance.UpdatePlayerScore();
            return;
        }
    }
    void NCER_BulletImpact(EventData obj)
    {
        if (obj.Code == LobbyManager.BULLET_IMPACT_CODE)
        {
            object[] data = (object[])obj.CustomData;
            weapon.Instance.BulletImpact((Vector3)data[0], (Vector3)data[1]);
        }
    }
    void NCER_StartGame(EventData obj)
    {
        if (obj.Code == LobbyManager.START_GAME_CODE_EVENT)
        {
            int data = (int)obj.CustomData;
            LobbyManager.Instance.StartGame(data);
        }
    }
    void NCER_KillFeed(EventData obj)
    {
        if (obj.Code == LobbyManager.KILL_FEED_CODE_EVENT)
        {
            object[] data = (object[])obj.CustomData;
            string[] feed = { (string)data[0], (string)data[1] };
            KillFeed.Instance.AddKillFeedDetail(feed);
        }
    }

}
