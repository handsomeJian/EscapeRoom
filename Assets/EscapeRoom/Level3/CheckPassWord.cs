using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices.ComTypes;

public class CheckPassWord : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_InputField userInputField; // Assign your InputField in the inspector
    [SerializeField] private Button escapeButton, closeBotton; // Assign your Button in the inspector
    [SerializeField] private string correctString = "handsomeJJ";
    [SerializeField] private GameObject warning;
    [SerializeField] private AudioSource wrongBuzz;
    [SerializeField] private RectTransform targetArea1, targetArea2; // The area where the click should not trigger the audio
    public UdpSocketSender sender;
    private bool isArea1;
    void Start()
    {
        isArea1 = true;
        escapeButton.onClick.AddListener(CheckInput);
        closeBotton.onClick.AddListener(CloseWarning);
    }

    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0)&& isArea1)//&& !IsPointerOverUIObject()
        {
            // Check if the click is outside the target area
            if (!RectTransformUtility.RectangleContainsScreenPoint(targetArea1, Input.mousePosition))
            {
                // Play the audio
                wrongBuzz.Play();
            }
        }
        else if (Input.GetMouseButtonDown(0) && !isArea1)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(targetArea2, Input.mousePosition))
            {
                // Play the audio
                wrongBuzz.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            sender.SendData("Failed");
        }
    }
    void CheckInput()
    {

        string userInput = userInputField.text;


        if (userInput.Equals(correctString))
        {
            isArea1 = true;
            Debug.Log("Input matches the specific string!");
            sender.SendData("Success");
            
        }
        else
        {
            isArea1 = false;
            Debug.Log("Input does not match.");
            userInputField.text = "";
            warning.SetActive(true);
            wrongBuzz.Play();
            sender.SendData("Failed");
        }
    }
    void CloseWarning()
    {
        warning.SetActive(false);
        isArea1 = true;
    }
}
