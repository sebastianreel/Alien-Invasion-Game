using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Weapon weapon;
    private Vector2 moveInput;
    [SerializeField] int health, maxHealth = 3;
    [SerializeField] private AudioSource shootSoundEffect;
    [SerializeField] private AudioSource dieSoundEffect;
    public static event Action<PlayerController> OnPlayerKilled;
    //[SerializeField] ParticleSystem shootParticle = null;

    private float activeMoveSpeed;

    // Dash Stuff 
    /*
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    */

    [SerializeField] float startDashTime = 1f;
    [SerializeField] float dashSpeed = 1f;
    float currentDashTime;
    bool canDash = true;
    bool playerCollision = true;
    bool canMove = true;

    Vector2 mousePosition;
    public HealthBar healthBar;
    public GameObject loseTextObject;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        activeMoveSpeed = moveSpeed;

        loseTextObject.SetActive(false);
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
        rb.velocity = moveInput * activeMoveSpeed;

        if (Input.GetMouseButtonDown(0))
        {
            shootSoundEffect.Play();
            weapon.Fire();
            //shootEffect();
        }


        /*
        if (Input.GetMouseButtonDown(1) && canDash)
        {
            StartCoroutine(Dash());
        }
        */

        if (canDash && Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(Dash(new Vector2(1f, 1f)));
            }

            else if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(Dash(new Vector2(1f, -1f)));
            }

            else if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(Dash(new Vector2(-1f, 1f)));
            }

            else if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(Dash(new Vector2(-1f, -1f)));
            }

            if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(Dash(Vector2.up));
            }

            else if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(Dash(Vector2.left));
            }

            else if (Input.GetKey(KeyCode.S))
            {
                StartCoroutine(Dash(Vector2.down));
            }

            else if (Input.GetKey(KeyCode.D))
            {
                StartCoroutine(Dash(Vector2.right));
            }
        }

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //public void shootEffect()
    //{
    //    shootParticle.Play();
    //}

    private void FixedUpdate()
    {
        if (canMove == true) // CHANGE --- Need to disable movement when dashing.
        {
            moveInput.Normalize();
            rb.velocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed); // CHANGE --- No need to multiply by Time.deltaTime / Physics are already frame rate independent.
        }

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    IEnumerator Dash(Vector2 direction)
    {
        canDash = false;
        canMove = false; // CHANGE --- Need to disable movement when dashing.
        currentDashTime = startDashTime; // Reset the dash timer.

        while (currentDashTime > 0f)
        {
            currentDashTime -= Time.deltaTime; // Lower the dash timer each frame.

            rb.velocity = direction * dashSpeed; // Dash in the direction that was held down.
                                                 // No need to multiply by Time.DeltaTime here, physics are already consistent across different FPS.

            yield return null; // Returns out of the coroutine this frame so we don't hit an infinite loop.
        }

        rb.velocity = new Vector2(0f, 0f); // Stop dashing.

        canDash = true;
        canMove = true; // CHANGE --- Need to enable movement after dashing.
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        healthBar.SetHealth(health);


        Debug.Log($"Damage Taken: {damageAmount}");
        Debug.Log($"Player Health: {health}");

        if (health <= 0)
        {
            Destroy(gameObject);
            OnPlayerKilled?.Invoke(this);
            dieSoundEffect.Play();
        }
    }
}
