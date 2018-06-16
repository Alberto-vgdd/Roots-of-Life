using System.Collections;
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
	public AudioClip playerAttack1AudioClip;
	public AudioClip playerAttack2AudioClip;

	[Header("Background Music Parameters")]
	public AudioSource backgroundMusicSource;
	public AudioClip levelMusicAudioClip;
	public AudioClip titleMusicAudioClip;

	[Header("Audio Settings")]
	[Range(0f,1f)]
	public float itemAudioVolume = 0.7f;
	[Range(0f,1f)]
	public float playerAudioVolume = 0.5f;
	[Range(0f,1f)]
	public float backgroundMusicVolume = 0.0f;

	void Awake()
	{
		GlobalData.SoundManagerScript = this;
	}		

	void Start()
	{
		itemAudioSource.volume = itemAudioVolume;
		playerAudioSource.volume = playerAudioVolume;
		backgroundMusicSource.volume = backgroundMusicVolume;
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

	public void PlayAttack1Sound()
	{
		playerAudioSource.loop = false;

		if (!playerAudioSource.clip.Equals(playerAttack1AudioClip))
		{
			playerAudioSource.clip = playerAttack1AudioClip;
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

	public void PlayAttack2Sound()
	{
		if ( !playerAudioSource.clip.Equals(playerAttack2AudioClip) || !playerAudioSource.isPlaying)
		{
			playerAudioSource.loop = true;
			playerAudioSource.clip = playerAttack2AudioClip;
			playerAudioSource.Play();
		}

	}
	
	public void StopAttack2Sound()
	{
		if (playerAudioSource.clip.Equals(playerAttack2AudioClip))
		{
			playerAudioSource.Stop();
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

	public void MuteInGameFX()
	{
		itemAudioSource.volume = 0;
		playerAudioSource.volume = 0;
	}

	public void UnMuteInGameFX()
	{
		itemAudioSource.volume = itemAudioVolume;
		playerAudioSource.volume = playerAudioVolume;
	}
}
