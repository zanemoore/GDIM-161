using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Slider))]
public class SettingsSliders : MonoBehaviour
{
    Slider slider
    {
        get { return GetComponent<Slider>(); }
    }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string volumeName;
    // Start is called before the first frame update

    public void UpdateValueOnChange(float value)
    {
        mixer.SetFloat(volumeName, value);
    }
}
