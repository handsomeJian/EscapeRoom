using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.Depth;

public class DepthManager : MonoBehaviour
{
    private EnvironmentDepthTextureProvider _depthTextureProvider;

    private void Awake()
    {
        _depthTextureProvider = GetComponent<EnvironmentDepthTextureProvider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _depthTextureProvider.RemoveHands(true);
    }

    private void Update()
    {
        _depthTextureProvider.RemoveHands(true);
    }
}
