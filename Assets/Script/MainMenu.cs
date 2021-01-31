using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public Image blackScreen;
    public GameObject menuFirstButton, creditsFirstButton;
    public bool fadeToBlack, fadeFromBlack;
    public float blackScreenFadeSpeed;
    public int pauseSound, selectSound, activateSound;
    public GameObject creditsScreen;
    private bool creditsOpen, creditsClosed;
    public GameObject mouseDisable;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstButton);
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
    }

    IEnumerator StartGameCo()
    {
        fadeToBlack = true;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(firstLevel);
    }

    IEnumerator CreditsCo()
    {
        if (creditsScreen.activeInHierarchy)
        {
            fadeToBlack = true;
            yield return new WaitForSeconds(1.75f);
            creditsScreen.SetActive(false);
            yield return new WaitForSeconds(1.75f);
            fadeFromBlack = true;

            creditsScreen.SetActive(false);
            mouseDisable.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(menuFirstButton);
        }

        else
        {
            fadeToBlack = true;
            yield return new WaitForSeconds(1.75f);
            creditsScreen.SetActive(true);
            yield return new WaitForSeconds(1.75f);
            fadeFromBlack = true;

            creditsScreen.SetActive(true);
            mouseDisable.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(creditsFirstButton);
        }
    }

    IEnumerator QuitCo()
    {
        fadeToBlack = true;

        yield return new WaitForSeconds(3f);

        Application.Quit();
    }

    public void ButtonSelected()
    {
        AudioManager.instance.PlaySFX(selectSound);
    }

    public void ButtonActivated()
    {
        AudioManager.instance.PlaySFX(activateSound);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCo());
    }

    public void OpenCloseCredits()
    {
        StartCoroutine(CreditsCo());
    }

    public void QuitGame()
    {
        StartCoroutine(QuitCo());
    }
}
