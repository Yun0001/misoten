using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    public enum seName
    {
        seTest
    }

    public enum bgmName
    {
        bgmTest
    }

    [SerializeField]
    private AudioClip[] seClip;

    [SerializeField]
    private AudioClip[] bgmClip;

    private AudioSource[] audioSource;

	// Use this for initialization
	void Awake ()
    {
        audioSource[0] = gameObject.GetComponent<AudioSource>();
        audioSource[0].clip = seClip[0];	
	}

    public void PlaySE(seName sename)
    {
        audioSource[0].clip = seClip[(int)sename];
        audioSource[0].Play();
    }

    public void PlayBGM(bgmName bgmname)
    {
        audioSource[0].clip = bgmClip[(int)bgmname];
        audioSource[0].Play();
    }

}
