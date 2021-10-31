using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;

    [SerializeField] private Animator playerAnim;

    [SerializeField] private Transform cameraRef;


    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float attackRadius;

    [SerializeField] private MousePivot mousePivot;

    public void TakeDamage(int amount)
    {
        playerAnim.SetTrigger("React");
        Debug.Log("PLAYER GOT HIT");
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
        }
        else
            playerAnim.SetBool("IsMoving", false);

    }


    public void Attack()
    {
        playerAnim.SetTrigger("Attack");

        LookAtPivot();

        Collider[] targets = Physics.OverlapSphere(transform.position, attackRadius, targetLayer);

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

    public void LookAtPivot()
    {
        transform.LookAt(new Vector3(mousePivot.pivot.transform.position.x, transform.position.y, mousePivot.pivot.transform.position.z));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }


}
