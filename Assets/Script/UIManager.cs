using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image blackScreen;
    public float blackScreenFadeSpeed = 0.5f;
    public bool fadeToBlack, fadeFromBlack;
    public GameObject pauseScreen;
    public GameObject pauseFirstButton;

    //public Image mask, fill;
    public float distanceBetweenObjects;
    //public float maxDistance, minDistance, curDistance;
    public GameObject obj1, obj2;
    public GameObject rider, head;

    public string titleScreen;

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

        //GetCurrentDistanceToHead();
        Debug.DrawLine(obj1.transform.position, obj2.transform.position, Color.green);
        distanceBetweenObjects = Vector3.Distance(obj1.transform.position, obj2.transform.position);
        //curDistance = distanceBetweenObjects;
        rider.transform.position = obj1.transform.position;
        head.transform.position = obj2.transform.position;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        if (pauseScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }

        else
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
    }

    public void Resume()
    {
        PauseUnpause();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(titleScreen);
        Time.timeScale = 1f;
    }

    /*private void GetCurrentDistanceToHead()
    {
        float currentOffset = curDistance - minDistance;
        float maximumOffset = maxDistance - minDistance;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;
    }*/
}
