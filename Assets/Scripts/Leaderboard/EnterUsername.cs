using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnterUsername : MonoBehaviour
{
    public GameObject warning;
    public TMP_InputField nameInputField;

    public bool SubmitUsername()
    {
        var inputString = nameInputField.text.ToLower();
        if (inputString.Length < 3 || inputString.Length > 14 ||inputString.Contains(" "))
        {
            warning.SetActive(true);
            return false;
        }
        SQLiteHelper.InsertPlayer(inputString);
        PlayerInfo.Username = inputString;
        return true;
    }
}
