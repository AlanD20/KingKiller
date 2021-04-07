using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class StartMenuScript : MonoBehaviourPunCallbacks
{
    public static StartMenuScript Instance;
    void Awake()
    {
        Instance = this;
    }
    public TMP_Text versionText;
    public TMP_Text statusText;
    string statusConnection = "No Connection";
    //
    //Lobby Components..
    //
    [SerializeField] Button btnBack;
    [SerializeField] Button btnCreateLobby;
    [SerializeField] TMP_InputField LobbyName;
    [SerializeField] TMP_Text errorInfo;
    [SerializeField] Transform lobbyListContent;
    [SerializeField] GameObject lobbyListItemPrefab;

    //
    // Joined Lobby components.
    //
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] TMP_Text gametimeText;
    void Start()
    {
        if (PlayerPrefs.HasKey("XSense") || PlayerPrefs.HasKey("YSense"))
        {
            CameraScript.sensitivity.x = PlayerPrefs.GetFloat("XSense");
            CameraScript.sensitivity.x = PlayerPrefs.GetFloat("YSense");
        }
        statusConnection = "Connecting To Server...";
        PhotonNetwork.SendRate = 35;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "1.0";
        versionText.text = "v" + PhotonNetwork.GameVersion;
    }
    void Update()
    {
        statusText.text = statusConnection;
    }
    public override void OnConnectedToMaster()
    {
        statusConnection = "Connected";
        PhotonNetwork.JoinLobby();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        statusConnection = "Disconnected " + cause.ToString();
    }
    public override void OnJoinedLobby()
    {
        menuManager.Instance.openMenu("menuUI");
    }
    public override void OnJoinedRoom()
    {
        menuManager.Instance.openMenu("joinedLobbyUI");
        btnDisable(false);
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        for (byte i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().setupPlayer(players[i]);
        }
    }
    public override void OnLeftRoom()
    {
        menuManager.Instance.openMenu("lobbyUI");
    }
    public void JoinLobby(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        btnDisable(true);
    }
    void btnDisable(bool check)
    {
        if (check)
        {
            btnBack.interactable = false;
            btnCreateLobby.interactable = false;
        }
        else
        {
            btnBack.interactable = true;
            btnCreateLobby.interactable = true;
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        StartCoroutine(ShowError("Unable to create a lobby: " + message));
        btnDisable(false);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        StartCoroutine(ShowError("Unable to join the lobby: " + message));
        btnDisable(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> lobbyList)
    {
        foreach (Transform trans in lobbyListContent)
        {
            Destroy(trans.gameObject);
        }
        for (byte i = 0; i < lobbyList.Count; i++)
        {
            if (lobbyList[i].RemovedFromList) continue;
            Instantiate(lobbyListItemPrefab, lobbyListContent).GetComponent<lobbyListItem>().setupLobby(lobbyList[i]);
        }
    }
    public void CreateLobby()
    {
        if (string.IsNullOrEmpty(LobbyName.text))
        {
            StartCoroutine(ShowError("Lobby is Empty"));
            return;
        }
        RoomOptions LobbyOptions = new RoomOptions();
        LobbyOptions.MaxPlayers = 6;
        LobbyOptions.CleanupCacheOnLeave = true;
        LobbyOptions.PublishUserId = true;
        PhotonNetwork.JoinOrCreateRoom(LobbyName.text, LobbyOptions, TypedLobby.Default);
        menuManager.Instance.openMenu("loadingUI");
        btnDisable(true);
    }
    public void ReturnToMenu()
    {
        menuManager.Instance.openMenu("menuUI");
    }
    //
    //Joined lobby Methods...
    //
    public void StartGame()
    {
        ///!!after the game started.......
        menuManager.Instance.openMenu("loadingUI");
        LobbyManager.gameTimeByMasterClient = Convert.ToInt32(gametimeText.text);
        PhotonNetwork.LoadLevel(1);
    }
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().setupPlayer(newPlayer);
    }
    IEnumerator ShowError(string text)
    {
        errorInfo.text = text;
        yield return new WaitForSeconds(3f);
        errorInfo.text = "";
    }
}
