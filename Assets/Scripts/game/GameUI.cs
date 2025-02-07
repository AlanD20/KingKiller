﻿using UnityEngine;
using Photon.Pun;
public class GameUI : MonoBehaviour
{
    public static bool GamePaused = false;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject Settings;
    PhotonView SelfPV;
    void Awake()
    {
        SelfPV = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (!SelfPV.IsMine || target.DeathAnimation) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    public void ShowSettingsMenuUI()
    {
        PauseMenu.SetActive(false);
        Settings.SetActive(true);
    }
    public void ResumeGame()
    {
        GamePaused = false;
        Settings.SetActive(false);
        PauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    public void PauseGame()
    {
        GamePaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Settings.SetActive(false);
        PauseMenu.SetActive(true);
    }
}
