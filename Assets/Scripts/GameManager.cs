using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameOver = false;

    public static bool winCondition = false;

    public static int actualPlayer = 0;

    public List<Controller_Target> targets;

    public List<Controller_Player> players;

    void Start()
    {
        Physics.gravity = new Vector3(0, -30, 0);
        gameOver = false;
        winCondition = false;
        SetConstraits();
    }

    void Update()
    {
        GetInput();
        CheckWin();

    }

    private void CheckWin()
    {
        int i = 0;
        foreach(Controller_Target t in targets)
        {
            if (t.playerOnTarget)
            {
                i++;
                //Debug.Log(i.ToString());
            }
        }
        if (i >= 6)
        {
            winCondition = true;
            //SceneManager.LoadScene(1);
        }
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (actualPlayer <= 0)
            {
                actualPlayer = 7;
                SetConstraits();
            }
            else
            {
                actualPlayer--;
                SetConstraits();
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (actualPlayer >= 7)
            {
                actualPlayer = 0;
                SetConstraits();
            }
            else
            {
                actualPlayer++;
                SetConstraits();
            }
        }
    }

    private void SetConstraits()
    {
        foreach(Controller_Player p in players)
        {
            if (p == players[actualPlayer])
            {
                p.rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                p.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
    }
}
