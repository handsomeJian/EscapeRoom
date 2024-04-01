using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteractionManager : MonoBehaviour
{
    public GameObject CaseObject;

    public GameObject HandObject;

    Renderer caseRenderer;

    public float HandRadius = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        caseRenderer = CaseObject.GetComponent<Renderer>();
        caseRenderer.material.SetFloat("_Radius", HandRadius);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = HandObject.transform.position;
        caseRenderer.material.SetVector("_HandPos", new Vector4(pos.x, pos.y, pos.z, 0));
    }
}
