using UnityEngine;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    public static menuManager Instance;
    [SerializeField] ui[] menus;

    void Awake()
    {
        Instance = this;
    }
    public void openMenu(string uiName)
    {
        for (byte i = 0; i < menus.Length; i++)
        {
            if (menus[i].uiName == uiName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                closeMenu(menus[i]);
            }
        }
    }
    public void openMenu(ui menu)
    {
        for (byte i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                closeMenu(menus[i]);
            }
        }
        menu.Open();
    }
    public void closeMenu(ui menu)
    {

        menu.Close();
    }
}
