using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text name1Text;
    public TMP_Text name2Text;
    public TMP_Text score1Text;
    public TMP_Text score2Text;
    public TMP_Text roundOverText;

    public GameObject[] spawnPoints; // Array to hold references to spawn points

    private int player1Score = 0;
    private int player2Score = 0;
    public bool IsRoundOver { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        name1Text.text = PlayerPrefs.GetString("Player1Name", "Player 1");
        name2Text.text = PlayerPrefs.GetString("Player2Name", "Player 2");
        score1Text.text = player1Score.ToString();
        score2Text.text = player2Score.ToString();
        roundOverText.gameObject.SetActive(false);
    }

    public void Player1Scored()
    {
        player1Score++;
        score1Text.text = player1Score.ToString();
        CheckForGameWinner();
    }

    public void Player2Scored()
    {
        player2Score++;
        score2Text.text = player2Score.ToString();
        CheckForGameWinner();
    }

    public void EndRound(string winner)
    {
        Debug.Log("Round Ended. Winner: " + winner);

        string player1Name = PlayerPrefs.GetString("Player1Name", "Player 1");
        string player2Name = PlayerPrefs.GetString("Player2Name", "Player 2");

        string winnerName = (winner == "Player 1") ? player1Name : player2Name;
        roundOverText.text = "Round Over. Winner: " + winnerName;
        roundOverText.gameObject.SetActive(true);
        IsRoundOver = true;

        // Stop the tanks from moving
        TankController[] tanks = FindObjectsOfType<TankController>();
        foreach (TankController tank in tanks)
        {
            tank.enabled = false;
        }

        // Destroy all bullet objects
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }

        // Check if either player has reached 8 points
        if (player1Score >= 8 || player2Score >= 8)
        {
            // Declare the game winner
            string gameWinner = (player1Score >= 8) ? name1Text.text : name2Text.text;
            roundOverText.text = "Game Winner: " + gameWinner;

            // Start the coroutine to return to the main menu after 5 seconds
            StartCoroutine(ReturnToMainMenu());
        }
        else
        {
            // Start the countdown for the next round
            StartCoroutine(NextRoundCountdown());
        }
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameStartMenu");
    }


    private void CheckForGameWinner()
    {
        if (player1Score >= 8)
        {
            string player1Name = PlayerPrefs.GetString("Player1Name", "Player 1");
            roundOverText.text = "Game Winner: " + player1Name;
            roundOverText.gameObject.SetActive(true);
            IsRoundOver = true;
            StartCoroutine(RestartGameAfterDelay());
        }
        else if (player2Score >= 8)
        {
            string player2Name = PlayerPrefs.GetString("Player2Name", "Player 2");
            roundOverText.text = "Game Winner: " + player2Name;
            roundOverText.gameObject.SetActive(true);
            IsRoundOver = true;
            StartCoroutine(RestartGameAfterDelay());
        }
    }

    private IEnumerator NextRoundCountdown()
    {
        yield return new WaitForSeconds(3f);

        roundOverText.gameObject.SetActive(false);
        ResetRound();
    }
    //RESET ROUND
    private void ResetRound()
    {
        // Reset the round status
        IsRoundOver = false;

        // Randomly select spawn points for both tanks
        GameObject spawnPoint1 = GetRandomSpawnPoint();
        GameObject spawnPoint2 = GetDifferentSpawnPoint(spawnPoint1);

        // Set tank positions
        TankController[] tanks = FindObjectsOfType<TankController>();
        tanks[0].transform.position = spawnPoint1.transform.position;
        tanks[1].transform.position = spawnPoint2.transform.position;

        // Enable movement for tanks
        foreach (TankController tank in tanks)
        {
            tank.ResetPosition();
            tank.enabled = true;
        }
    }
    //RESET GAME
    private IEnumerator RestartGameAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameStartMenu");
    }

    private GameObject GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    private GameObject GetDifferentSpawnPoint(GameObject spawnPoint1)
    {
        GameObject spawnPoint2;
        do
        {
            spawnPoint2 = GetRandomSpawnPoint();
        } while (spawnPoint2 == spawnPoint1);

        return spawnPoint2;
    }
}
