using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] soundClips; // 여러 종류의 사운드 클립 배열
    private AudioSource audioSource;

    private void Awake()
    {
        // AudioSource 컴포넌트를 가져오거나 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 특정 사운드를 1회 재생하는 함수
    public void PlaySound(int soundIndex)
    {
        // 유효한 인덱스인지 확인 (사운드 클립이 배열에 존재하는지)
        if (soundIndex >= 0 && soundIndex < soundClips.Length)
        {
            audioSource.clip = soundClips[soundIndex];
            audioSource.PlayOneShot(audioSource.clip);
        }
        else
        {
            Debug.LogWarning("잘못된 사운드 인덱스입니다.");
        }
    }

    
}
