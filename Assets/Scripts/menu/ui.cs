using UnityEngine;

public class ui : MonoBehaviour
{
    public string uiName;
    public bool open;
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }
    public void Close()
    {
        open = false;
        gameObject.SetActive(false);

    }
}
