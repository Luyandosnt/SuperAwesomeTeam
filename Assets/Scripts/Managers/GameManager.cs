using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;
    public GameObject nextWaveButton;
    public TMP_Text waveText;
    public Enemy[] Enemies;
    public Transform EnemiesParent;
    public Transform[] InstancePosition;
    public float Frequency;
    public int amountToSpawn;

    public GameObject winScreen;

    private float Timer;
    private int Position = 0;
    [HideInInspector] public bool isWaveActive = false;
    private int enemiesToSpawn = 5;
    private int currentEnemies;
    private int lastWave;

    public static int currentLevel = 1;

    [HideInInspector] public int currentWave = 0;
    public static GameManager gameManager { get; private set; }

    private void Awake()
    {
        gameManager = this;
    }

    private void Start()
    {
        if (currentLevel == 1)
        {
            lastWave = 15;
        }
        else if (currentLevel == 2)
        {
            lastWave = 25;
        }
        else if (currentLevel == 3)
        {
            lastWave = 35;
        }
        NextWave();
    }

    public void NextWave()
    {
        waveText.color = Color.gray;
        nextWaveButton.SetActive(false);
        HandleWaveScaling();
    }

    public void HandleWaveEnd()
    {
        if (isWaveActive && currentEnemies == 0)
        {
            nextWaveButton.SetActive(true);
            isWaveActive = false;
            audioManager.PlayWaveClearMusic();
            audioManager.PlayPrepTimeMusic();
            GeneralResourceController.Instance.ClassifyResource(GeneralResourceController.ResourceType.Gold, 10);
        }
        if (currentWave == lastWave)
        {
            HandleWin();
        }
    }

    void HandleWaveScaling()
    {
        if (currentLevel == 1) { HandleWaveScalingLevel1(); }
        else if (currentLevel == 2) { HandleWaveScalingLevel2(); }
        else if (currentLevel == 3) { HandleWaveScalingLevel3(); }
    }

    private void HandleWaveScalingLevel3()
    {
        throw new System.NotImplementedException();
    }

    private void HandleWaveScalingLevel2()
    {
        throw new System.NotImplementedException();
    }

    private void HandleWaveScalingLevel1()
    {
        if (currentWave == 0)
        {
            currentWave++;
            enemiesToSpawn = amountToSpawn;
            currentEnemies = enemiesToSpawn;
            audioManager.PlayPrepTimeMusic();
            waveText.text = "PREPARE FOR WAVE 1";
            Invoke("FirstWaveSetOn", 15f);
        }
        else
        {
            currentWave++;
            enemiesToSpawn = amountToSpawn + (2 * (currentWave-1));
            currentEnemies = enemiesToSpawn;
            Frequency -= 1f;
            audioManager.PlayWaveStartMusic();
            if (currentWave == 15)
            {
                audioManager.PlayBossMusic();
                waveText.color = Color.red;
                waveText.text = "BOSS WAVE";
            }
            else
            {
                audioManager.PlayCombatMusic();
                waveText.text = "WAVE " + currentWave;
            }
            isWaveActive = true;
            Debug.Log("Wave " + currentWave + " has started!");
        }
    }

    void FirstWaveSetOn()
    {
        isWaveActive = true;
        waveText.text = "WAVE 1";
        audioManager.PlayWaveStartMusic();
        audioManager.PlayCombatMusic();
        Debug.Log("Wave " + currentWave + " has started!");
    }

    private void HandleWin()
    {
        winScreen.SetActive(true);
        audioManager.PlayWinMusic();
    }

    private void Update()
    {
        HandleEnemySpawns();
    }

    void HandleEnemySpawns()
    {
        if (currentLevel == 1) { HandleEnemySpawnsLevel1(); }
        else if (currentLevel == 2) { HandleEnemySpawnsLevel2(); }
        else if (currentLevel == 3) { HandleEnemySpawnsLevel3(); }
    }
    private void HandleEnemySpawnsLevel3()
    {
        throw new System.NotImplementedException();
    }

    private void HandleEnemySpawnsLevel2()
    {
        throw new System.NotImplementedException();
    }

    private void HandleEnemySpawnsLevel1()
    {
        if (isWaveActive && Timer >= Frequency)
        {
            Position = Random.Range(0, InstancePosition.Length);
            int enemyIndex = -1;
            if (currentWave <= 2)
            {
                enemyIndex = 0;
            }
            else if (currentWave <= 5)
            {
                enemyIndex = Random.Range(0, 2);
            }
            else
            {
                enemyIndex = Random.Range(0, Enemies.Length);
            }
            int levelToSet = -1;
            if (currentWave <= 2)
            {
                levelToSet = 1;
            }
            else if (currentWave <= 5)
            {
                levelToSet = Random.Range(1, 3);
            }
            else if (currentWave <= 10)
            {
                levelToSet = Random.Range(1, 4);
            }
            else if (currentWave < 15)
            {
                levelToSet = Random.Range(2, 4);
            }
            else
            {
                levelToSet = Random.Range(2, 5);
            }
            Enemy enemy = Instantiate(Enemies[enemyIndex], InstancePosition[Position].position, Quaternion.identity, EnemiesParent);
            enemy.SetLevel(levelToSet);
            Timer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isWaveActive)
            Timer += Time.deltaTime;
    }

    public void AddEnemy()
    {
        currentEnemies++;
    }

    public void RemoveEnemy()
    {
        currentEnemies--;
        HandleWaveEnd();
    }
}
