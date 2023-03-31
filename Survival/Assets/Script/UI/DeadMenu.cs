using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    public GameObject deadMenuUI;
    public static bool IsDead;
    public PlayerStats playerStats;

    private void Update()
    {
        if (playerStats.isDead)
        {
            IsDead = true;
            deadMenuUI.SetActive(true);
        }
        else
        {
            IsDead = false;
        }
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
