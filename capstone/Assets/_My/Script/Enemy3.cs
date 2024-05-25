using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy3 : MonoBehaviour
{
    public bool isRange;
    public static Enemy instance;
    [SerializeField]
    private float enemyMaxHP = 10;
    public float enemyCurrentHP = 0;
    private float hp = 10;

    private NavMeshAgent agent;
    private Animator animator;

    private GameObject targetPlayer;
    private float targetDelay = 0.5f;

    private CapsuleCollider enemyCollider;
    private BoxCollider handCollider;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<CapsuleCollider>();
        handCollider = GetComponentInChildren<BoxCollider>();

        targetPlayer = GameObject.FindWithTag("Player");

        handCollider.enabled = false;
        InitEnemyHP();
    }

    public float cooltime;
    public float currenttime;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;

        if (hp > enemyCurrentHP)
        {
            animator.SetTrigger("Hit");
            hp -= 1;
        }
        if (enemyCurrentHP <= 0 || remainTime == 0)
        {
            StartCoroutine(EnemyDie());
            return;
        }

        if (targetPlayer != null)
        {
            float maxDelay = 0.5f;
            targetDelay += Time.deltaTime;

            if (targetDelay < maxDelay)
            {
                return;
            }

            agent.destination = targetPlayer.transform.position;
            //transform.LookAt(targetPlayer.transform.position);

            isRange = Vector3.Distance(transform.position, targetPlayer.transform.position) <= agent.stoppingDistance;

            if (isRange)
            {
                if (currenttime <= 0)
                {
                    StartCoroutine(EAttack());
                    currenttime = cooltime;
                }
            }
            else
            {
                animator.SetBool("isRun", true);
                animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
            }
            currenttime -= Time.deltaTime * 10;
            targetDelay = 0;
        }
    }

    private void InitEnemyHP()
    {
        enemyCurrentHP = enemyMaxHP;
    }

    public void enemyHitDamage(float takeDamage)
    {
        enemyCurrentHP -= takeDamage;

        if(enemyCurrentHP <= 0)
        {
            EnemyDie();
        }
    }

    IEnumerator EnemyDie()
    {
        if (!GameManager.instance.isLive)
            yield break;

        agent.speed = 0;
        animator.SetTrigger("Dead");
        enemyCollider.enabled = false;

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        gameObject.SetActive(false);
        InitEnemyHP();
        agent.speed = 2.5f;
        enemyCollider.enabled = true;
    }

    IEnumerator EAttack()
    {
        animator.SetBool("isRun", false);
        agent.isStopped = true;
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        handCollider.enabled = true;
        animator.SetBool("Attack", false);

        yield return new WaitForSeconds(0.5f);
        handCollider.enabled = false;

        yield return new WaitForSeconds(2f);
        agent.speed = 2.5f;
        agent.isStopped = false;
    }
}