using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerManager
{
    [HideInInspector] public int playerNumber = 1;
    [HideInInspector] public string coloredPlayerText;
    [HideInInspector] public GameObject playerInstance;
    [HideInInspector] public int wins;
    [HideInInspector] public int score = 0;
    [HideInInspector] public Player player;
    public Color playerColor;
    public Transform spawnPoint;

    private SpriteRenderer spriteRenderer;
    //private GameObject canvasGameObject;

    public void Setup()
    {
        spriteRenderer = playerInstance.GetComponent<SpriteRenderer>();
        player = playerInstance.GetComponent<Player>();
        //canvasGameObject = playerInstance.GetComponentInChildren<Canvas>().gameObject;

        spriteRenderer.color = playerColor;
        coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";

        player.playerManager = this;
        player.playerNumber = playerNumber;
    }

    public void DisableControl()
    {
        player.enabled = false;

        //canvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        player.enabled = true;

        //canvasGameObject.SetActive(true);
    }

    public void IncreaseScore()
    {
        score++;
    }

    public void Reset()
    {
        playerInstance.transform.position = spawnPoint.position;
        playerInstance.transform.rotation = spawnPoint.rotation;

        // need to turn everything off, including winning player, before turning them back on again
        playerInstance.SetActive(false);
        playerInstance.SetActive(true);
    }
}
