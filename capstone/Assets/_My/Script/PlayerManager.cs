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
    private AudioClip reroadSound;
    private AudioSource weaponSound;

    private Enemy enemy;

    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
        weaponSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AimCheck();

    }
    private void AimCheck()
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

            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
            {
                //Debug.Log("Name : " + hit.transform.gameObject.name);
                targetPosition = hit.point;
                aimObject.transform.position = hit.point;

                enemy = hit.collider.gameObject.GetComponent<Enemy>();
            }
            else
            {
                targetPosition = camTransform.position + camTransform.forward * aimObjectDis;
                aimObject.transform.position = camTransform.position + camTransform.forward * aimObjectDis;
            }
            Vector3 targetAim = targetPosition;
            targetAim.y = transform.position.y;
            Vector3 aimDir = (targetAim - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 50f);
            SetRigWeight(1);
            if (input.shoot)
            {
                anim.SetBool("Shoot", true);
                GameManager.instance.Shooting(targetPosition, enemy, weaponSound, shootingSound);
            }
            else
            {
                anim.SetBool("Shoot", false);
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
    private void AimControll(bool isChecked)
    {
        aimCam.gameObject.SetActive(isChecked);
        aimImage.SetActive(isChecked);
        controller.isAimMove = isChecked;
    }

    public void Reload()
    {
        //Debug.Log("Reload");
        controller.isReload = false;
        SetRigWeight(1);
        anim.SetLayerWeight(1, 0);
        /*
        PlayWeaponSound(reroadSound[2]);
        */
    }

    private void SetRigWeight(float weight)
    {
        aimRig.weight = weight;
        handRig.weight = weight;
    }

    public void ReroadWeaponClip()
    {
        GameManager.instance.ReroadClip();
        /*
        PlayWeaponSound(reroadSound[0]);
        */
    }

    public void ReroadInsertClip()
    {
        /*
        PlayWeaponSound(reroadSound[1]);
        */
    }

    private void PlayWeaponSound(AudioClip sound)
    {
        weaponSound.clip = sound;
        weaponSound.Play();
    }
}
