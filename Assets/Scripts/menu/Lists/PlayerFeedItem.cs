using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerFeedItem : MonoBehaviour
{
    [SerializeField] TMP_Text killText;

    public void ShowKillFeed(string[] data)
    {
        killText.text = PlayerFeed.GetKillFeed(data);
        StartCoroutine(DeleteAfter());
    }
    IEnumerator DeleteAfter()
    {
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }

}
