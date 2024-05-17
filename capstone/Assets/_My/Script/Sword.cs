using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public enum Type { Melee, Range }; //Melee = 근접공격
    public Type type;  //타입
    public int damage; //데미지
    public float rate; //공격속도
    public BoxCollider meleeArea; //공격범위
    public TrailRenderer trailEffect; //이펙트
    public ThirdPersonController third;

    public void Use()
    {
        StopCoroutine("Swing");
        StartCoroutine("Swing");
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.4f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.35f);
        trailEffect.enabled = false;
        meleeArea.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().enemyCurrentHP -= damage;
        }
    }
}
