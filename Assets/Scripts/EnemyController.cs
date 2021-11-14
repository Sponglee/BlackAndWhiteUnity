using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamagable
{
    public Transform attackPoint;
    public NavMeshAgent agent;

    [SerializeField] private GameObject attackFx;
    [SerializeField] private float movementSpeed = 500f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private GameObject hitPref;
    [SerializeField] private AIControllerBase _aiController;
    [SerializeField] private HPSystem hpSystem;
    [SerializeField] private int attackDamage = 5;
    private GameManager gameManagerRef;
    private PlayerController playerRef;

    public void TakeDamage(int damage)
    {
        if (hpSystem.IsDead)
            return;


        hpSystem.DecreaseHp(damage);
        playerAnim.transform.DOScale(Vector3.one * 1.3f, 0.2f).SetEase(Ease.OutQuad).OnComplete(() => { playerAnim.transform.localScale = Vector3.one; });
        playerAnim.Play("React");
        Destroy(Instantiate(hitPref, transform.position + Vector3.up, Quaternion.identity), 3f);

        _aiController.ChangeState(AIState.GotHit);

        if (hpSystem.IsDead)
        {
            playerAnim.SetBool("IsDead", true);
            playerAnim.Play("Death");
            _aiController.ChangeState(AIState.Dead);
        }

    }

    private void Start()
    {
        gameManagerRef = GameManager.Instance;
        playerRef = gameManagerRef.PlayerController;
        agent.avoidancePriority = Random.Range(0, 50);
        agent.speed = movementSpeed;

    }

    private void Update()
    {
        //Check for behaviour
        _aiController.CheckBehaviour();


    }

    public void Move(Vector3 destination)
    {
        if (destination == Vector3.zero)
        {
            playerAnim.SetBool("IsMoving", false);
        }
        else
        {
            if (agent.isActiveAndEnabled)
            {

                playerAnim.SetBool("IsMoving", true);
                agent.SetDestination(destination);
                FaceTarget(destination);
            }
        }
    }



    private void FaceTarget(Vector3 destination)
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            agent.updateRotation = false;
            //insert your rotation code here
        }
        else
        {
            agent.updateRotation = true;
        }

        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.5f);
    }


    //Initial attack animation
    public void AttackAnim()
    {
        playerAnim.ResetTrigger("Attack1");
        playerAnim.SetBool("IsMoving", false);
        playerAnim.SetTrigger("Attack0");
    }

    //Damage calculations and end attack animation
    public void Attack(Transform target)
    {

        Debug.Log("ATTACK");
        playerAnim.SetTrigger("Attack1");
        AttackEffect(target);

        if (target != null)
            target.GetComponent<IDamagable>().TakeDamage(attackDamage);


    }

    public virtual void AttackEffect(Transform attackTarget)
    {
        Destroy(Instantiate(attackFx, attackPoint.position, Quaternion.identity), 2f);

    }
}
