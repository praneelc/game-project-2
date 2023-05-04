using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public InputActionMap inputActions;

    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;

    // Start is called before the first frame update
    void Start()
    {
        inputActions["ClickStart"].performed += _ => StartGame();

        int s = 0;
        int hs = 0;

        if (PlayerPrefs.HasKey("Score"))
        {
            s = PlayerPrefs.GetInt("Score");
        } else
        {
            PlayerPrefs.SetInt("Score", 0);
        }

        if (PlayerPrefs.HasKey("HighScore"))
        {
            hs = Mathf.Max(s, PlayerPrefs.GetInt("HighScore"));
        } else
        {
            PlayerPrefs.SetInt("HighScore", Mathf.Max(0, s));
        }

        StartCoroutine(AnimateScores(s, hs));
    }

    private IEnumerator AnimateScores(int sc, int hsc)
    {
        int s = 0;
        int hs = 0;

        while (s < sc)
        {
            s++;
            hs++;

            score.SetText("" + s);
            highScore.SetText("" + hs);

            yield return new WaitForSeconds(Mathf.Clamp(3 / hsc, .01f, .1f));
        }

        while (hs < hsc)
        {
            hs++;
            highScore.SetText("" + hs);

            yield return new WaitForSeconds(Mathf.Clamp(3/hsc, .01f, .1f));
        }


    }


    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Debug.Log("start game clicked");
        SceneManager.LoadScene("GameplayScene");
    }
}
