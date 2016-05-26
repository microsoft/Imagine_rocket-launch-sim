using UnityEngine;
using System.Collections;

/// <summary>
/// Audio related helper functions. These are static functions and can be call directly using
/// AudioHelper.FunctionName() syntax without having to attach to any GameObject.
/// </summary>
public class AudioHelper
{
	/// <summary>
	/// Creates and attaches a new audio source component to the given game object. The
	/// audio source will play the audio clip provided. If an audio source with a matching
	/// audio clip is found in AudioManager, the settings will be copied over to the new
	/// audio source component.
	/// </summary>
	/// <returns>The new audio source component.</returns>
	/// <param name="obj">The game object the audio source component is attached to.</param>
	/// <param name="clip">The audio clip for the audio source component.</param>
	public static AudioSource CreateAudioSource(GameObject obj, AudioClip clip)
	{
		// Check if we have a match in the audio manager.
		AudioSource customSource = AudioManager.Instance.GetSourceWithClip(clip.name);

		AudioSource source;
		if (customSource == null)
		{
			// If no match is found, create a new audio source and use the default settings.
			source = obj.AddComponent<AudioSource>();
			source.clip = clip;
		}
		else
		{
			// If a match is found, create an audio source based on the custom settings.
			source = obj.AddComponent<AudioSource>(customSource);
		}
		return source;
	}
}
