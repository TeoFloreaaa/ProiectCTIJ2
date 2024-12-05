using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Referin?? la juc?tor
    public float detectionRange = 10f; // Distan?a de detectare a juc?torului
    public float attackRange = 2f; // Distan?a la care scheletul atac?
    public Animator animator; // Animatorul scheletului

    private NavMeshAgent agent; // Componenta pentru deplasare
    private bool isAttacking = false; // Flag pentru a verifica dac? atac?

    void Start()
    {
        // Ini?ializeaz? NavMeshAgent-ul
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        // Calculeaz? distan?a dintre schelet ?i juc?tor
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange)
        {
            // Scheletul atac? juc?torul
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            // Scheletul se deplaseaz? spre juc?tor
            MoveTowardsPlayer();
        }
        else
        {
            // Scheletul st? pe loc
            Idle();
        }
    }

    void MoveTowardsPlayer()
    {
        if (isAttacking) return;

        agent.isStopped = false;
        agent.SetDestination(player.position); // Deplasare spre juc?tor
        animator.SetBool("isWalking", true); // Activeaz? anima?ia de mers
    }

    void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            agent.isStopped = true; // Opre?te deplasarea
            animator.SetBool("isWalking", false); // Dezactiveaz? anima?ia de mers
            animator.SetTrigger("attack"); // Activeaz? anima?ia de atac

            // Po?i ad?uga logica pentru a aplica daune aici
            Invoke(nameof(ResetAttack), 1.5f); // Reseteaz? atacul dup? o durat?
        }
    }

    void Idle()
    {
        if (isAttacking) return;

        agent.isStopped = true;
        animator.SetBool("isWalking", false); // Dezactiveaz? anima?ia de mers
    }

    void ResetAttack()
    {
        isAttacking = false; // Permite urm?torul atac
    }
}
