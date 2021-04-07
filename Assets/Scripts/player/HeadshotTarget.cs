using UnityEngine;

public class HeadshotTarget : MonoBehaviour
{

    public void OneShotKill(string uid)
    {
        GetComponentInChildren<target>().TakeDamage(300, uid);
    }
}
