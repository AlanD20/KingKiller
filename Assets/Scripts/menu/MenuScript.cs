using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class MenuScript : MonoBehaviour
{
    [SerializeField] Button btnStart;
    [SerializeField] TMP_InputField usernameField;
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName")) usernameField.text = PlayerPrefs.GetString("PlayerName");
    }

    void Update()
    {
        if (string.IsNullOrEmpty(usernameField.text))
            btnStart.interactable = false;
        else btnStart.interactable = true;

    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Settings()
    {
        menuManager.Instance.openMenu("settingsUI");
    }
    public void multiplayer()
    {
        if (PhotonNetwork.IsConnected && usernameField.text != "")
        {
            PhotonNetwork.NickName = usernameField.text;
            PlayerPrefs.SetString("PlayerName", usernameField.text);
            menuManager.Instance.openMenu("lobbyUI");
        }

    }
}
