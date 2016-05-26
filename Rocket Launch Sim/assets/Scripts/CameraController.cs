using UnityEngine;
using System.Collections;

/// <summary>
/// CameraController - script that manages camera movement.
/// </summary>
public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }

	// The object we are tracking with the camera
	[SerializeField]
	GameObject target;

	// y-offset from our target
	[SerializeField]
	float offset = 0;

	// Is the rocket flying?
	bool flying = false;
	// Does the rocket still have fuel?
	bool hasFuel = true;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start()
	{
		Reset();
	}

	/// <summary>
	/// Resets the camera's position and state
	/// </summary>
	public void Reset()
	{
		flying = false;
		hasFuel = true;
		RenderSettings.fog = false;

		var newPos = transform.position;
		// Track x-position
		newPos.x = target.transform.position.x;

		// Track y-position + offset
		newPos.y = target.transform.position.y + offset;

		transform.position = newPos;
	}

	void Update()
	{
		if (flying)
		{
			// Track x-position if the rocket is still flying
			var newPos = transform.position;
			newPos.x = target.transform.position.x;

			// Screen-shake horizontally if using fuel
			if (hasFuel)
			{
				newPos.x += Random.Range(-0.15f, 0.15f);
			}

			// Track y-position if the rocket is still flying
			newPos.y = target.transform.position.y + offset;
			transform.position = newPos;
		}
	}

	/// <summary>
	/// Called when the rocket starts flying.
	/// </summary>
	public void OnFlyingStarted()
	{
		flying = true;
	}

	/// <summary>
	/// Called when the rocket runs out of fuel.
	/// </summary>
	public void OnFuelEmpty()
	{
		hasFuel = false;
	}

	/// <summary>
	/// Called when the rocket is no longer moving upwards.
	/// </summary>
	public void OnFlyingEnded()
	{
		flying = false;
	}

	/// <summary>
	/// Sets the camera's target to an object, to track it.
	/// </summary>
	/// <param name="inTarget">The object to track.</param>
	void SetTarget(GameObject inTarget)
	{
		target = inTarget;
	}
}
