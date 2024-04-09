using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] BGMList;

    AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM(int index)
    {
        m_audioSource.Pause();
        m_audioSource.clip = BGMList[index];
        m_audioSource.Play();
    }
}
