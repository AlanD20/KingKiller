using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
public class healthbar : MonoBehaviour
{
    AudioSource AS;
    public Slider slider;
    public Gradient gried;
    public Image fill;
    PhotonView PlayerHealthPV;
    void Awake()
    {
        AS = GetComponent<AudioSource>();
        PlayerHealthPV = GetComponent<PhotonView>();
    }
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gried.Evaluate(1f);

    }
    public void SetHealth(float health)
    {
        if (health <= 35f) LowHealthSound();
        slider.value = health;
        fill.color = gried.Evaluate(slider.normalizedValue);
    }
    void LowHealthSound()
    {
        if (PlayerHealthPV.IsMine)
            AS.Play();
    }

}
