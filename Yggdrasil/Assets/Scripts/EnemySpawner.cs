using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using TMPro;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject resourcePrefab;
    [SerializeField] private GameObject buttonStart;
    [SerializeField] private GameObject ImageLogo;
    [SerializeField] private GameObject GamePlayUI;
    [SerializeField] private AudioSource AxeSound;
    [SerializeField] private AudioSource SunSound;
    [SerializeField] private AudioSource HealthSound;
    [SerializeField] private TMPro.TextMeshProUGUI Score;
    [SerializeField]public List<GameObject> spawnedEnemy;
    [SerializeField]public List<GameObject> spawnedResource;

    [SerializeField] private float fixedTimeEnemy;
    [SerializeField] private float fixedTimeResource;
    [SerializeField] private float percentTimeEnemy;
    [SerializeField] private float percentTimeResource;
    [SerializeField] private float scalePerTime;
    private float enemyTimeSpawn;
    private float resourceTimeSpawn;
    private float scaleTimeSpawn;
    private float screenHeight;
    private float screenWidth;
    private bool isStart;
    private float offsetTime;
    private void Start()
    {
        enemyTimeSpawn = 0;
        resourceTimeSpawn = 0;
        spawnedEnemy = new List<GameObject>();
        spawnedResource = new List<GameObject>();
        var screenSpace = new Vector2(Screen.width, Screen.height);
        isStart = false;
        GamePlayUI.SetActive(false);
        Score.gameObject.SetActive(false);
        buttonStart.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1)
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        // ImageLogo.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1)
        //     .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        UpdateScreenSize(0);
    }

    private void Update()
    {
        var gameTime = Time.realtimeSinceStartup;
        if (!isStart) return;
        if (gameTime - enemyTimeSpawn > fixedTimeEnemy)
        {
            // Debug.Log("Enemy");
            enemyTimeSpawn = gameTime;
            SpawnEnemy(enemyPrefab, ObjectType.ENEMY); 
        }
        
        if (gameTime - resourceTimeSpawn > fixedTimeResource)
        {
            // Debug.Log("Resource");
            resourceTimeSpawn = gameTime;
            SpawnEnemy(resourcePrefab, ObjectType.RESOURCE);
        }

        if (gameTime - scaleTimeSpawn > scalePerTime)
        {
            scaleTimeSpawn = gameTime;
            fixedTimeEnemy *= (1 - percentTimeEnemy);
            fixedTimeResource *= (1 - percentTimeResource);
        }
    }

    public void onClickStart()
    {
        Debug.LogError("IsStart");
        isStart = true;
        buttonStart.SetActive(false);
        ImageLogo.SetActive(false);
        GamePlayUI.SetActive(true);
        offsetTime = enemyTimeSpawn = resourceTimeSpawn = Time.realtimeSinceStartup;
    }

    public void ShowScore()
    {
        Score.gameObject.SetActive(true);
        Score.text = Math.Round(Time.realtimeSinceStartup - offsetTime).ToString();
        Score.gameObject.transform.DOScale(2, 3);
    }

    public void ResetUI()
    {
        Score.gameObject.SetActive(false);
    }
    public void Restart()
    {
        enemyTimeSpawn = 0;
        resourceTimeSpawn = 0;
        spawnedEnemy = new List<GameObject>();
        spawnedResource = new List<GameObject>();
        var screenSpace = new Vector2(Screen.width, Screen.height);
        isStart = false;
        buttonStart.SetActive(true);
        ImageLogo.SetActive(true);
        GamePlayUI.SetActive(false);
        buttonStart.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1)
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        // ImageLogo.gameObject.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1)
        //     .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        UpdateScreenSize(0);
        ResetScreen();
        foreach (var element in spawnedEnemy)
        {
            Destroy(element);   
        }
        foreach (var element in spawnedResource)
        {
            Destroy(element);   
        }
    }

    public void PlayAxeSound()
    {
        if (AxeSound.isPlaying)
        {
            AxeSound.Stop();
        }
        AxeSound.Play();
    }
    
    public void PlaySunSound()
    {
        if (SunSound.isPlaying)
        {
            SunSound.Stop();
        }
        SunSound.Play();
    }
    
    public void PlayHealSound()
    {
        if (HealthSound.isPlaying)
        {
            HealthSound.Stop();
        }
        HealthSound.Play();
    }
    
    private void SpawnEnemy(GameObject enemy, ObjectType objectType)
    {
        var edgePosition = Random.Range(0, 2);

        var newEnemy = Instantiate(enemy, GetSpawnPosition(), Quaternion.identity);
        
        if (objectType == ObjectType.ENEMY)
        {
            spawnedEnemy.Add(newEnemy);
        }
        else
        {
            spawnedResource.Add(newEnemy);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        var edgePositionWidth = Random.Range(0, 2);
        var edgePositionHeight = Random.Range(0, 2);
        if (edgePositionHeight == 0)
        {
            edgePositionHeight = -1;
        }
        // Debug.Log($"Screen: {screenWidth} - {screenHeight} || {edgePositionHeight * screenHeight} || {edgePositionHeight * screenWidth}");
        if (edgePositionWidth == 0)
        {
            return new Vector3(Random.Range(-screenWidth, screenWidth), edgePositionHeight * screenHeight, 0);
        }
        else
        {
            return new Vector3(edgePositionHeight * screenWidth, Random.Range(-screenWidth, screenHeight), 0);
        }
    }

    public void UpdateScreenSize(float scaleValue)
    {
        Debug.LogError($"{Screen.width} || {Screen.height}");
        Camera main = Camera.main;
        if (main != null)
        {
            if (main.orthographicSize > 10) return;

            DOTween.To(() => main.orthographicSize, x => main.orthographicSize = x, main.orthographicSize + scaleValue, 1);

            // main.orthographicSize += scaleValue;
            var workSpace = main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            screenWidth = workSpace.x;
            screenHeight = workSpace.y;
        }
    }

    public void ResetScreen()
    {
        Camera main = Camera.main;
        if (main != null)
        {
            Debug.LogError("VaoZooom");
            DOTween.To(() => main.orthographicSize, x => main.orthographicSize = x, 6, 2);
        }
    }
}
