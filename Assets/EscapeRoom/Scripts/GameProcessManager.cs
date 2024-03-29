using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcessManager : MonoBehaviour
{
    public static GameProcessManager instance;
    public GameObject SceneMesh;
    private bool sceneMeshState = true;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            sceneMeshState = !sceneMeshState;
            TriggerSceneMesh(sceneMeshState);
        }
    }

    public void TriggerSceneMesh(bool active)
    {
        SceneMesh.SetActive(active);
    }
}
