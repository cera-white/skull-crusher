using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public float thrust = 5.0f;

    private Rigidbody2D rb2D;
    private PlayerManager hitBy = null;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Move(PlayerManager hitter)
    {
        hitBy = hitter;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw(hitBy.player.horizontalAxisName);
        vertical = (int)Input.GetAxisRaw(hitBy.player.verticalAxisName);

        // can't move diagonally
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0)
        {
            rb2D.AddForce(transform.right * thrust, ForceMode2D.Impulse);
        }
        else if (vertical != 0)
        {
            rb2D.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            gameObject.SetActive(false);

            if (hitBy != null)
            {
                hitBy.IncreaseScore();
            }
        }
        else if (other.tag == "Moveable")
        {
            Boulder boulder = other.GetComponent<Boulder>();
            if (boulder != null && boulder.hitBy != null)
            {
                hitBy = boulder.hitBy;
            }
        }
    }
}
