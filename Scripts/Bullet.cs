using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 55f;
    public float lifetime = 4f;
    public float colliderEnableDelay = 0.1f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        RotateTowardsMovementDirection();
    }

    private void RotateTowardsMovementDirection()
    {
        Vector2 direction = GetComponent<Rigidbody2D>().velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            // Handle collision with walls
            // Ensure the bullet bounces off walls properly
            // You may need to adjust the Rigidbody2D properties or use physics materials
        }

        if (collision.collider.CompareTag("Player1") || collision.collider.CompareTag("Player2"))
        {
            // Handle collision with tanks
            TankController tank = collision.collider.GetComponent<TankController>();
            if (tank != null)
            {
                if (tank.CompareTag("Player1"))
                {
                    GameManager.Instance.Player2Scored();
                    GameManager.Instance.EndRound("Player 2");
                }
                else if (tank.CompareTag("Player2"))
                {
                    GameManager.Instance.Player1Scored();
                    GameManager.Instance.EndRound("Player 1");
                }

                // Play the explosion sound
                AudioManager.Instance.PlayExplosionSound();
            }

            Destroy(gameObject);
        }
    }

    private IEnumerator EnableColliderAfterDelay()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        yield return new WaitForSeconds(colliderEnableDelay);
        collider.enabled = true;
    }

}