using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    private float xRangeMargin = 3f;
    private static int maxEnemiesOnScreen = 5;
    private GameObject player;
    public float spawnPositionX;
    public float spawnPositionY;
    public float orientation;
    private float dist;
    private List<GameObject> enemiesOnScreen = new List<GameObject>();
    float timePassed = 0f;
    float timeToSpawn = 4f;
    int scoreLimit = 100;

    // Visible properties
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject skeletonSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        InvokeRepeating("SpawnEnemy", 2f, timeToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        // TODO: Set a time limit to spawn to avoid caos spawning and increment enemy's life to make the game harder
        if(player.GetComponent<PlayerController>().getPlayerScore() > scoreLimit)
        {
            CancelInvoke();
            timeToSpawn = timeToSpawn - timeToSpawn * 0.3f;
            scoreLimit += 100;
            InvokeRepeating("SpawnEnemy", 2f, timeToSpawn);
        }
    }

    void SpawnEnemy()
    {
        if (playerController.isGrounded && enemiesOnScreen.Count < maxEnemiesOnScreen)
        {
            orientation = Random.Range(0, 2) * 2 - 1;
            spawnPositionX = transform.position.x - (xRangeMargin * orientation);
            dist = Vector3.Distance(player.transform.position, skeletonSpawner.transform.position);
            spawnPositionY = skeletonSpawner.transform.position.y + dist;
            enemiesOnScreen.Add(Instantiate(enemies[0], new Vector3(spawnPositionX, player.transform.position.y, 0), enemies[0].transform.rotation));
        }
    }

    public void DestroyEnemy(GameObject obj)
    {
        enemiesOnScreen.Remove(obj);
        Destroy(obj);
    }
}
