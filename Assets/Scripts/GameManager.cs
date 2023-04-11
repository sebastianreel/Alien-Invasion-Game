using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemiesLeftText;
    
    List<Enemy> enemies = new List<Enemy>();
    List<PlayerController> players = new List<PlayerController>();
    
    [SerializeField] GameObject winTextObject;
    [SerializeField] AudioSource winSoundEffect;
    [SerializeField] GameObject loseTextObject;
    [SerializeField] AudioSource loseSoundEffect;
    [SerializeField] GameObject backToMenuWin;
    [SerializeField] GameObject backToMenuLose;
    [SerializeField] GameObject nextLevelButton;



    private void Start()
    {
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        backToMenuWin.SetActive(false);
        backToMenuLose.SetActive(false);
        nextLevelButton.SetActive(false);

    }

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += HandleEnemyDefeated;
        PlayerController.OnPlayerKilled += HandlePlayerDefeated;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= HandleEnemyDefeated;
        PlayerController.OnPlayerKilled -= HandlePlayerDefeated;

    }

    private void Awake()
    {
        enemies = GameObject.FindObjectsOfType<Enemy>().ToList();
        UpdateEnemiesLeftText();
    }

    void HandleEnemyDefeated(Enemy enemy)
    {
        if (enemies.Remove(enemy))
        {
           UpdateEnemiesLeftText();
        }

        if (enemies.Count == 0)
        {
            winTextObject.SetActive(true);
            backToMenuWin.SetActive(true);
            nextLevelButton.SetActive(true);
            winSoundEffect.Play();
        }
    }

    void HandlePlayerDefeated(PlayerController player)
    {
        if(players.Count == 0)
        {
            loseTextObject.SetActive(true);
            backToMenuLose.SetActive(true);
            loseSoundEffect.Play();
        }
    }

    void UpdateEnemiesLeftText()
    {
        enemiesLeftText.text = $"Enemies Left: {enemies.Count}";
    }
}
