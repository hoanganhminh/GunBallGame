using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explose : MonoBehaviour
{
    
    public AudioSource audioSource;

    public AudioClip explosesound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.Play();
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
