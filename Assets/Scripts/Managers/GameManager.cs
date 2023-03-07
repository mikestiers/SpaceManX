using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager instance
    {
        get => _instance;
    }

    public int maxLives = 10;
    private int _lives = 5;

    public int lives
    {
        get { return _lives; }
        set
        {
            if (_lives > value)
                //we lost a life - we need to respawn
                Respawn();

            _lives = value;

            if (_lives > maxLives)
                _lives = maxLives;

            if (_lives < 0)
                SceneManager.LoadScene(2);

            Debug.Log("Lives have been set to " + _lives.ToString());
        }
    }

    public PlayerController playerPrefab;
    [HideInInspector] public PlayerController playerInstance = null;
    [HideInInspector] public Level currentLevel = null;
    [HideInInspector] public Transform currentSpawnPoint;

    private void Awake() // runs before start
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        lives = maxLives;
    }

    public void SpawnPlayer(Transform spawnPoint)
    {
        playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        currentSpawnPoint = spawnPoint;
    }

    void Respawn()
    {
        if (playerInstance)
            playerInstance.transform.position = currentSpawnPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
                SceneManager.LoadScene(1);
            else if (SceneManager.GetActiveScene().buildIndex == 2)
                SceneManager.LoadScene(1);
            else
                SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.K))
            lives--;
    }

    public void UpdateCheckpoint(Transform spawnPoint)
    {
        currentSpawnPoint = spawnPoint;
    }
}
