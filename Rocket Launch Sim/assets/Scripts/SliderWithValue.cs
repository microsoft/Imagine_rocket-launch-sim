using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// A script that controls a slider with a text label.
/// This script will also change the text label in edit mode.
/// </summary>
[ExecuteInEditMode]
public class SliderWithValue : MonoBehaviour
{
	[SerializeField]
	Slider slider = null;
	[SerializeField]
	Text text = null;
	[SerializeField]
	int decimals = 0;

	void Start()
	{
		ChangeValue(slider.value);
	}

	void Update()
	{
		decimals = Mathf.Max(decimals, 0);
	}

	void OnEnable()
	{
		// Add an event listeners to the slider
		slider.onValueChanged.AddListener(ChangeValue);
	}

	void OnDisable()
	{
		// Remove the event listener from the slider
		slider.onValueChanged.RemoveListener(ChangeValue);
	}

	/// <summary>
	/// Updates the text label with the slider's value.
	/// </summary>
	/// <param name="value">The value to update.</param>
	void ChangeValue(float value)
	{
		text.text = value.ToString("n" + decimals);
	}
}
