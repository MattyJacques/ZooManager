// Title        : LoopingMainTheme.cs
// Purpose      : Loop the main menu / loading screen music
// Author       : Dan Budworth-Mead
// Date         : 19/09/2016

using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class LoopingMainTheme : MonoBehaviour
    {
        [SerializeField]    private AudioClip _audioClip;
                            private AudioSource _audioSource;
        [SerializeField]    private float _startTime = 8.548f;
        [SerializeField]    private float _endTime = 76.512f;
                            private float _loopTime = 0;

        void Awake()
        {
            _loopTime = _endTime - _startTime;
        }

        void Start()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;

            _audioSource.timeSamples = (int)(_startTime * 44100);

            _audioSource.clip = _audioClip;

            StartCoroutine(PlayMusic());
        }

        IEnumerator PlayMusic()
        {
            do
            {
                _audioSource.Play();
                yield return new WaitForSeconds(_loopTime);
            } while (true); //keep repeating until game object is deleted.
        }
    }
}