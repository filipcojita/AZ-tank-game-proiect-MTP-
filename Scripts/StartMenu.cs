using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour
{
    public TMP_InputField player1NameInput;
    public TMP_InputField player2NameInput;
    public AudioSource audioSource;
    public AudioClip backgroundMusic;

    private void Start()
    {
        // Play the background music
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StartGame()
    {
        Debug.Log("GameStartMenu started.");

        // Get the player names entered in the input fields
        string player1Name = player1NameInput.text;
        string player2Name = player2NameInput.text;

        // Save player names
        PlayerPrefs.SetString("Player1Name", player1Name);
        PlayerPrefs.SetString("Player2Name", player2Name);

        // Load the main game scene
        SceneManager.LoadScene("MainGameScene");
    }
}
