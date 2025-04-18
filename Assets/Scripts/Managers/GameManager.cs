using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Entities")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPositions;

    [Header("Game Variables")]
    [SerializeField] private float enemySpawnRate;
    [SerializeField] private GameObject playerPrefab;

    private GameObject tempEnemy;
    private bool isEnemySpawning;
    private Weapon meleeWeapon = new Weapon("Melee", 1, 0);
    private Player player;
    private bool isPlaying;

    public Action OnGameStart;
    public Action OnGameOver;

    public ScoreManager scoreManager;
    public PickupSpawner pickupSpawner;

    private static GameManager instance;

    public static GameManager GetInstance()
    {
        return instance;
    }

    void SetSingleton()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;
    }

    private void Awake()
    {
        SetSingleton();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //isEnemySpawning = true;
        //StartCoroutine(EnemySpawner());
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.E))
        {
            CreateEnemy();
        }*/
    }

    void CreateEnemy()
    {
        tempEnemy = Instantiate(enemyPrefab);
        tempEnemy.transform.position = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].position;
        tempEnemy.GetComponent<Enemy>().weapon = meleeWeapon;
        tempEnemy.GetComponent<MeleeEnemy>().SetMeleeEnemy(2, 0.25f);
    }

    //Continously using coroutine
    IEnumerator EnemySpawner()
    {
        while(isEnemySpawning)
        {
            yield return new WaitForSeconds(1.0f/enemySpawnRate);
            CreateEnemy();
        }
    }

    public void SetEnemySpawnState(bool status)
    {
        isEnemySpawning = status;
    }

    public void NotifyDeath(Enemy enemy)
    {
        pickupSpawner.SpawnPickup(enemy.transform.position);
    }

    public Player GetPlayer()
    {
        return player;
    }

    public bool IsPlaying()
    { return isPlaying; }

    public void StartGame()
    {
        //Create the player
        player = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity).GetComponent<Player>();
        player.OnDeath += StopGame;
        isPlaying = true;

        OnGameStart?.Invoke();
        //scoreManager.OnGameStart();
        StartCoroutine(GameStarter());
    }

    IEnumerator GameStarter()
    {
        yield return new WaitForSeconds(2.0f);
        isEnemySpawning = true;

        StartCoroutine(EnemySpawner());
    }

    public void StopGame()
    {
        isEnemySpawning = false;
        scoreManager.SetHighScore();

        StartCoroutine(GameStopper());
    }

    IEnumerator GameStopper()
    {
        isEnemySpawning = false;
        yield return new WaitForSeconds(2);
        isPlaying = false;

        //Delete all enemies
        foreach(Enemy item in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            Destroy(item.gameObject);
        }

        //Delete all pickups
        foreach (Pickup item in FindObjectsByType<Pickup>(FindObjectsSortMode.None))
        {
            Destroy(item.gameObject);
        }

        OnGameOver?.Invoke();
    }
}
