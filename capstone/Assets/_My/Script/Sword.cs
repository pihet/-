using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public enum Type { Melee, Range }; //Melee = ��������
    public Type type;  //Ÿ��
    public int damage; //������
    public float rate; //���ݼӵ�
    public BoxCollider meleeArea; //���ݹ���
    public TrailRenderer trailEffect; //����Ʈ
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
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.enemyCurrentHP -= damage;
            }

            Enemy2 enemy2 = other.gameObject.GetComponent<Enemy2>();
            if (enemy2 != null)
            {
                enemy2.enemyCurrentHP -= damage;
            }

            Enemy3 enemy3 = other.gameObject.GetComponent<Enemy3>();
            if (enemy3 != null)
            {
                enemy3.enemyCurrentHP -= damage;
            }
        }
    }
}
