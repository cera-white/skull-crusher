using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public int playerNumber = 1;
    [HideInInspector] public string horizontalAxisName;
    [HideInInspector] public string verticalAxisName;

    private Animator animator;
    // private int bombs;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        //food = GameManager.instance.playerFoodPoints;

        //foodText.text = "Food: " + food;

        horizontalAxisName = "Horizontal" + playerNumber;
        verticalAxisName = "Vertical" + playerNumber;

        base.Start();
    }

    //private void OnDisable()
    //{
    //    // GameManager.instance.playerFoodPoints = food;
    //}

    // Update is called once per frame
    void Update()
    {
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw(horizontalAxisName);
        vertical = (int)Input.GetAxisRaw(verticalAxisName);

        // can't move diagonally
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Boulder>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);

        //RaycastHit2D hit;

        //if (Move(xDir, yDir, out hit))
        //{
        //    SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        //}

        //CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.tag == "Exit")
        //{
        //    Invoke("Restart", restartLevelDelay);
        //    enabled = false;
        //}
        //else if (other.tag == "Bomb")
        //{
        //    bombs++;
        //    //foodText.text = "+" + pointsPerFood + " Food: " + food;
        //    //SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
        //    other.gameObject.SetActive(false);
        //}
    }

    protected override void OnCantMove<T>(T component)
    {
        Boulder hitBoulder = component as Boulder;
        hitBoulder.Move(playerManager);

        // animator.SetTrigger("playerChop");
    }

    //private void Restart()
    //{
    //    //Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
    //    //and not load all the scene object in the current scene.
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    //}

    //private void CheckIfGameOver()
    //{
    //    // if (food <= 0)
    //    // {
    //        // SoundManager.instance.PlaySingle(gameOverSound);
    //        // SoundManager.instance.musicSource.Stop();
    //        // GameManager.instance.GameOver();
    //    // }
    //}
}
