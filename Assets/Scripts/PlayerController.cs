using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed = 5;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    Rigidbody2D rb;
    Animator anim;
    bool facingRight = true;
    bool isFiring = false;
    float fireRate = 0.5f;
    float nextFire = 0.0f;
    float attackDamage = 40f;
    public bool isGrounded = true;
    private float timeToStart = 2f;
    public LayerMask enemyLayers;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToStart <= 0)
        {
            if (!isFiring)
            {
                float dirX = Input.GetAxis("Horizontal");
                rb.transform.Translate(new Vector3(speed * Time.deltaTime * dirX, 0, 0));

                anim.SetBool("isMoving", (dirX > 0 || dirX < 0));

                if (dirX > 0 && !facingRight)
                {
                    this.Flip();
                }
                else if (dirX < 0 && facingRight)
                {
                    this.Flip();
                }

                if (Input.GetMouseButtonDown(0) && this.isGrounded)
                {
                    StartCoroutine(this.Attack());
                }
            }

            if (this.isGrounded)
            {
                anim.SetBool("isJumping", false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && this.isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                this.isGrounded = false;
                anim.SetBool("isJumping", true);
            }
        } else
        {
            timeToStart -= Time.deltaTime;
        }

        
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        this.facingRight = !this.facingRight;
    }

    private IEnumerator Attack() {
        isFiring = true;
        anim.SetBool("isFiring", isFiring);
        yield return new WaitForSeconds(0.5f);
        isFiring = false;
        anim.SetBool("isFiring", isFiring);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Enemy>().Die();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.isGrounded = true;
        }
    }
    
    public bool getFacingRight()
    {
        return this.facingRight;
    }

}
