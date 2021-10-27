using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamagable
{
    // [SerializeField] private float movementSpeed = 500f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator playerAnim;

    private GameManager gameManagerRef;
    private PlayerController playerRef;

    [SerializeField] private AIControllerBase _aiController;

    [SerializeField] private NavMeshAgent agent;

    public void TakeDamage(int damage)
    {
        playerAnim.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => { playerAnim.transform.localScale = Vector3.one; });
        playerAnim.SetTrigger("React");
    }

    private void Start()
    {
        gameManagerRef = GameManager.Instance;
        playerRef = gameManagerRef.PlayerController;
    }

    private void Update()
    {
        // if (gameManagerRef.CheckState() == StateEnum.PlayState || Vector3.Distance(playerRef.transform.position, transform.position) >= 0.5f)
        //     Move(Vector3.Scale((playerRef.transform.position - transform.position).normalized, new Vector3(1, 0, 1)));

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
            playerAnim.SetBool("IsMoving", true);
            agent.SetDestination(destination);
        }

    }

    public void Attack(IDamagable target)
    {
        playerAnim.SetTrigger("Attack");
        target.TakeDamage(1);
    }

    // public void Move(Vector3 direction)
    // {

    //     var move = direction;
    //     move.y = rb.velocity.y;
    //     rb.velocity = new Vector3(move.x * Time.fixedDeltaTime * movementSpeed, move.y * Time.fixedDeltaTime * movementSpeed / 10f, move.z * Time.fixedDeltaTime * movementSpeed);



    //     if (direction != Vector3.zero)
    //     {
    //         rb.rotation = Quaternion.LookRotation(Vector3.Scale(direction, new Vector3(1, 0, 1)));
    //         playerAnim.SetBool("IsMoving", true);
    //     }
    //     else
    //         playerAnim.SetBool("IsMoving", false);

    // }
}
