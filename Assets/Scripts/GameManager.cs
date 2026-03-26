using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Over")]
    public GameObject gameOverPanel;

    [Header("Players")]
    public PlayerHealth[] players;
    public Transform[] livesPanels;

    [Header("Life settings")]
    public GameObject lifeIconPrefab;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    float spacing = 60f;
    private int[] lastKnownLives;
    private List<GameObject>[] lifeIcons;
    private bool gameOverTriggered = false; //en flagga för att säkerställa att game over-sekvensen bara triggas en gång

    void Start()
    {
        lastKnownLives = new int[players.Length];
        lifeIcons = new List<GameObject>[players.Length];

        for (int p = 0; p < players.Length; p++) //loopar genom alla spelare och skapar livsikoner för varje spelare
        {
            lifeIcons[p] = new List<GameObject>(); //skapar en ny lista för att hålla livsikonerna för varje spelare
            lastKnownLives[p] = players[p].lives; //sätter lastKnownLives för varje spelare till deras nuvarande antal liv

            for (int i = 0; i < players[p].lives; i++) //loopar genom antalet liv som spelaren har och skapar en ikon för varje liv
            {
                GameObject icon = Instantiate(lifeIconPrefab, livesPanels[p]);
                icon.transform.localScale = Vector3.one;
                icon.transform.localPosition = new Vector3(i * spacing, 0, 0);
                lifeIcons[p].Add(icon);
            }
        }

        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); //avslutar spelet när Escape-tangenten trycks ned
        }
        UpdateUI(); //uppdaterar UI varje frame för att visa den aktuella hälsan

        if (gameOverPanel.activeSelf)
        {
            if (Input.anyKeyDown) //kontrollerar om någon tangent har tryckts ned när game over-panelen är aktiv
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void UpdateUI()
    {
        Debug.Log("UpdateUI körs, gameOverTriggered: " + gameOverTriggered); 
        for (int p = 0; p < players.Length; p++) //loopar genom alla spelare och uppdaterar deras livsikoner
        {
            if (players[p] != null && players[p].gameObject.activeSelf) lastKnownLives[p] = players[p].lives; //om playerHealth är null, uppdatera lastKnownLives till spelarens nuvarande antal liv

            for (int i = 0; i < lifeIcons[p].Count; i++) //loopar genom alla livsikoner och uppdaterar deras sprite baserat på spelarens aktuella hälsa
            {
                Image img = lifeIcons[p][i].GetComponent<Image>();
                img.sprite = i < lastKnownLives[p] ? fullHeart : emptyHeart; //om indexet är mindre än spelarens liv, sätt ikonen till fullHeart, annars sätt den till emptyHeart
            }
        }

        if (!gameOverTriggered) //kontrollerar om game over-sekvensen inte redan har triggas
        {
            for (int p = 0; p < players.Length; p++) //loopar genom alla spelare och kontrollerar om någon av dem har förlorat alla liv
            {
                Debug.Log("Player " + p + " lastKnownLives: " + lastKnownLives[p] + " activeSelf: " + (players[p] != null ? players[p].gameObject.activeSelf.ToString() : "NULL"));
                if (lastKnownLives[p] <= 0) //kontrollerar om spelaren har förlorat alla liv och om game over-panelen inte redan är aktiv
                {
                    Debug.Log("Game over triggad!");
                    gameOverTriggered = true;
                    DisableAllPlayers();
                    StartCoroutine(ShowGameOverPanel()); //startar en coroutine för att visa game over-panelen efter en kort fördröjning
                    break;
                }
            }
        }
    }

    void DisableAllPlayers()
    {
        foreach (PlayerHealth player in players) //loopar genom alla spelare och inaktiverar deras GameObjects för att dölja dem när game over-sekvensen startar
        {
            if (player != null)
            {
                player.GetComponent<MovementController>().enabled = false;
                player.GetComponent<BombController>().enabled = false; //inaktiverar bombkontrollen för att förhindra att spelaren kan placera bomber under game over-sekvensen
            }
        }
    }
    System.Collections.IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f); //väntar i 1,5 sekund innan game over-panelen visas
        gameOverPanel.SetActive(true); //om spelaren har förlorat alla liv, visa game over-panelen
    }
}
