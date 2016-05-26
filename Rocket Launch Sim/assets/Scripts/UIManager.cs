using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The user interface (UI) manager is responsible for controlling which screen to display
/// as well as updating the current game heads up display (HUD). The UI manager is a singleton
/// and can be accessed in any script using the UIManager.Instance syntax.
/// </summary>
public class UIManager : MonoBehaviour
{
	// The static singleton instance of the UI manager.
	public static UIManager Instance { get; private set; }

	// Control panel
	[SerializeField]
	Text budgetText = null;
	Color underBudgetColor = Color.white;
	Color overBudgetColor = Color.red;

	[SerializeField]
	Slider rocketMaterialSlider = null;

	[SerializeField]
	Text rocketMaterialValue = null;

	[SerializeField]
	Slider fuelTypeSlider = null;

	[SerializeField]
	Text fuelTypeValue = null;

	[SerializeField]
	Slider fuelAmountSlider = null;

	[SerializeField]
	Button launchButton = null;

	bool cpDisabled = false;
	bool overBudget = false;

	// HUD
	[SerializeField]
	Slider fuelRemainingSlider = null;

	[SerializeField]
	Text speedText = null;

	[SerializeField]
	Text maxSpeedText = null;

	[SerializeField]
	Text heightText = null;

	[SerializeField]
	Text maxHeightText = null;

	[SerializeField]
	GameObject[] screens = {};	// GameObject array for all the screens.

	[SerializeField]
	GameObject hud;				// GameObject for the HUD.

	[SerializeField]
	AudioClip buttonClick;	// Sound when a button is clicked.
	AudioSource buttonClickSource;

	void Awake()
	{
		// Register this script as the singleton instance.
		Instance = this;
	}

	void Start()
	{
		buttonClickSource = AudioHelper.CreateAudioSource(gameObject, buttonClick);
	}

	void OnEnable()
	{
		rocketMaterialSlider.onValueChanged.AddListener(OnRocketMaterialChanged);
		fuelTypeSlider.onValueChanged.AddListener(OnFuelTypeChanged);
		fuelAmountSlider.onValueChanged.AddListener(OnFuelAmountChanged);
	}

	void OnDisable()
	{
		rocketMaterialSlider.onValueChanged.RemoveListener(OnRocketMaterialChanged);
		fuelTypeSlider.onValueChanged.RemoveListener(OnFuelTypeChanged);
		fuelAmountSlider.onValueChanged.RemoveListener(OnFuelAmountChanged);
	}

	/// <summary>
	/// Shows the screen with the given name and hide everything else.
	/// </summary>
	/// <param name="name">Name of the screen to be shown.</param>
	public void ShowScreen(string name)
	{
		// Loop through all the screens in the array.
		foreach (GameObject screen in screens)
		{
			// Activate the screen with the matching name, and deactivate
			// any screen that doesn't match.
			screen.SetActive(screen.name == name);
		}
	}

	/// <summary>
	/// Shows/hides the HUD.
	/// </summary>
	/// <param name="show">Do we show the HUD?</param>
	public void ShowHUD(bool show)
	{
		hud.SetActive(show);
	}

	/// <summary>
	/// Initialization function to set up the control panel in the HUD.
	/// </summary>
	/// <param name="materials">List of valid rocket materials.</param>
	/// <param name="fuels">List of valid fuel types.</param>
	/// <param name="maxFuel">Maximum fuel allowed.</param>
	public void SetupControlPanel(List<RocketMaterial> materials, List<RocketFuel> fuels, int maxFuel)
	{
		// Rocket material slider is whole numbers corresponding to indices into the material list
		rocketMaterialSlider.minValue = 0;
		rocketMaterialSlider.maxValue = materials.Count - 1;
		rocketMaterialSlider.wholeNumbers = true;
		rocketMaterialSlider.value = 0;

		// Fuel type slider is whole numbers corresponding to indices into the fuel type list
		fuelTypeSlider.minValue = 0;
		fuelTypeSlider.maxValue = fuels.Count - 1;
		fuelTypeSlider.wholeNumbers = true;
		fuelTypeSlider.value = 0;

		// Fuel amount slider is whole numbers from 1 to maxFuel
		fuelAmountSlider.minValue = 1;
		fuelAmountSlider.maxValue = maxFuel;
		fuelTypeSlider.wholeNumbers = true;
		fuelAmountSlider.value = 1;
	}

	public void UpdateHUD(float speed, float maxSpeed, float height, float maxHeight)
	{
		ShowSpeed(speed);
		ShowMaxSpeed(maxSpeed);
		ShowHeight(height);
		ShowMaxHeight(maxHeight);
	}

	/// <summary>
	/// Updates the speed text in the HUD.
	/// </summary>
	/// <param name="speed">Speed.</param>
	void ShowSpeed(float speed)
	{
		// 1 decimal place
		speedText.text = string.Format(LocalizationManager.Instance.GetString("HUD Speed"), speed.ToString("n1"));
	}

	/// <summary>
	/// Updates the max speed text in the HUD.
	/// </summary>
	/// <param name="maxSpeed">Max speed.</param>
	void ShowMaxSpeed(float maxSpeed)
	{
		// 1 decimal place
		maxSpeedText.text = string.Format(LocalizationManager.Instance.GetString("HUD Max Speed"), maxSpeed.ToString("n1"));
	}

