using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    public TextMeshProUGUI timeText;
    public GameObject startGUI;
    public GameObject gameGUI;
    public GameObject gameOverGUI;


    public List<GameObject> targetPrefabs;

    public bool isGameActive;
    private int roundDuration = 60;

    private bool isSpawning = false;
    private Coroutine spawningCoroutine;
    private Vector3 spawnerPosition = new Vector3(0, 12, 0);
    private float spawnerSpeed = 5f;
    private float spawnerDirection = 1f; // 1 per movimento verso destra, -1 per movimento verso sinistra


    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        isGameActive = true;
        startGUI.SetActive(false);
        gameGUI.SetActive(true);
        StartCoroutine(UpdateTimer(roundDuration + 1));

        isSpawning = true;
        spawnerDirection = 1f; // Inizialmente il movimento va verso destra
        spawningCoroutine = StartCoroutine(SpawnTarget());
        StartCoroutine(MoveSpawner());
    }

    private IEnumerator SpawnTarget()
    {
        GameObject currentTarget = null; // Memorizza l'istanza corrente del target

        while (isGameActive)
        {
            if (currentTarget == null)
            {
                // Se non c'è un target attualmente istanziato, crea uno nuovo
                GameObject randomTargetPrefab = targetPrefabs[Random.Range(0, targetPrefabs.Count)];
                currentTarget = Instantiate(randomTargetPrefab, spawnerPosition, Quaternion.identity);
            }

            // Muovi il target insieme al "spawner"
            currentTarget.transform.position = spawnerPosition;

            // Se l'utente preme spazio, rilascia il target e lo rende disponibile per un nuovo spawn
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentTarget = null;
            }

            yield return null;
        }
    }


    private IEnumerator MoveSpawner()
    {
        while (isSpawning)
        {
            // Muovi il "spawner" lungo l'asse X
            spawnerPosition.z += spawnerSpeed * spawnerDirection * Time.deltaTime;

            // Se il "spawner" raggiunge un limite, cambia direzione
            if (spawnerPosition.z >= 14 || spawnerPosition.z <= -14)
            {
                spawnerDirection *= -1f; // Cambio di direzione
            }

            // Aggiorna la posizione del "spawner"
            // L'asse Y rimane costante a 12, poiché il "spawner" si muove solo sull'asse X
            spawnerPosition.y = 12;

            yield return null;
        }
    }

    IEnumerator UpdateTimer(int timeToSet)
    {
        while (isGameActive && roundDuration != 0)
        {
            yield return new WaitForSeconds(1);
            timeToSet--;
            timeText.text = "Time: " + timeToSet;

            if (timeToSet <= 0)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        gameOverGUI.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
