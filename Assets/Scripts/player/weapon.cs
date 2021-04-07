using System;
using System.Linq;
using UnityEngine;
using System.Collections; //for relaoding...
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class weapon : MonoBehaviourPun
{
    public static weapon Instance;
    [SerializeField] Camera fpsCam;
    [SerializeField] ParticleSystem muzzle;
    [SerializeField] GameObject impact;
    playerManagement plm;
    Animator ani;
    [SerializeField] weaponUI weapUI;
    PhotonView PlayerWeaponPV;
    public string WeaponName;
    //ammo and reloading
    public float range = 100f, damage = 10f, fireRate = 7f, nextTimeToFire = 0f, reloadTime = 3f;

    public int maxAmmo = 10, magAmmo = 90;
    int currentAmmo = 0;

    bool isReloading = false;
    //! weapon soundss
    AudioSource AS;
    void Awake()
    {
        Instance = this;
        ani = GetComponentInParent<Animator>();
        PlayerWeaponPV = GetComponent<PhotonView>();
        AS = GetComponent<AudioSource>();
    }

    void Start()
    {
        currentAmmo = maxAmmo;
        weapUI.changeAmmo(currentAmmo, this.magAmmo);
    }
    void OnEnable()
    {
        isReloading = false;
        ani.SetBool("isReloading", false);
    }


    void Update()
    {
        if (!PlayerWeaponPV.IsMine) return;
        if (!GameUI.GamePaused)
        {

            weapUI.changeAmmo(currentAmmo, magAmmo);
            if (magAmmo <= 0 && currentAmmo <= 0)
            {
                weapUI.EmptyAmmo("No Ammo");
                return;
            }
            if (Input.GetKey(KeyCode.R) && currentAmmo < maxAmmo) StartCoroutine(ReloadGun()); //calling IEnumerator function.
            if (isReloading) return;
            if (currentAmmo <= 0)
            { //reloading the gun
                StartCoroutine(ReloadGun()); //calling IEnumerator function.
                return;
            }
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Fire();
            }
        }

    }
    private void Fire()
    {
        if (!PlayerWeaponPV.IsMine) return;
        PlayerWeaponPV.RPC("RPC_Play", RpcTarget.Others);
        muzzle.Play();
        AS.Play();
        RaiseEventOptions rep = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        RaycastHit hit;
        currentAmmo--; //ammo reduction
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            hit.transform.GetComponent<HeadshotTarget>()?.OneShotKill(PhotonNetwork.LocalPlayer.UserId);
            hit.transform.GetComponent<target>()?.TakeDamage(damage, PhotonNetwork.LocalPlayer.UserId);
            if (!PlayerWeaponPV.IsMine) BulletImpact(hit.point, hit.normal);
            object[] BulletInfo = new object[] { hit.point, hit.normal };
            PhotonNetwork.RaiseEvent(LobbyManager.BULLET_IMPACT_CODE, BulletInfo, rep, SendOptions.SendReliable);
            // PlayerWeaponPV.RPC("RPC_BulletImpact", RpcTarget.All, hit.point, hit.normal);
        }
    }
    [PunRPC]
    void RPC_Play()
    {
        muzzle.Play();
        AS.Play();
    }
    public void BulletImpact(Vector3 hitPosition, Vector3 hitNormal)
    {
        if (PlayerWeaponPV.IsMine) return;
        Collider[] colliders = Physics.OverlapSphere(hitPosition, .3f);
        if (colliders.Length != 0)
        {
            GameObject imp = Instantiate(impact, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal) * impact.transform.rotation);
            Destroy(imp, 2f);
            imp.transform.SetParent(colliders[0].transform);
        }
    }

    IEnumerator ReloadGun() //reloading function
    {
        ani.speed = 4 / reloadTime;
        ani.SetBool("isReloading", true);
        isReloading = true;
        yield return new WaitForSeconds(reloadTime - .25f);
        ani.SetBool("isReloading", false);
        yield return new WaitForSeconds(.25f);
        if (this.currentAmmo <= 0 && this.magAmmo <= 0)
            weapUI.EmptyAmmo("No Ammo");

        short leftOver = Convert.ToInt16(maxAmmo - this.currentAmmo);

        if (this.magAmmo <= maxAmmo)
            LeftOverAmmoCheck(leftOver);
        else
        {
            this.magAmmo -= leftOver;
            this.currentAmmo = maxAmmo;
        }
        // magAmmo -= maxAmmo;
        // currentAmmo = maxAmmo;  
        isReloading = false;
    }
    void LeftOverAmmoCheck(short leftOver)
    {
        if (this.magAmmo >= leftOver)
        {
            this.magAmmo -= leftOver;
            this.currentAmmo = maxAmmo;
        }
        else if (this.magAmmo <= leftOver && this.magAmmo <= this.currentAmmo)
        {
            this.currentAmmo = 30;
            this.magAmmo = 0;
        }
        else
        {
            this.currentAmmo += leftOver;
            this.magAmmo -= leftOver;
        }
    }

}
