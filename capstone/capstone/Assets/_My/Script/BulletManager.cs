using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody bulletRigidbody;

    [SerializeField]
    private float moveSpeed = 10f;
    private float destroyTime = 3f;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;

        if(destroyTime <= 0)
        {
            DestroyBullet();
        }

        BullectMove();
    }

    private void BullectMove()
    {
        bulletRigidbody.velocity = transform.forward * moveSpeed;
    }

    private void DestroyBullet()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
        destroyTime = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.enemyCurrentHP -= 1;
            }

            Enemy2 enemy2 = other.gameObject.GetComponent<Enemy2>();
            if (enemy2 != null)
            {
                enemy2.enemyCurrentHP -= 1;
            }

            Enemy3 enemy3 = other.gameObject.GetComponent<Enemy3>();
            if (enemy3 != null)
            {
                enemy3.enemyCurrentHP -= 1;
            }
            DestroyBullet();
        }
    }
}
