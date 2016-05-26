using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaticTextManager : MonoBehaviour
{
	[System.Serializable]
	public struct TextInfo
	{
		public Text textObject;
		public string localizationKey;
	}

	public TextInfo[] textObjects;

	// Use this for initialization
	void Start()
	{
		UpdateText();
	}

	void UpdateText()
	{
		foreach (TextInfo t in textObjects)
		{
			t.textObject.text = LocalizationManager.Instance.GetString(t.localizationKey);
		}
	}

	public void OnLanguageChanged()
	{
		UpdateText();
	}
}
