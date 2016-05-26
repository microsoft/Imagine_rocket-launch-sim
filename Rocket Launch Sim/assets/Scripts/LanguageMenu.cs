using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LanguageMenu : MonoBehaviour
{
	List<string> languages;

	[SerializeField]
	GameObject menuPanel;
	[SerializeField]
	Button parentButton;
	[SerializeField]
	GameObject menuItemPrefab;

	bool open;

	// Use this for initialization
	void Start()
	{
		languages = LocalizationManager.Instance.GetLanguages();

		foreach(string l in languages)
		{
			GameObject button = Instantiate(menuItemPrefab) as GameObject;
			button.GetComponentInChildren<Text>().text = LocalizationManager.Instance.GetLanguageString(l);
			button.transform.SetParent(menuPanel.transform);
			string lang = l;
			button.GetComponent<Button>().onClick.AddListener(
				() =>
				{
					OnLanguageSelected(lang);
					
				}
				);
		}

		menuPanel.SetActive(false);
		open = false;

		parentButton.GetComponentInChildren<Text>().text = LocalizationManager.Instance.GetLanguageString(LocalizationManager.Instance.GetCurrentLanguage());
	}

	void OnLanguageSelected(string lang)
	{
		LocalizationManager.Instance.SetLanguage(lang);
		parentButton.GetComponentInChildren<Text>().text = LocalizationManager.Instance.GetLanguageString(lang);
		CloseMenu();
	}

	public void OnMenuPressed()
	{
		open = !open;
		menuPanel.SetActive(open);
	}

	public void CloseMenu()
	{
		open = false;
		menuPanel.SetActive(false);
	}
}
