using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] float dist;
    [SerializeField] private float worldEndBoundY;
    private Animator anim;
    private bool isFacingRight = true;
    private SpawnEnemies spawnScript;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        spawnScript = GameObject.Find("EnemySpawnManager").GetComponent<SpawnEnemies>();
        dist = gameObject.transform.position.x - player.transform.position.x;
        if(dist > 0 && isFacingRight)
        { 
            this.Flip();
        } else if(dist < 0 && !isFacingRight){
            this.Flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        dist = gameObject.transform.position.x - player.transform.position.x;
        if (dist > 0 && isFacingRight)
        {
            this.Flip();
        }
        else if(dist < 0 && !isFacingRight){
            this.Flip();
        }

        if (gameObject.transform.position.y < worldEndBoundY)
        {
            spawnScript.DestroyEnemy(gameObject);
        }
    }

    public void Die()
    {
        Debug.Log("Enemy died!");
        // Play die animation
        anim.SetBool("isDead", true);

        //GetComponent<Collider2D>().isTrigger = true;
        this.enabled = false;
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }

    public bool isOutOfBounds()
    {
        if(gameObject.transform.position.y < worldEndBoundY)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
