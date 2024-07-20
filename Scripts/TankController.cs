using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public float moveSpeed = 75f;
    public float rotateSpeed = 200f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 2f;
    public Vector3 initialPosition;

    private string movementAxisName;
    private string rotationAxisName;
    private KeyCode fireKey;
    private float shootCooldownTimer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;

        if (gameObject.tag == "Player1")
        {
            movementAxisName = "Vertical";
            rotationAxisName = "Horizontal";
            fireKey = KeyCode.Equals;
        }
        else if (gameObject.tag == "Player2")
        {
            movementAxisName = "Vertical2";
            rotationAxisName = "Horizontal2";
            fireKey = KeyCode.Space;
        }

        shootCooldownTimer = 0f;
    }

    void Update()
    {
        if (!GameManager.Instance.IsRoundOver)
        {
            float move = Input.GetAxis(movementAxisName) * moveSpeed * Time.deltaTime;
            float rotate = Input.GetAxis(rotationAxisName) * rotateSpeed * Time.deltaTime;

            if (Input.GetAxis(movementAxisName) > 0)
            {
                move *= 1.2f;
            }

            transform.Translate(0, move, 0);
            transform.Rotate(0, 0, -rotate);
            shootCooldownTimer -= Time.deltaTime;

            if (Input.GetKeyDown(fireKey) && shootCooldownTimer <= 0)
            {
                Shoot();
                shootCooldownTimer = shootCooldown;
            }
        }
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    public Transform[] spawnPoints;

    public void ResetPosition()
    {
        // Randomly select a spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform randomSpawnPoint = spawnPoints[randomIndex];

        // Reset tank's position and rotation to the selected spawn point
        transform.position = randomSpawnPoint.position;
        transform.rotation = randomSpawnPoint.rotation;

        // Reset tank's velocity
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
