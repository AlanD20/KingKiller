using UnityEngine;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] GameUI gameui;
    public void SaveChanges()
    {
        SettingsScript.Instance.Save();
        this.IgnoreChanges();
    }
    public void IgnoreChanges()
    {
        gameui.PauseGame();
    }
}
