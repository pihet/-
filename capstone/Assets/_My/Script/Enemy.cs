using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]

    private float enemyMaxHP = 10;
    public float enemyCurruntHP = 0;
    private float hp = 10;

    private NavMeshAgent agent;
    private Animator animator;

    private GameObject targetPlayer;
    private float targetDealay = 0.5f;

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
        if (hp > enemyCurruntHP)
        {
            animator.SetTrigger("Hit");
            hp -= 1;
        }
        if (enemyCurruntHP <= 0)
        {
            StartCoroutine(EnemyDie());
            return;
        }

        if (targetPlayer != null)
        {
            float maxDealay = 0.5f;
            targetDealay += Time.deltaTime;

            if (targetDealay < maxDealay)
            {
                return;
            }

            agent.destination = targetPlayer.transform.position;
            transform.LookAt(targetPlayer.transform.position);

            bool isRange = Vector3.Distance(transform.position, targetPlayer.transform.position) <= agent.stoppingDistance;

            if (isRange)
            {
                animator.SetBool("isRun", false); // ������ �� ���� �ִϸ��̼� ����
                animator.SetTrigger("Attack");
            }
            else
            {
                animator.SetBool("isRun", true); // �̵� ���� ��� "zombie running" �ִϸ��̼� ����
                animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
            }
            targetDealay = 0;
        }
    }

    private void InitEnemyHP()
    {
        enemyCurruntHP = enemyMaxHP;
    }

    IEnumerator EnemyDie()
    {
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
    if(collision.gameObject.CompareTag("Player"))
    {
        Player playerComponent = collision.gameObject.GetComponent<Player>();

        if(playerComponent != null)
        {
            playerComponent.playerCurrentHP -= 1;
            
            if(playerComponent.playerCurrentHP <= 0)
            {
                playerComponent.playerCurrentHP = 0;
            }
        }
    }
}
}