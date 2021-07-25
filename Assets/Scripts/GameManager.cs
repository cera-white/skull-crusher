using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public BoardManager boardScript;
    public int numRoundsToWin = 5;
    public float startDelay = 3f;
    public float endDelay = 3f;
    public PlayerManager[] players;
    public float timeRemaining = 10;
    public GameObject playerPrefab;
    public Text timerText;
    public Text scoreText;
    public Text messageText;

    private int roundNumber = 0;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private PlayerManager roundWinner;
    private PlayerManager gameWinner;
    private bool timerIsRunning = false;

    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    private void Start()
    {
        timerText.text = ((int)timeRemaining).ToString();

        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnAllPlayers();
        //SetCameraTargets();

        StartCoroutine(GameLoop());
    }

    void Update()
    {
        UpdateScore();

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = ((int)timeRemaining).ToString();
            }
            else
            {
                timeRemaining = 0;
                timerText.text = ((int)timeRemaining).ToString();
                timerIsRunning = false;
            }
        }
    }

    void InitGame()
    {
        // update UI
        boardScript.SetupScene(roundNumber + 1);
    }

    //public void GameOver()
    //{
    //    enabled = false;
    //}

    private void SpawnAllPlayers()
    {
        // loop through Players
        for (int i = 0; i < players.Length; i++)
        {
            players[i].playerInstance = Instantiate(playerPrefab, players[i].spawnPoint.position, Quaternion.identity) as GameObject;
            players[i].playerNumber = i + 1;
            players[i].Setup();
        }
    }

    //private void SetCameraTargets()
    //{
    //    Transform[] targets = new Transform[m_Tanks.Length];

    //    for (int i = 0; i < targets.Length; i++)
    //    {
    //        targets[i] = m_Tanks[i].m_Instance.transform;
    //    }

    //    // add tanks to CameraControl's targets array
    //    m_CameraControl.m_Targets = targets;
    //}

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (gameWinner != null)
        {
            //reload current scene
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        //InitGame();

        ResetAllPlayers();
        DisablePlayerControl();

        //m_CameraControl.SetStartPositionAndSize();

        roundNumber++; // increment round number
        //messageText.text = "ROUND " + roundNumber;
        messageText.text = "START";

        yield return startWait;
    }


    private IEnumerator RoundPlaying()
    {
        timerIsRunning = true;

        EnablePlayerControl();

        messageText.text = string.Empty;

        while (timerIsRunning)
        {
            yield return null; // wait for condition to be false
        }
    }

    private IEnumerator RoundEnding()
    {
        DisablePlayerControl();

        // clear out previous winner
        roundWinner = null;

        roundWinner = GetRoundWinner();

        if (roundWinner != null)
        {
            roundWinner.wins++;
        }

        gameWinner = GetGameWinner();

        string message = EndMessage();
        messageText.text = message;

        yield return endWait;
    }

    private PlayerManager GetRoundWinner()
    {
        PlayerManager currentWinner = null;

        for (int i = 0; i < players.Length; i++)
        {
            //if (players[i].instance.activeSelf)
            //    return players[i];
            if (currentWinner == null || players[i].score > currentWinner.score)
            {
                currentWinner = players[i];
            }
        }

        //return null;
        return currentWinner;
    }

    private PlayerManager GetGameWinner()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].wins == numRoundsToWin)
                return players[i];
        }

        return null;
    }

    private string EndMessage()
    {
        string message = "DRAW!";

        if (roundWinner != null)
            message = roundWinner.coloredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < players.Length; i++)
        {
            message += players[i].coloredPlayerText + ": " + players[i].wins + " WINS\n";
        }

        if (gameWinner != null)
            message = gameWinner.coloredPlayerText + " WINS THE GAME!";

        return message;
    }

    private void ResetAllPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Reset();
        }
    }


    private void EnablePlayerControl()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].EnableControl();
        }
    }


    private void DisablePlayerControl()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].DisableControl();
        }
    }

    public void UpdateScore()
    {
        string text = "";

        for (int i = 0; i < players.Length; i++)
        {
            text += players[i].coloredPlayerText + " Score: " + players[i].score + " ";
        }

        scoreText.text = text;
    }
}