	/// <summary>
	/// Updates the height text in the HUD.
	/// </summary>
	/// <param name="height">Height.</param>
	void ShowHeight(float height)
	{
		// 1 decimal place
		heightText.text = string.Format(LocalizationManager.Instance.GetString("HUD Height"), height.ToString("n1"));
	}

	/// <summary>
	/// Updates the max height text in the HUD.
	/// </summary>
	/// <param name="maxHeight">Max height.</param>
	void ShowMaxHeight(float maxHeight)
	{
		// 1 decimal place
		maxHeightText.text = string.Format(LocalizationManager.Instance.GetString("HUD Max Height"), maxHeight.ToString("n1"));
	}

	/// <summary>
	/// Updates the fuel remaining slider in the HUD.
	/// </summary>
	/// <param name="remainingFuel">Remaining fuel.</param>
	/// <param name="maxFuel">Maximum amount of fuel that was taken on this launch.</param>
	public void ShowFuelRemaining(float remainingFuel, float maxFuel)
	{
		fuelRemainingSlider.minValue = 0;
		fuelRemainingSlider.maxValue = maxFuel;
		fuelRemainingSlider.value = remainingFuel;
	}

	/// <summary>
	/// Updates the rocket material details in the control panel.
	/// </summary>
	/// <param name="mat">Material.</param>
	public void UpdateRocketMaterial(RocketMaterial mat)
	{
		// Material cost is a currency value with no decimal places
		rocketMaterialValue.text = string.Format(LocalizationManager.Instance.GetString(mat.materialKey), mat.weight, mat.cost.ToString("C0"));
	}

	/// <summary>
	/// Updates the fuel type details in the control panel.
	/// </summary>
	/// <param name="fuel">Fuel type.</param>
	public void UpdateFuelType(RocketFuel fuel)
	{
		// Fuel cost is a currency value with no decimal places
		fuelTypeValue.text = string.Format(LocalizationManager.Instance.GetString(fuel.fuelKey), fuel.impulsePerWeight, fuel.costPerWeight.ToString("C0"));
	}

	/// <summary>
	/// Updates the budget text in the control panel.
	/// </summary>
	/// <param name="currentCost">Current cost of the rocket.</param>
	/// <param name="budget">Maximum allowed budget.</param>
	public void UpdateBudget(int currentCost, int budget)
	{
		// Cost and budget are currency values with no decimal places
		budgetText.text = string.Format(LocalizationManager.Instance.GetString("CP Budget"), currentCost.ToString("C0"), budget.ToString("C0"));

		// Change color of budget text if over budget
		if (currentCost > budget)
		{
			overBudget = true;
			budgetText.color = overBudgetColor;
		}
		else
		{
			overBudget = false;
			budgetText.color = underBudgetColor;
		}

		// Disable launch button if over budget or if control panel is disabled
		launchButton.interactable = !cpDisabled && !overBudget;
	}

	/// <summary>
	/// Handler for slider events on the rocket material slider.
	/// </summary>
	/// <param name="value">Value of the slider.</param>
	void OnRocketMaterialChanged(float value)
	{
		GameplayManager.Instance.SetRocketMaterial(Mathf.RoundToInt(value));
	}

	/// <summary>
	/// Handler for slider events on the fuel type slider.
	/// </summary>
	/// <param name="value">Value of the slider.</param>
	void OnFuelTypeChanged(float value)
	{
		GameplayManager.Instance.SetFuelType(Mathf.RoundToInt(value));
	}

	/// <summary>
	/// Handler for slider events on the fuel amount slider.
	/// </summary>
	/// <param name="value">Value of the slider.</param>
	void OnFuelAmountChanged(float value)
	{
		GameplayManager.Instance.SetFuelAmount(value);
	}

	/// <summary>
	/// Called when the tutorial screen is shown.
	/// </summary>
	public void OnTutorial()
	{
		EnableControlPanel(false);
	}

	/// <summary>
	/// Called when the rocket is launched.
	/// </summary>
	public void OnStartFlying()
	{
		EnableControlPanel(false);
	}

	/// <summary>
	/// Called when the rocket is reset to its initial position.
	/// </summary>
	public void OnReset()
	{
		EnableControlPanel(true);
	}

	/// <summary>
	/// Enables the UI elements in the control panel.
	/// </summary>
	/// <param name="enabled">Enabled.</param>
	void EnableControlPanel(bool enabled)
	{
		cpDisabled = !enabled;
		rocketMaterialSlider.interactable = enabled;
		fuelTypeSlider.interactable = enabled;
		fuelAmountSlider.interactable = enabled;

		//Launch button is only enabled if we're also not over budget
		launchButton.interactable = !cpDisabled && !overBudget;
	}

	/// <summary>
	/// Call this function to play the button click sound.
	/// </summary>
	public void OnButton()
	{
		buttonClickSource.Play();
	}

	public void OnLanguageChanged()
	{
		foreach (StaticTextManager staticText in FindObjectsOfType<StaticTextManager>())
		{
			staticText.OnLanguageChanged();
		}
	}
}