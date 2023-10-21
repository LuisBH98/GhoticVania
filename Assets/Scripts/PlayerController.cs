using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed = 5;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] TMP_Text totalScore;
    [SerializeField] private string gameOverScene;
    Rigidbody2D rb;
    Animator anim;
    bool facingRight = true;
    bool isFiring = false;
    float fireRate = 0.5f;
    float nextFire = 0.0f;
    float attackDamage = 40f;
    bool invulnerable = false;
    public bool isGrounded = true;
    private float timeToStart = 2f;
    public LayerMask enemyLayers;
    int playerScore = 0;
    string scoreText;
    int maxHealth = 100;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        scoreText = totalScore.text;
        totalScore.SetText(scoreText + playerScore.ToString());
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToStart <= 0)
        {
            if (!isFiring && !anim.GetBool("isHurt"))
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
            int enemyScore = enemy.GetComponentInChildren<Enemy>().getEnemyScore();
            this.playerScore += enemyScore;
            totalScore.SetText(scoreText + this.playerScore.ToString());
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !invulnerable )
        {
            Debug.Log("Player hits enemy");
            if(currentHealth > 0)
            {
                int enemyDamage = collision.gameObject.GetComponentInChildren<Enemy>().getEnemyDealDamage();
                Vector2 direction = (gameObject.transform.position - collision.gameObject.transform.position).normalized;
                rb.AddForce(direction * 15, ForceMode2D.Impulse);
                currentHealth -= enemyDamage;
                StartCoroutine(Hurt());
            } else
            {
                // GAME OVER
                StartCoroutine(Die());
                // SceneManager.LoadScene(gameOverScene);
            }
            
        }
    }

    public bool getFacingRight()
    {
        return this.facingRight;
    }

    public IEnumerator Hurt()
    {
        anim.SetBool("isHurt", true);
        invulnerable = true;
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isHurt", false);
        StopAllCoroutines();
        StartCoroutine(ResetInvulnerability());
    }

    public IEnumerator Die()
    {
        anim.SetBool("isHurt", true);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(gameOverScene);
    }

    IEnumerator ResetInvulnerability()
    {
        yield return new WaitForSeconds(1f);
        invulnerable = false;
    }

    void PlayHurtAnim()
    {
        anim.SetTrigger("hurt");
    }

}
