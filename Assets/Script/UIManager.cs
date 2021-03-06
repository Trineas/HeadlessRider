﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image blackScreen, eyeGlow;
    public float blackScreenFadeSpeed = 0.5f;
    public float eyeGlowFadeSpeed = 1.5f;
    public bool fadeToBlack, fadeFromBlack, fadeToGlow, fadeFromGlow;
    public GameObject pauseScreen;
    public GameObject pauseFirstButton;
    public float distanceBetweenObjects;
    public GameObject rider, head;
    public Slider distanceSlider;
    public string mainMenu, loseScreen, winScreen;
    public GameObject mouseDisable;
    public int pauseSound, selectSound, activateSound;

    private void Awake()
    {
        instance = this;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (fadeToBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, blackScreenFadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if (fadeFromBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, blackScreenFadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 0f)
            {
                fadeFromBlack = false;
            }
        }

        if (fadeToGlow)
        {
            eyeGlow.color = new Color(eyeGlow.color.r, eyeGlow.color.g, eyeGlow.color.b, Mathf.MoveTowards(eyeGlow.color.a, 1f, eyeGlowFadeSpeed * Time.deltaTime));

            if (eyeGlow.color.a == 1f)
            {
                fadeToGlow = false;
            }
        }

        if (fadeFromGlow)
        {
            eyeGlow.color = new Color(eyeGlow.color.r, eyeGlow.color.g, eyeGlow.color.b, Mathf.MoveTowards(eyeGlow.color.a, 0f, eyeGlowFadeSpeed * Time.deltaTime));

            if (eyeGlow.color.a == 0f)
            {
                fadeFromGlow = false;
            }
        }

        Debug.DrawLine(rider.transform.position, head.transform.position, Color.red);
        distanceBetweenObjects = Vector3.Distance(rider.transform.position, head.transform.position);
        distanceSlider.value = distanceBetweenObjects;

        if (distanceSlider.value <= 3.5f)
        {
            fadeToGlow = true;
        }
        else if (distanceSlider.value >= 6f)
        {
            fadeFromGlow = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            PauseUnpause();
        }

        if (rider.GetComponent<PlayerController>().playerDied())
        {
            StartCoroutine(LoseTransitionCo());
        }

        if (rider.GetComponent<PlayerController>().playerWon())
        {
            StartCoroutine(WinTransitionCo());
        }
    }

    IEnumerator BackToMenuCo()
    {
        Time.timeScale = 1f;

        fadeToBlack = true;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(mainMenu);
    }

    IEnumerator LoseTransitionCo()
    {
        fadeToBlack = true;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(loseScreen);
    }

    IEnumerator WinTransitionCo()
    {
        fadeToBlack = true;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(winScreen);
    }

    public void PauseUnpause()
    {
        if (pauseScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(false);
            mouseDisable.SetActive(false);
            AudioManager.instance.wagon.UnPause();
            AudioManager.instance.bgm.UnPause();
            Time.timeScale = 1f;
        }

        else
        {
            AudioManager.instance.PlaySFX(pauseSound);
            pauseScreen.SetActive(true);
            mouseDisable.SetActive(true);
            AudioManager.instance.wagon.Pause();
            AudioManager.instance.bgm.Pause();
            Time.timeScale = 0f;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
    }

    public void ButtonSelected()
    {
        AudioManager.instance.PlaySFX(selectSound);
    }

    public void ButtonActivated()
    {
        AudioManager.instance.PlaySFX(activateSound);
    }

    public void Resume()
    {
        PauseUnpause();
    }

    public void MainMenu()
    {
        StartCoroutine(BackToMenuCo());
    }
}
