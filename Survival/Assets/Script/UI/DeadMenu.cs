using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    public GameObject deadMenuUI;
    public PlayerStats playerStats;

    private void Update()
    {
        if (playerStats.isDead)
        {
            deadMenuUI.SetActive(true);
        }
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
