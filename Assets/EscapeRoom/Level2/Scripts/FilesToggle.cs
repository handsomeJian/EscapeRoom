using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilesToggle : MonoBehaviour
{

    [SerializeField] private GameObject computerfile; // Assign in inspector
    private bool computerFileState = false;
    // Start is called before the first frame update
    void Start()
    {
        computerfile.SetActive(computerFileState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleComputerFile()
    {
        computerFileState = !computerFileState; 
        computerfile.SetActive(computerFileState);
    }
}
