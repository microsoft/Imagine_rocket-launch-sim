using UnityEngine;
using System.Collections;

/// <summary>
/// Script to play a sound when the game object is activated.
/// </summary>
public class PlaySoundOnActive : MonoBehaviour
{
	public AudioClip sound;	// Sound to be played.

	AudioSource source;

	void OnEnable()
	{
		// Check if we have a audio source, if not create a new one.
		if (source == null)
		{
			source = AudioHelper.CreateAudioSource(gameObject, sound);
		}
		source.Play();
	}
}
