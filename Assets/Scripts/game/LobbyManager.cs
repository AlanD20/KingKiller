using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;
    [SerializeField] Transform leaderboardContent;
    [SerializeField] GameObject LeaderboardItemPrefab;
    public static List<playerDetails> allPlayers;
    [SerializeField] GameObject timerUI;

    public const byte START_GAME_CODE_EVENT = 3;
    public const byte REFRESH_TIMER_CODE_EVENT = 4;
    public const byte DEATH_CODE_EVENT = 9;
    public const byte KILL_CODE_EVENT = 7;
    public const byte KILL_FEED_CODE_EVENT = 8;
    public static byte BULLET_IMPACT_CODE = 11;
    public static int gameTimeByMasterClient = 1;
    void Awake()
    {
        if (allPlayers == null) allPlayers = new List<playerDetails>();
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        //! DontDestroyOnLoad(gameObject); cause problem after re-joining the game.
        Instance = this;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1)
        {
            PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("start_scene");
    }
    public override void OnJoinedRoom()
    {
        UpdatePlayerList();
        foreach (Transform child in leaderboardContent)
            Destroy(child.gameObject);

        foreach (playerDetails pl in allPlayers)
        {
            Instantiate(LeaderboardItemPrefab, leaderboardContent).GetComponent<playerDetailsItem>().updateLeaderboard(pl);
        }

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        Instantiate(LeaderboardItemPrefab, leaderboardContent).GetComponent<playerDetailsItem>().NewPlayerLeaderboard(newPlayer);
    }
    public void UpdatePlayerList()
    {
        allPlayers.Clear();
        Player[] players = PhotonNetwork.PlayerList;
        playerDetails __playerDetail;
        foreach (Player pl in players)
        {
            __playerDetail = new playerDetails();
            if (pl.UserId == PhotonNetwork.LocalPlayer.UserId)
                __playerDetail = PlayerManager.Instance.pld;
            else
            {
                __playerDetail.username = pl.NickName;
                __playerDetail.PlayerID = pl.UserId;
            }
            if (!allPlayers.Contains(__playerDetail))
                allPlayers.Add(__playerDetail);
        }
    }
    public void UpdatePlayerScore()
    {
        foreach (Transform child in leaderboardContent)
            Destroy(child.gameObject);
        allPlayers.Sort((p1, p2) =>
        {
            if (p2.kills > p1.kills) return 1;
            else return 0;
        });
        foreach (playerDetails pl in allPlayers)
        {
            Instantiate(LeaderboardItemPrefab, leaderboardContent).GetComponent<playerDetailsItem>().updateLeaderboard(pl);
        }
    }
    public void StartGame(int timestamp)
    {
        if (!GameTimer.GameStarted)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameTimer.GameStarted = true;
            GameTimer.StartTime = timestamp;
            GameTimer.Instance.SetTimer(gameTimeByMasterClient);
            timerUI.SetActive(true);
        }
        else timerUI.SetActive(false);
    }
}