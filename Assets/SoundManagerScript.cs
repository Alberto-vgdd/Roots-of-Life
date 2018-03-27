using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour 
{
	public AudioSource itemAudioSource;
	public AudioClip acornAudioClip;

	public AudioSource playerAudioSource;

	void Start()
	{
		GlobalData.SoundManagerScript = this;
	}		

	public void PlayAcornSound()
	{
		if (!itemAudioSource.clip.Equals(acornAudioClip))
		{
			itemAudioSource.clip = acornAudioClip;
			itemAudioSource.Play();
		}
		else
		{
			if (itemAudioSource.isPlaying)
			{
				itemAudioSource.time = 0f;
			}
			else
			{
				itemAudioSource.Play();
			}
		}
	}
}
