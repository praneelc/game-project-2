using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public InputActionMap inputActions;

    // Start is called before the first frame update
    void Start()
    {
        inputActions["ClickStart"].performed += _ => StartGame();
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
