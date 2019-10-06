using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI {
    public class GeneralMixerManager : MonoBehaviour
    {
        public AudioMixer mixer;

        public Slider sliderMaster;
        public Slider sliderMusic;
        public Slider sliderSfx;

        private void Start()
        {
            mixer.GetFloat("MasterVolume", out var value);
            sliderMaster.value = value;
            mixer.GetFloat("MusicVolume", out var value2);
            sliderMusic.value = value2;
            mixer.GetFloat("SoundEffectsVolume", out var value3);
            sliderSfx.value = value3;
        }
        
        public void ChangeGeneral()
        {
            mixer.SetFloat("MasterVolume", sliderMaster.value);
        }
        
        public void ChangeMusic()
        {
            mixer.SetFloat("MusicVolume", sliderMusic.value);
        }
        
        public void ChangeSfx()
        {
            mixer.SetFloat("SoundEffectsVolume", sliderSfx.value);
        }
    }
}