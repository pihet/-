using Cinemachine;
using StarterAssets;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class PlayerManager : MonoBehaviour
{
    private StarterAssetsInputs input;
    private ThirdPersonController controller;
    private Animator anim;
    private Sword swing;

    public float meleetime = 1.5f;
    public float meleetimer = 0.0f;

    [Header("Aim")]
    [SerializeField]
    private CinemachineVirtualCamera aimCam;
    [SerializeField]
    private GameObject aimImage;
    [SerializeField]
    private GameObject aimObject;
    [SerializeField]
    private float aimObjectDis = 10f;
    [SerializeField]
    private LayerMask targetLayer;

    [Header("IK")]
    [SerializeField]
    private Rig handRig; 
    [SerializeField]
    private Rig aimRig;

    [Header("Weapon Sound Effect")]
    [SerializeField]
    private AudioClip shootingSound;
    [SerializeField]
    private AudioClip[] reroadSound;
    private AudioSource weaponSound;

    private Enemy enemy;
    private Enemy2 enemy2;
    private Enemy3 enemy3;
    [SerializeField]
    private GameObject bloodEffect; 
    private float giveDamageOf = 1f;


    void Start()
    {
        if(!GameManager.instance.isLive)
            return;
        GameObject.Find("WeaponHolder").GetComponent<NewBehaviourScript>();
        input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
        weaponSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.isLive)
            return;
        meleetimer += Time.deltaTime;

        AimCheck();
        Melee();

        if (NewBehaviourScript.selectedWeapon == 0)
        {
            anim.SetInteger("switch weapon", 0);
        }
        else if (NewBehaviourScript.selectedWeapon == 1)
        {
            swing = GameObject.Find("HisuKnife").GetComponent<Sword>();
            anim.SetInteger("switch weapon", 1);
        }
        if (input.shoot && !controller.isReload)
        {
            Shoot();
        }
    }
    private void Melee()
    {
        if (anim.GetInteger("switch weapon") == 1)
        {
            if (input.melee)
            {
                if (meleetimer > meleetime)
                {
                    swing.Use();
                    anim.SetTrigger("Melee");
                    meleetimer = 0.0f;
                    input.melee = false;
                }
            }
        }
        else
        {
            if (input.melee)
            {
                input.melee = false;
            }
        }
    }
    private void AimCheck()
    {
        if (anim.GetInteger("switch weapon") == 0)
        {
            if (input.reload)
            {
                input.reload = false;

                if (controller.isReload)
                {
                    return;
                }

                AimControll(false);
                anim.SetLayerWeight(1, 1);
                anim.SetTrigger("Reload");
                controller.isReload = true;
            }

            if (controller.isReload)
            {
                return;
            }


            if (input.aim)
            {
                AimControll(true);

                anim.SetLayerWeight(1, 1);
                SetRigWeight(0);
                Vector3 targetPosition = Vector3.zero;
                Transform camTransform = Camera.main.transform;
                RaycastHit hit;

                Vector3 lookDirection = targetPosition - transform.position;
                if (lookDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
                }

                if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
                {
                    //Debug.Log("Name : " + hit.transform.gameObject.name);
                    targetPosition = hit.point;
                    aimObject.transform.position = hit.point;

                    enemy = hit.collider.gameObject.GetComponent<Enemy>();
                    enemy2 = hit.collider.gameObject.GetComponent<Enemy2>();
                    enemy3 = hit.collider.gameObject.GetComponent<Enemy3>();
                }
                else
                {
                    targetPosition = camTransform.position + camTransform.forward * aimObjectDis;
                    aimObject.transform.position = camTransform.position + camTransform.forward * aimObjectDis;
                }
                Vector3 targetAim = targetPosition;
                targetAim.y = transform.position.y;
                Vector3 aimDir = (targetAim - transform.position).normalized;


                SetRigWeight(1);

                if (input.shoot && !controller.isReload)
                {
                    Shoot(targetPosition);
                }

            }
            else
            {
                AimControll(false);
                SetRigWeight(0);
                anim.SetLayerWeight(1, 0);
                anim.SetBool("Shoot", false);
            }
        }
        
    }
    private void AimControll(bool isChecked)
    {
        aimCam.gameObject.SetActive(isChecked);
        aimImage.SetActive(isChecked);
        controller.isAimMove = isChecked;
    }

    private void Shoot(Vector3? target = null)
    {
        if (GameManager.instance.curruntBullet > 0)
        {
            anim.SetLayerWeight(1, 1);
            anim.SetTrigger("Shoot");
            Vector3 targetPosition = Vector3.zero;
            Transform camTransform = Camera.main.transform;
            RaycastHit hit;

            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
            {
                targetPosition = hit.point;
                enemy = hit.collider.gameObject.GetComponent<Enemy>();
                enemy2 = hit.collider.gameObject.GetComponent<Enemy2>();
                enemy3 = hit.collider.gameObject.GetComponent<Enemy3>();
            }
            else
            {
                targetPosition = camTransform.position + camTransform.forward * aimObjectDis;
            }

            Vector3 lookDirection = targetPosition - transform.position;
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
            }

            GameManager.instance.Shooting(targetPosition, enemy, weaponSound, shootingSound);
            if (enemy != null)
            {
                ApplyDamage(enemy, hit);
            }
            if (enemy2 != null)
            {
                ApplyDamage(enemy2, hit);
            }
            if (enemy3 != null)
            {
                ApplyDamage(enemy3, hit);
            }

            GameManager.instance.curruntBullet--;
        }
    }

    private void ApplyDamage(Enemy enemy, RaycastHit hit)
    {
        enemy.enemyHitDamage(giveDamageOf);
        GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(bloodEffectGo, 1f);
    }

    private void ApplyDamage(Enemy2 enemy, RaycastHit hit)
    {
        enemy2.enemyHitDamage(giveDamageOf);
        GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(bloodEffectGo, 1f);
    }

    private void ApplyDamage(Enemy3 enemy, RaycastHit hit)
    {
        enemy3.enemyHitDamage(giveDamageOf);
        GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(bloodEffectGo, 1f);
    }

    public void Reload()
    {
        controller.isReload = false;
        SetRigWeight(1);
        anim.SetLayerWeight(1, 0);
        PlayWeaponSound(reroadSound[2]);
    }

    private void SetRigWeight(float weight)
    {
        aimRig.weight = weight;
        handRig.weight = weight;
    }

    public void ReroadWeaponClip()
    {
        GameManager.instance.ReroadClip();
        PlayWeaponSound(reroadSound[0]);
    }

    public void ReroadInsertClip()
    {
        PlayWeaponSound(reroadSound[1]);
    }

    private void PlayWeaponSound(AudioClip sound)
    {
        weaponSound.clip = sound;
        weaponSound.Play();
    }
}
