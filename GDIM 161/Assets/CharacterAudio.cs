using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public AudioSource src;
    public AudioClip dmg1, dmg2, dmg3, dmg4;
    private AudioClip dmgToUse;

    private void newDmg()
    {
        switch(Random.Range(1, 5))
        {
            case 1:
                dmgToUse = dmg1;
                break;
            case 2:
                dmgToUse = dmg2;
                break;
            case 3:
                dmgToUse = dmg3;
                break;
            case 4:
                dmgToUse = dmg3;
                break;
        }
    }

    public void playDmg()
    {
        newDmg();
        src.PlayOneShot(dmgToUse, 1.2f);
    }

    public void playDeath()
    {
        src.Play();
    }
}
