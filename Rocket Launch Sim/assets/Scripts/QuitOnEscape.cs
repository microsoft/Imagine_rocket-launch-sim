using UnityEngine;
using System.Collections;

/// <summary>
/// Script to detect if the escape key is pressed. If so it will quit the game.
/// </summary>
public class QuitOnEscape : MonoBehaviour
{
	void LateUpdate()
	{
		// Check if the ESC key is pressed.
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			// Quit the application.
			Application.Quit();
		}
	}
}
