using System;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [SerializeField] private GameObject hitPref;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;

    [SerializeField] private Animator playerAnim;

    [SerializeField] private Transform cameraRef;


    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float attackRadius;

    [SerializeField] private MousePivot mousePivot;
    [SerializeField] private Transform attackPoint;

    public void TakeDamage(int amount)
    {
        // playerAnim.SetLayerWeight(1, 0);
        playerAnim.Play("React");
        Destroy(Instantiate(hitPref, transform.position + Vector3.up, Quaternion.identity), 3f);

        // DOVirtual.DelayedCall(1f, () =>
        // {
        //     playerAnim.SetLayerWeight(1, 1);
        // });
        // Debug.Log("PLAYER GOT HIT");
    }

    private void Start()
    {

        cameraRef = Camera.main.transform;
    }

    public void Move(Vector3 direction)
    {

        var move = cameraRef.rotation * direction;
        move.y = rb.velocity.y;
        rb.velocity = new Vector3(move.x * Time.fixedDeltaTime * movementSpeed, move.y * Time.fixedDeltaTime * movementSpeed / 10f, move.z * Time.fixedDeltaTime * movementSpeed);



        if (direction != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(Vector3.Scale(cameraRef.rotation * direction, new Vector3(1, 0, 1)));
            playerAnim.SetBool("IsMoving", true);

            if (!IsAttacking)
                model.DOLocalRotate(Vector3.zero, 0.5f);
        }
        else
            playerAnim.SetBool("IsMoving", false);

    }

    private bool IsAttacking = false;

    public void Attack()
    {
        if (IsAttacking)
            return;

        IsAttacking = true;
        DOVirtual.DelayedCall(0.5f, () =>
        {
            IsAttacking = false;
        });

        // playerAnim.SetTrigger("Attack");
        playerAnim.Play("Attack");

        LookAtPivot(() =>
        {

        });

        Collider[] targets = Physics.OverlapSphere(attackPoint.position, attackRadius, targetLayer);


        Debug.Log("Targets " + targets.Length);

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
    }

    public void LookAtPivot(TweenCallback aCallback)
    {
        model.DOLookAt(new Vector3(mousePivot.pivot.transform.position.x, transform.position.y, mousePivot.pivot.transform.position.z), 0.3f).OnComplete(aCallback);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }


}
