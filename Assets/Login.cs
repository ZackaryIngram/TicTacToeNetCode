using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{

    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;
    public Button goToRegisterButton;

    ArrayList credentials;

    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(loggingIn);
        goToRegisterButton.onClick.AddListener(moveToRegisterScene);

        //Look for correct file, if it does load it into the arraylist, otherwise print out error message.
        if (File.Exists(Application.dataPath + "/accountInfo.txt"))
        {
            credentials = new ArrayList(File.ReadAllLines(Application.dataPath + "/accountInfo.txt"));
        }
        else
        {
            Debug.Log("accountInfo file doesn't exist");
        }

    }



    // Update is called once per frame
    void loggingIn()
    {
        bool isExists = false;

        credentials = new ArrayList(File.ReadAllLines(Application.dataPath + "/accountInfo.txt"));

        //Check if user name and password exists in the file, if so load the gameplayscene
        foreach (var i in credentials)
        {
            string line = i.ToString();
            //Debug.Log(line);
            //Debug.Log(line.Substring(11));
            //substring 0-indexof(:) - indexof(:)+1 - i.length-1
            if (i.ToString().Substring(0, i.ToString().IndexOf(":")).Equals(usernameInput.text) &&
                i.ToString().Substring(i.ToString().IndexOf(":") + 1).Equals(passwordInput.text))
            {
                isExists = true;
                break;
            }
        }

        if (isExists)
        {
            Debug.Log($"Logging in '{usernameInput.text}'");
            MoveToGameplayScene();
        }
        else
        {
            Debug.Log("Incorrect account information");
        }
    }

    void moveToRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }

    void MoveToGameplayScene()
    {
        SceneManager.LoadScene("TicTacToeScene");
    }
}