using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviourPunCallbacks
{
    public static PauseMenuScript Instance;
    [SerializeField] GameUI gameui;
    void Awake()
    {
        Instance = this;
    }
    public void ResumeGame()
    {
        gameui.ResumeGame();
    }
    public void Settings()
    {
        gameui.ShowSettingsMenuUI();
    }
    public void LeaveGame()
    {
        if (this.photonView.IsMine && this != null)
        {
            GameUI.GamePaused = false;
            PhotonNetwork.LeaveRoom(true);
            PhotonNetwork.Disconnect();
        }
    }
}
