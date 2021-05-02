using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Dont destroy audio over scene change
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
