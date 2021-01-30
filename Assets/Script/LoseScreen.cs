using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    public string firstLevel, mainMenu;
    public Image blackScreen, retryImage, backImage;
    public GameObject menuFirstButton;
    public bool fadeToBlack, fadeFromBlack, fadeToRetry, fadeToBack;
    public float blackScreenFadeSpeed;
    public Text loseText;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstButton);

        StartCoroutine(LoseCo());
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

        if (fadeToRetry)
        {
            retryImage.color = new Color(retryImage.color.r, retryImage.color.g, retryImage.color.b, Mathf.MoveTowards(retryImage.color.a, 1f, blackScreenFadeSpeed * Time.deltaTime));

            if (retryImage.color.a == 1f)
            {
                fadeToRetry = false;
            }
        }

        if (fadeToBack)
        {
            backImage.color = new Color(backImage.color.r, backImage.color.g, backImage.color.b, Mathf.MoveTowards(backImage.color.a, 1f, blackScreenFadeSpeed * Time.deltaTime));

            if (backImage.color.a == 1f)
            {
                fadeToBack = false;
            }
        }
    }

    IEnumerator RetryCo()
    {
        fadeToBlack = true;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(firstLevel);
    }

    IEnumerator BackToMenuCo()
    {
        fadeToBlack = true;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(mainMenu);
    }

    IEnumerator LoseCo()
    {
        yield return new WaitForSeconds(5f);

        fadeToRetry = true;

        yield return new WaitForSeconds(0.5f);

        fadeToBack = true;
    }

    public void Retry()
    {
        StartCoroutine(RetryCo());
    }

    public void BackToMenu()
    {
        StartCoroutine(BackToMenuCo());
    }
}
