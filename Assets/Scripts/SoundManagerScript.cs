﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour 
{
	[Header("Item Audio Parameters")]
	public AudioSource itemAudioSource;
	public AudioClip acornAudioClip;

	[Header("Player Audio Parameters")]
	public AudioSource playerAudioSource;
	public AudioClip playerWalkAudioClip;
	public AudioClip playerRunAudioClip;
	public AudioClip playerJumpAudioClip;

	[Header("Background Music Parameters")]
	public AudioSource backgroundMusicSource;
	public AudioClip levelMusicAudioClip;
	public AudioClip titleMusicAudioClip;

	void Awake()
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


	public void PlayJumpSound()
	{
		playerAudioSource.loop = false;

		if (!playerAudioSource.clip.Equals(playerJumpAudioClip))
		{
			playerAudioSource.clip = playerJumpAudioClip;
			playerAudioSource.Play();
		}
		else
		{
			if (playerAudioSource.isPlaying)
			{
				playerAudioSource.time = 0f;
			}
			else
			{
				playerAudioSource.Play();
			}
		}

	}

	public void PlayWalkSound()
	{
		
		
		if (!playerAudioSource.clip.Equals(playerWalkAudioClip) || !playerAudioSource.isPlaying)
		{
			playerAudioSource.loop = true;
			playerAudioSource.clip = playerWalkAudioClip;
			playerAudioSource.Play();
		}
	}

	public void PlayRunSound()
	{
		
		
		if (!playerAudioSource.clip.Equals(playerRunAudioClip) || !playerAudioSource.isPlaying)
		{
			playerAudioSource.loop = true;
			playerAudioSource.clip = playerRunAudioClip;
			playerAudioSource.Play();
		}

	}

	public void StopWalkRunSound()
	{
		
		if (playerAudioSource.clip.Equals(playerRunAudioClip) || playerAudioSource.clip.Equals(playerWalkAudioClip))
		{
			playerAudioSource.Stop();
		}
	}
	
	public void PlayTitleMusic()
	{
		if (!backgroundMusicSource.clip.Equals(titleMusicAudioClip))
		{
			backgroundMusicSource.clip = titleMusicAudioClip;
			backgroundMusicSource.Play();
		}
		else
		{
			if (backgroundMusicSource.isPlaying)
			{
				backgroundMusicSource.time = 0f;
			}
			else
			{
				backgroundMusicSource.Play();
			}
		}
	}

	public void PlayLevelMusic()
	{
		if (!backgroundMusicSource.clip.Equals(levelMusicAudioClip))
		{
			backgroundMusicSource.clip = levelMusicAudioClip;
			backgroundMusicSource.Play();
		}
		else
		{
			if (backgroundMusicSource.isPlaying)
			{
				backgroundMusicSource.time = 0f;
			}
			else
			{
				backgroundMusicSource.Play();
			}
		}
	}
}
