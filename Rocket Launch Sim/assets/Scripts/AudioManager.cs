using UnityEngine;
using System.Collections;

/// <summary>
/// The audio manager allows us to centralize all the audio settings under one game object so it
/// is easier for the sound designer to locate and tune the audio settings. The audio source paramters
/// can then be referenced by game objects that actually plays the sound. The audio manager is a
/// singleton and can be accessed in any script using the AudioManager.Instance syntax.
/// </summary>
public class AudioManager : MonoBehaviour
{
	// The static singleton instance of the audio manager.
	public static AudioManager Instance { get; private set; }
	
	void Awake()
	{
		// Register this script as the singleton instance.
		Instance = this;
	}

	/// <summary>
	/// Returns the audio source with the matching audio clip name.
	/// </summary>
	/// <returns>The audio source.</returns>
	/// <param name="clipName">The audio clip name.</param>
	public AudioSource GetSourceWithClip(string clipName)
	{
		// Loop through all children audio sources.
		AudioSource[] audioSources = GetComponents<AudioSource>();
		foreach (AudioSource source in audioSources)
		{
			// Return the one with a matching audio clip name.
			if (source.clip.name == clipName)
			{
				return source;
			}
		}

		// If no match is found, log a warning and return null.
		Debug.LogWarning("[AudioManager] Can't find audio source with clip: " + clipName);
		return null;
	}
}
