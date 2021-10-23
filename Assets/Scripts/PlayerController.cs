using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;

    [SerializeField] private Animator playerAnim;

    [SerializeField] private Transform cameraRef;

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


}
