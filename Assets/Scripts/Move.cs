using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] int speed;
    Rigidbody2D rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.linearVelocity = new Vector3 (rb.linearVelocity.x, speed, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.linearVelocity = new Vector3 (rb.linearVelocity.x, -speed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = new Vector3 (-speed, rb.linearVelocity.y, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = new Vector3 (speed, rb.linearVelocity.y, 0);
        }
    }
}
