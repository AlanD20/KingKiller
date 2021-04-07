using UnityEngine;

public class SettingsStartMenuUI : MonoBehaviour
{
    public void SaveChanges()
    {
        SettingsScript.Instance.Save();
        this.BackToMenu();
    }
    public void BackToMenu()
    {
        menuManager.Instance.openMenu("menuUI");
    }
}
