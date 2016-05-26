using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The localization manager is responsible for generating a list of language dictionaries
/// from text strings read from an external file. It also tracks the currently selected
/// language so we can retrieve the correct text string by giving string key. The localization
/// manager is a singleton and can be accessed in any script using the
/// LocalizationManager.Instance syntax.
/// </summary>
public class LocalizationManager : MonoBehaviour
{
	// The static singleton instance of the localization manager.
	public static LocalizationManager Instance { get; private set; }

	// The currently selected language (default to the first language if empty).
	static string currentLanguage = "";

	List<string> languages;											// List of available languages.
	Dictionary<string, Dictionary<string, string>> locDictionary;	// Collection of language dictionaries.

	void Awake()
	{
		// Register this script as the singleton instance.
		Instance = this;

		// Read the localization string data file.
		locDictionary = CSVReader.Read("Localization");
		var first = locDictionary.ElementAt(0).Value;
		if (first.Count <= 0)
		{
			Debug.Log("Invalid localization file. No language found!");
			return;
		}

		// Get the list of available languages.
		languages = new List<string>();
		foreach (string k in first.Keys)
		{
			languages.Add(k);
		}

		// If we don't have a language selected, default to the first language in our list.
		if (currentLanguage == null || currentLanguage.Length == 0)
		{
			currentLanguage = languages.First();
		}
	}

	/// <summary>
	/// Retrieves a string based on a given key and in the given language.
	/// </summary>
	/// <param name="key">The string key.</param>
	/// <param name="language">The language to look up the key in.</param>
	/// <returns>The localized string for the key in the given language.</returns>
	string GetLocString(string key, string language)
	{
		// Check if we have an entry for the given key.
		if (locDictionary.ContainsKey(key))
		{
			// Check if we have an entry for the given language with this key.
			if (locDictionary[key].ContainsKey(language))
			{
				// Return the localized string.
				return locDictionary[key][language];
			}
		}

		// The key or language is not found, return an error string.
		return "?_no_key_?";
	}

	/// <summary>
	/// Returns a string for the current language based on the given key.
	/// </summary>
	/// <param name="key">The string key.</param>
	/// <returns>The corresponding string for the current language.</returns>
	public string GetString(string key)
	{
		return GetLocString(key, currentLanguage);
	}

	/// <summary>
	/// Returns the language name for the given language.
	/// </summary>
	/// <param name="language">The language name.</param>
	/// <returns>The translated language name.</returns>
	public string GetLanguageString(string language)
	{
		return GetLocString("Language", language);
	}

	/// <summary>
	/// Returns a list of supported languages.
	/// </summary>
	/// <returns>The list of supported languages.</returns>
	public List<string> GetLanguages()
	{
		return languages.ToList(); // copy
	}

	/// <summary>
	/// Returns the current language.
	/// </summary>
	/// <returns>The current language.</returns>
	public string GetCurrentLanguage()
	{
		return currentLanguage;
	}

	/// <summary>
	/// Sets the current language.
	/// </summary>
	/// <param name="language">The newly selected language.</param>
	public void SetLanguage(string language)
	{
		// Check if this language is available.
		if (languages.Contains(language))
		{
			// Switch to this language and refresh all the text strings.
			currentLanguage = language;
			GameplayManager.Instance.OnLanguageChanged();
		}
		else
		{
			// This language is not available, ignore the request and show a warning.
			Debug.LogWarningFormat("Localizer does not have translations for language {0}, ignoring...", language);
		}
	}
}
