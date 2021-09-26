using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;

    public void Move(Vector3 direction)
    {
        // Двигаем сферу согласно пользовательскому импуту.
        var move = Camera.main.transform.rotation * direction;
        move.y = 0f;
        rb.velocity = move * movementSpeed;



    }
}
