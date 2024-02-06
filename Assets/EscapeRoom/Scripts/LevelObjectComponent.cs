using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Oculus.Interaction;

public class LevelObjectComponent : MonoBehaviour
{
    public string ObjectName;

    public bool hasAnchor = false;
    public Guid AnchorID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnObjectStartMoving(PointerEvent evt)
    {
        LevelObjectManager.Instance.RemoveSpatialAnchor(this);
    }

    public void OnObjectEndMoving(PointerEvent evt)
    {
        LevelObjectManager.Instance.CreateSpatialAnchor(this);
    }
}
