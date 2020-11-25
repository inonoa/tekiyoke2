using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace SoundOrMusic
{
    [CreateAssetMenu(fileName = "Sound Volume Changer", menuName = "Scriptable Object/Sound Volume Changer", order = 0)]
    public class SoundVolumeChanger : ScriptableObject
    {
        const float MinVolume = -30;
        const float MaxVolume = 20;
        
        [SerializeField] [Range(MinVolume, MaxVolume)] float _SEVolume  = 0;
        [SerializeField] [Range(MinVolume, MaxVolume)] float _BGMVolume = 0;

        [SerializeField] AudioMixer mixer;
        
        [Button]
        void Apply()
        {
            mixer.SetFloat("SEVolume",  _SEVolume);
            mixer.SetFloat("BGMVolume", _BGMVolume);
        }
        
        public void ChangeSEVolume(float volume)
        {
            float volumeActual = Mathf.Lerp(MinVolume, MaxVolume, volume);
            _SEVolume = volumeActual;
            mixer.SetFloat("SEVolume", volumeActual);
        }

        public void ChangeBGMVolume(float volume)
        {
            float volumeActual = Mathf.Lerp(MinVolume, MaxVolume, volume);
            _BGMVolume = volumeActual;
            mixer.SetFloat("BGMVolume", volumeActual);
        }
    }
}
