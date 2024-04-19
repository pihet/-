using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int enemySpawnCount = 0;
    private int maxEnemySpawnCount = 10;

    [Header("Bullet")]
    [SerializeField]
    private Transform bulletPoint;
    [SerializeField]
    private GameObject bulletObj;
    [SerializeField]
    private float maxShootDelay = 0.2f;
    [SerializeField]
    private float curruntShootDelay = 0.2f;
    [SerializeField]
    private Text bulletText;
    private int maxBullet = 30;
    private int currentBullet = 0;

    [Header("Weapon Fx")]
    [SerializeField]
    private GameObject weaponFlashFX;
    [SerializeField]
    private Transform bulletCasePoint;
    [SerializeField]
    private GameObject bulletCaseFX;
    [SerializeField]
    private Transform weaponClipPoint;
    [SerializeField]
    private GameObject weaponClipFX;

    [Header("Enemy")]
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject[] spawnPoint;

    void Start()
    {
        instance = this;

        curruntShootDelay = 0;

        InitBullet();

        StartCoroutine(EnemySpawn());
    }

    // Update is called once per frame
    void Update()
    {
        bulletText.text = currentBullet + " / " + maxBullet;
    }

    public void Shooting(Vector3 targetPosition, Enemy enemy, AudioSource weaponSound, AudioClip shootingSound)
    {
        curruntShootDelay += Time.deltaTime;

        if (curruntShootDelay < maxShootDelay || currentBullet <= 0)
            return;

        currentBullet -= 1;
        curruntShootDelay = 0;

        weaponSound.clip = shootingSound;
        weaponSound.Play();

        Vector3 aim = (targetPosition - bulletPoint.position).normalized;

        //Instantiate(weaponFlashFX, bulletPoint);
        Instantiate(weaponFlashFX, bulletPoint);
        GameObject flashFX = PoolManager.instance.ActivateObj(1);
        SetObjPosition(flashFX, bulletPoint);
        flashFX.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

        //Instantiate(bulletCaseFX, bulletCasePoint);
        GameObject caseFX = PoolManager.instance.ActivateObj(2);
        SetObjPosition(caseFX, bulletCasePoint);

        //Instantiate(bulletObj, bulletPoint.position, Quaternion.LookRotation(aim, Vector3.up));
        
        GameObject prefabToSpawn = PoolManager.instance.ActivateObj(0);
        SetObjPosition(prefabToSpawn, bulletPoint);
        prefabToSpawn.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);
        

        //Raycast
        /*
        if(enemy �� null && enemy.enemyCurrentHP > 0)
        {
            enemy.enemyCurrentHP -= 1;
            Debug.Log("enemy HP :" + enemy.enemyCurrentHP);
        }
        */
    }

    public void ReroadClip()
    {
        //Instantiate(weaponClipFX, weaponClipPoint);
        GameObject clipFX = PoolManager.instance.ActivateObj(3);
        SetObjPosition(clipFX, weaponClipPoint);

        InitBullet();
    }
    private void InitBullet()
    {
        currentBullet = maxBullet;
    }

    private void SetObjPosition(GameObject obj, Transform targetTransform)
    {
        obj.transform.position = targetTransform.position;
    }

    IEnumerator EnemySpawn()
    {
        if(enemySpawnCount < maxEnemySpawnCount){
            Instantiate(enemy, spawnPoint[Random.Range(0,spawnPoint.Length)].transform.position, Quaternion.identity);
            enemySpawnCount++;
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemySpawn());
    }
}
