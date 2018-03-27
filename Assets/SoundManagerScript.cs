using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour 
{
	public AudioSource acornAudioSource;

	void Start()
	{
		GlobalData.SoundManagerScript = this;
	}		

	public void PlayAcornSound()
	{
		if (acornAudioSource.isPlaying)
		{
			acornAudioSource.time = 0f;
		}
		else
		{
			acornAudioSource.Play();
		}
	}
}
