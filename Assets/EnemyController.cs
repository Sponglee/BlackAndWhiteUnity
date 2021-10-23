using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 500f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator playerAnim;

    private GameManager gameManagerRef;
    private PlayerController playerRef;

    private void Start()
    {
        gameManagerRef = GameManager.Instance;
        playerRef = gameManagerRef.PlayerController;
    }

    private void Update()
    {
        if (gameManagerRef.CheckState() == StateEnum.PlayState || Vector3.Distance(playerRef.transform.position, transform.position) >= 0.5f)
            Move(Vector3.Scale((playerRef.transform.position - transform.position).normalized, new Vector3(1, 0, 1)));
    }


    public void Move(Vector3 direction)
    {

        var move = direction;
        move.y = rb.velocity.y;
        rb.velocity = new Vector3(move.x * Time.fixedDeltaTime * movementSpeed, move.y * Time.fixedDeltaTime * movementSpeed / 10f, move.z * Time.fixedDeltaTime * movementSpeed);



        if (direction != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(Vector3.Scale(direction, new Vector3(1, 0, 1)));
            playerAnim.SetBool("IsMoving", true);
        }
        else
            playerAnim.SetBool("IsMoving", false);

    }
}
