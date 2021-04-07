using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject pads;
    void Awake()
    {
        pads.transform.GetChild(0).gameObject.SetActive(false);
    }
}
