using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnterUsername : MonoBehaviour
{
    public GameObject warning;
    public GameObject gameMenu;
    public GameObject userInputMenu;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI nameTitle;

    public void OnSubmit()
    {
        var inputString = nameInputField.text;
        if (inputString.Length < 3 || inputString.Length > 14 ||inputString.Contains(" "))
        {
            warning.SetActive(true);
        }
        else
        {
            SQLiteHelper.InsertPlayer(inputString);
            gameMenu.SetActive(true);
            userInputMenu.SetActive(false);
            PlayerInfo.Username = inputString;
            nameTitle.text = inputString;
        }
        
    }
}
