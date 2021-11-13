using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamagable
{
    [SerializeField] private float movementSpeed = 500f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator playerAnim;

    [SerializeField] private GameObject hitPref;

    private GameManager gameManagerRef;
    private PlayerController playerRef;

    [SerializeField] private AIControllerBase _aiController;

    [SerializeField] public NavMeshAgent agent;

    [SerializeField] private HPSystem hpSystem;


    [SerializeField] private int attackDamage = 5;


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
            playerAnim.SetLayerWeight(1, 0);
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
        _aiController.CheckBehaviour(this);


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
            }
        }
    }


    //Initial attack animation
    public void AttackAnim()
    {
        playerAnim.SetBool("IsMoving", false);
        playerAnim.SetTrigger("Attack0");
    }

    //Damage calculations and end attack animation
    public void Attack(IDamagable target)
    {

        // Debug.Log("HHHHH");
        playerAnim.SetTrigger("Attack1");

        if (target != null)
            target.TakeDamage(attackDamage);


    }

}
