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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        distToPlayer = gameObject.transform.position.x - player.transform.position.x;
        if(distToPlayer > 0)
        {
            // Enemy appears on the right facing left
            Flip();
        }
        else if(distToPlayer < 0 && !isFacingRight)
        {
            // Enemy appears on the left facing right
            Flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        distToPlayer = gameObject.transform.position.x - player.transform.position.x;
        if (distToPlayer > 0 && isFacingRight)
        {
            this.Flip();
        }
        else if (distToPlayer < 0 && !isFacingRight)
        {
            this.Flip();
        }
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
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


}
