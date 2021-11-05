using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IDamagable
{
    [SerializeField] private GameObject hitPref;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;
    [SerializeField] private Transform attackSystem;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Transform cameraRef;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float attackRadius;
    [SerializeField] private MousePivot mousePivot;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private HPSystem hpSystem;
    [SerializeField] private ComboAttackSystem comboSystem;
    private bool IsAttacking = false;

    private void Start()
    {
        cameraRef = Camera.main.transform;
    }

    public void Move(Vector3 direction)
    {
        var move = cameraRef.rotation * direction;
        move.y = rb.velocity.y;

        if (direction != Vector3.zero)
        {
            agent.Move(new Vector3(move.x * Time.fixedDeltaTime * movementSpeed, move.y * Time.fixedDeltaTime * movementSpeed / 10f, move.z * Time.fixedDeltaTime * movementSpeed));
            playerAnim.SetBool("IsMoving", true);

            if (!IsAttacking)
                model.DOLocalRotate(Vector3.zero, 0.2f);
        }
        else
        {
            playerAnim.SetBool("IsMoving", false);
            agent.isStopped = true;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Scale(cameraRef.rotation * direction, new Vector3(1, 0, 1))), 0.5f);
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

    public void Dash()
    {
        playerAnim.SetBool("IsDashing", true);

        Move(transform.forward * 10f);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            rb.velocity = Vector3.zero;
            playerAnim.SetBool("IsDashing", false);
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
