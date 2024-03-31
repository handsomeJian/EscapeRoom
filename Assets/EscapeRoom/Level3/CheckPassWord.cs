using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckPassWord : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_InputField userInputField; // Assign your InputField in the inspector
    [SerializeField] private Button escapeButton, closeBotton; // Assign your Button in the inspector
    [SerializeField] private string correctString = "handsomeJJ";
    [SerializeField] private GameObject warning;
    void Start()
    {
        escapeButton.onClick.AddListener(CheckInput);
        closeBotton.onClick.AddListener(CloseWarning);
    }


    void CheckInput()
    {

        string userInput = userInputField.text;


        if (userInput.Equals(correctString))
        {
            Debug.Log("Input matches the specific string!");
            
        }
        else
        {
            Debug.Log("Input does not match.");
            userInputField.text = "";
            warning.SetActive(true);

        }
    }
    void CloseWarning()
    {
        warning.SetActive(false);
    }
}
