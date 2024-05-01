using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float enemyMaxHP = 10;
    public float enemyCurrentHP = 0;
    private float hp = 10;

    private NavMeshAgent agent;
    private Animator animator;

    private GameObject targetPlayer;
    private float targetDelay = 0.5f;

    private CapsuleCollider enemyCollider;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<CapsuleCollider>();

        targetPlayer = GameObject.FindWithTag("Player");

        InitEnemyHP();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        if (hp > enemyCurrentHP)
        {
            animator.SetTrigger("Hit");
            hp -= 1;
        }
        if (enemyCurrentHP <= 0)
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
            transform.LookAt(targetPlayer.transform.position);

            bool isRange = Vector3.Distance(transform.position, targetPlayer.transform.position) <= agent.stoppingDistance;

            if (isRange)
            {
                animator.SetBool("isRun", false);
                animator.SetTrigger("Attack");
            }
            else
            {
                animator.SetBool("isRun", true);
                animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
            }
            targetDelay = 0;
        }
    }

    private void InitEnemyHP()
    {
        enemyCurrentHP = enemyMaxHP;
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
        agent.speed = 1;
        enemyCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!GameManager.instance.isLive)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Player playerComponent = collision.gameObject.GetComponent<Player>();

            if (playerComponent != null)
            {
                playerComponent.playerCurrentHP -= 1;

                if (playerComponent.playerCurrentHP <= 0)
                {
                    GameManager.instance.isLive = false;
                    Invoke("EndScene", 3f);
                }
            }
        }
    }
        public void EndScene()
    {
        SceneManager.LoadScene(2);
    }
}