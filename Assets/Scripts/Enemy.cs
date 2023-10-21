using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    float distToPlayer;
    float worldEndBoundY;
    Animator anim;
    bool isFacingRight = false;
    [SerializeField] private float speed = 0.5f;
    int enemyScore = 20;
    int enemyDealDamage = 20;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;
        if(player.transform.position.x > gameObject.transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1 * (isFacingRight ? 1 : -1);
            MoveEnemy(1);
            isFacingRight = true;
        } else
        {
            scale.x = Mathf.Abs(scale.x) * (isFacingRight ? -1 : 1);
            MoveEnemy(-1);
            isFacingRight = false;
        }

        transform.localScale = scale;
    }

    void MoveEnemy(int speed_direction)
    {
        if (anim.GetBool("startWalk"))
        {
            transform.Translate(speed * Time.deltaTime * speed_direction, 0, 0);
        }
    }

    public void Die()
    {
        Debug.Log("Enemy died");
        StartCoroutine(PlayDieAnim());
    }

    IEnumerator PlayDieAnim()
    {
        anim.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Enemy>().enabled = false;
        yield return new WaitForSeconds(1f);
        SpawnEnemies spawnManager = GameObject.Find("EnemySpawnManager").GetComponent<SpawnEnemies>();
        spawnManager.DestroyEnemy(gameObject);
    }

    void StartWalking()
    {
        anim.SetBool("startWalk", true);
        GetComponent<Collider2D>().enabled = true;
    }

    public int getEnemyScore()
    {
        return this.enemyScore;
    }

    public int getEnemyDealDamage()
    {
        return this.enemyDealDamage;
    }
}
