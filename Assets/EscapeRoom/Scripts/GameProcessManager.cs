using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcessManager : MonoBehaviour
{
    public static GameProcessManager instance;
    public GameObject SceneMesh;
    private bool sceneMeshState = true;

    public GameObject[] AnchorIndicatorList;

    private bool anchorState = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        anchorState = true;
        //UpdateAnchorIndicatorState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            sceneMeshState = !sceneMeshState;
            TriggerSceneMesh(sceneMeshState);
        }
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            anchorState = !anchorState;
            UpdateAnchorIndicatorState(anchorState);
        }
    }

    void UpdateAnchorIndicatorState(bool state)
    {
        foreach (var indicator in AnchorIndicatorList)
        {
            indicator.SetActive(state);
        }
    }

    public void TriggerSceneMesh(bool active)
    {
        SceneMesh.SetActive(active);
    }
}
