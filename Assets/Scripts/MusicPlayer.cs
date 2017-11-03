using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    static GameObject music = null;

    void Awake()
    {

        // the following prevents duplicates of music player from instantiating and prevents the first instance from being destroyed 
        // in subsequent scenes

        // if an instance of music player already exists, destroy the newly instantiated instance of music player
        if (music)
        {
            Destroy(gameObject);
        }
        else
        {
            // keep the instance of music player and prevent it from destroying in  a new level if no instance if it exists yet
            music = gameObject;
            DontDestroyOnLoad(music);

        }
    }

    
}
