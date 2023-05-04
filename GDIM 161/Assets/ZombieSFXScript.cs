using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used as a soundboard, just link this script in the behavior script and call
// the corresponding sound
public class ZombieSFXScript : MonoBehaviour
{
    public AudioSource src;
    public AudioClip idle1, idle2, idle3, chase1, chase2, chase3, death1, death2;
    private float randNum;
    // public float idleVol;

    // every instance should have slightly unique pitch lol
    // UNIQUE VOICES LOL
    private void Start()
    {
        randNum = Random.Range(-1.0f, 1.0f);
        src.pitch += randNum;
    }
    
    // each will play random sound
    public void idle()
    {
        // src.volume = idleVol; for if I need to adjust the volume of each clip

        if (!src.isPlaying)
        {
            randNum = (int)Random.Range(1, 4);
            switch(randNum)
            {
                case 1: 
                    src.clip = idle1;
                    src.Play();
                    break;
                case 2: 
                    src.clip = idle2;
                    src.Play();
                    break;
                case 3: 
                    src.clip = idle3;
                    src.Play();
                    break;
            }
        }
    }

    public void chase()
    {
        if (!src.isPlaying)
        {
            randNum = Random.Range(1, 4);
            switch(randNum)
            {
                case 1: 
                    src.clip = chase1;
                    src.Play();
                    break;
                case 2: 
                    src.clip = chase2;
                    src.Play();
                    break;
                case 3: 
                    src.clip = chase3;
                    src.Play();
                    break;
            }
        }
    }

    public void death()
    {
        if (!src.isPlaying)
        {
            randNum = Random.Range(1, 3);
            switch(randNum)
            {
                case 1: 
                    src.clip = death1;
                    src.Play();
                    break;
                case 2: 
                    src.clip = death2;
                    src.Play();
                    break;
            }
        }
    }
}
