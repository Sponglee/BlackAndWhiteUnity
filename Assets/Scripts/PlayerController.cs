using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IDamagable
{

    [SerializeField] private GameObject hitPref;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Transform cameraRef;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float attackRadius;
    [SerializeField] private MousePivot mousePivot;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private HPSystem hpSystem;
    [SerializeField] private ComboAttackSystem comboSystem;

    public UnityEvent OnDash;

    public bool IsAttacking = false;
    public bool IsDashing = false;
    private void Start()
    {
        cameraRef = Camera.main.transform;
    }

    public void Move(Vector3 direction)
    {
        if (IsDashing)
            return;

        var move = Vector3.Scale(cameraRef.rotation * direction, new Vector3(1, 0, 1));
        move.y = rb.velocity.y;

        if (direction != Vector3.zero)
        {
            // agent.isStopped = true;
            agent.Move(new Vector3(move.x * Time.fixedDeltaTime * agent.speed, move.y * Time.fixedDeltaTime * movementSpeed / 10f, move.z * Time.fixedDeltaTime * agent.speed));
            playerAnim.SetBool("IsMoving", true);

            if (!IsAttacking)
                model.DOLocalRotate(Vector3.zero, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Scale(cameraRef.rotation * direction, new Vector3(1, 0, 1))), 0.5f);
        }
        else
        {
            playerAnim.SetBool("IsMoving", false);
        }
    }

    public void Dash(Vector3 dir)
    {
        if (IsDashing)
            return;

        OnDash.Invoke();

        IsDashing = true;
        playerAnim.SetBool("IsDashing", true);
        agent.speed = 0f;

        Vector3 startPos = transform.position;
        var move = Vector3.Scale(cameraRef.rotation * dir, new Vector3(1, 0, 1)).normalized;

        Vector3 offset = move;

        SmoothAgentMove(offset * dashDistance);

        DOVirtual.DelayedCall(0.3f, () =>
        {

            agent.speed = movementSpeed;
            agent.ResetPath();
            playerAnim.SetBool("IsDashing", false);
            IsDashing = false;
        });
    }


    public async void SmoothAgentMove(Vector3 destination)
    {
        float elapsed = 0f;
        float duration = 0.15f;


        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            agent.Move(Vector3.Slerp(Vector3.zero, destination, elapsed / duration));
            await Task.Yield();
        }
    }

    public void Jump()
    {
        // agent.SetDestination()
    }

    public void Attack()
    {
        if (IsAttacking)
            return;

        IsAttacking = true;

        DOVirtual.DelayedCall(0.5f, () =>
        {
            IsAttacking = false;
        });

        comboSystem.Attack();

        LookAtPivot(() =>
        {

            Collider[] targets = Physics.OverlapSphere(attackPoint.position, attackRadius, targetLayer);
            if (targets.Length > 0)
            {
                foreach (Collider item in targets)
                {
                    if (item.GetComponent<IDamagable>() != null)
                    {
                        item.GetComponent<IDamagable>().TakeDamage(1);
                    }
                }
            }
        });
    }


    public void Death()
    {
        playerAnim.Play("Death");
        GameManager.Instance.ChangeState(StateEnum.PauseState);
    }

    public void TakeDamage(int amount)
    {
        if (hpSystem.IsDead)
            return;

        // playerAnim.SetLayerWeight(1, 0);
        playerAnim.Play("React");

        hpSystem.DecreaseHp(amount);

        if (hpSystem.IsDead)
        {
            Death();
        }

        Destroy(Instantiate(hitPref, transform.position + Vector3.up, Quaternion.identity), 3f);
    }

    public void LookAtPivot(TweenCallback aCallback)
    {
        model.DOLookAt(new Vector3(mousePivot.pivot.transform.position.x, transform.position.y, mousePivot.pivot.transform.position.z), 0.25f).OnComplete(aCallback);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }


}
