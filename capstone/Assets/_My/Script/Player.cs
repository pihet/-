using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Animator anim;

    public float invincibilityTime = 1.5f; // 무적 상태 지속 시간 (초)
    public bool isInvincible = false;

    [SerializeField]
    private Slider HPSlider;

    private float playerMaxHP = 10;
    private float HP = 10;
    public float playerCurrentHP = 0;
    public float playerMinHP = 0;

    void Start()
    {
        InitPlayerHP();
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HPSlider.value = playerCurrentHP / playerMaxHP;
        if (playerCurrentHP < HP)
        {
            if (playerCurrentHP > 0)
            {
                HP = playerCurrentHP;
                TakeHit();
            }
            else
            {
                PlayerDeath();
            }
        }
    }

    private void InitPlayerHP()
    {
        playerCurrentHP = playerMaxHP;
    }
    
    private void PlayerDeath()
    {
        if (playerCurrentHP <= 0)
        {
            anim.SetBool("death", true);
        }
    }

    private void TakeHit()
    {
        if (!isInvincible)
        {
            anim.SetTrigger("hit");

            StartCoroutine(MakeInvincible());
        }
    }
    
    IEnumerator MakeInvincible()
    {
        isInvincible = true;
        GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(invincibilityTime);
        GetComponent<CharacterController>().enabled = true;
        isInvincible = false;
    }
}
