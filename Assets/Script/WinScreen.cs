using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public string mainMenu;
    public Image blackScreen, backImage;
    public GameObject menuFirstButton;
    public bool fadeToBlack, fadeFromBlack, fadeToBack;
    public float blackScreenFadeSpeed;
    public Text winText;
    public int pauseSound, selectSound, activateSound;

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

        if (fadeToBack)
        {
            backImage.color = new Color(backImage.color.r, backImage.color.g, backImage.color.b, Mathf.MoveTowards(backImage.color.a, 1f, blackScreenFadeSpeed * Time.deltaTime));

            if (backImage.color.a == 1f)
            {
                fadeToBack = false;
            }
        }
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

        fadeToBack = true;
    }

    public void ButtonSelected()
    {
        AudioManager.instance.PlaySFX(selectSound);
    }

    public void ButtonActivated()
    {
        AudioManager.instance.PlaySFX(activateSound);
    }

    public void BackToMenu()
    {
        StartCoroutine(BackToMenuCo());
    }
}
