using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int enemySpawnCount = 0;
    private int maxEnemySpawnCount = 10;

    [Header("Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;

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
    private int curruntBullet = 0;

    [Header("Weapon FX")]
    [SerializeField]
    private GameObject weaponFlashFx;
    [SerializeField]
    private Transform bulletCasePoint;
    [SerializeField]
    private GameObject bulletCaseFX;
    [SerializeField]
    private Transform weaponClipPoint;
    [SerializeField]
    private GameObject weaponClipFx;

    [Header("Enemy")]
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject[] spawnPoint;

    void Start()
    {
        
        isLive = true;

        instance = this;

        curruntShootDelay = 0;

        curruntBullet = 0;

        InitBullet();

        StartCoroutine(EnemySpawn());
    }

    void Update()
    {
        bulletText.text = curruntBullet + " / " + maxBullet;

        gameTime += Time.deltaTime;
        
        if(gameTime > maxGameTime){
            gameTime = maxGameTime;
        }
    }

    public void Shooting(Vector3 targetPosition, Enemy enemy, AudioSource weaponSound, AudioClip shootingSound)
    {
        if(!isLive)
            return;

        curruntShootDelay += Time.deltaTime;

        if (curruntShootDelay < maxShootDelay || curruntBullet <= 0)
            return;

        curruntBullet -= 1;
        curruntShootDelay = 0;

        weaponSound.clip = shootingSound;
        weaponSound.Play();

        Vector3 aim = (targetPosition - bulletPoint.position).normalized;

        //Instantiate(weaponFlashFx,bulletPoint);
        GameObject flashFX = PoolManager.instance.ActivateObj(1);
        SetObjPosition(flashFX,bulletPoint);
        flashFX.transform.rotation = Quaternion.LookRotation(aim,Vector3.up);

        //Instantiate(bulletCaseFX, bulletCasePoint);
        GameObject caseFX = PoolManager.instance.ActivateObj(2);
        SetObjPosition(caseFX,bulletCasePoint);

        //Instantiate(bulletObj,bulletPoint.position, Quaternion.LookRotation(aim,Vector3.up));
        GameObject prefabTospawn = PoolManager.instance.ActivateObj(0);
        SetObjPosition(prefabTospawn,bulletPoint);
        prefabTospawn.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);
    }

    public void ReroadClip()
    {
        if(!isLive)
            return;
        //Instantiate(weaponClipFx, weaponClipPoint);
        GameObject clipFx = PoolManager.instance.ActivateObj(3);
        SetObjPosition(clipFx,weaponClipPoint);

        InitBullet();
    }
    
    private void InitBullet()
    {
        curruntBullet = maxBullet;
    }

    private void SetObjPosition(GameObject obj, Transform targetTransform)
    {
        obj.transform.position = targetTransform.position;
    }

    IEnumerator EnemySpawn()
    {
        if (!isLive)
            yield break;

        if(enemySpawnCount < maxEnemySpawnCount){
            Instantiate(enemy, spawnPoint[Random.Range(0,spawnPoint.Length)].transform.position, Quaternion.identity);
            enemySpawnCount++;
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemySpawn());
    }
}
