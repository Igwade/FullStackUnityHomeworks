using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UICanvasManager : MonoBehaviour {
	public static UICanvasManager GlobalAccess;
	void Awake () {
		GlobalAccess = this;
	}

	[FormerlySerializedAs("MouseOverButton")] public bool mouseOverButton = false;
	[FormerlySerializedAs("PENameText")] public Text peNameText;
	[FormerlySerializedAs("ToolTipText")] public Text toolTipText;

	// Use this for initialization
	void Start () {
		if (peNameText != null)
			peNameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPeNameString();
	}
	
	// Update is called once per frame
	void Update () {
	
		// Mouse Click - Check if mouse over button to prevent spawning particle effects while hovering or using UI buttons.
		if (!mouseOverButton) {
			// Left Button Click
			if (Input.GetMouseButtonUp (0)) {
				// Spawn Currently Selected Particle System
				SpawnCurrentParticleEffect();
			}
		}

		if (Input.GetKeyUp (KeyCode.A)) {
			SelectPreviousPe ();
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			SelectNextPe ();
		}
	}

	public void UpdateToolTip(ButtonTypes toolTipType) {
		if (toolTipText != null) {
			if (toolTipType == ButtonTypes.Previous) {
				toolTipText.text = "Select Previous Particle Effect";
			}
			else if (toolTipType == ButtonTypes.Next) {
				toolTipText.text = "Select Next Particle Effect";
			}
		}
	}
	public void ClearToolTip() {
		if (toolTipText != null) {
			toolTipText.text = "";
		}
	}

	private void SelectPreviousPe() {
		// Previous
		ParticleEffectsLibrary.GlobalAccess.PreviousParticleEffect();
		if (peNameText != null)
			peNameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPeNameString();
	}
	private void SelectNextPe() {
		// Next
		ParticleEffectsLibrary.GlobalAccess.NextParticleEffect();
		if (peNameText != null)
			peNameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPeNameString();
	}

	private RaycastHit _rayHit;
	private void SpawnCurrentParticleEffect() {
		// Spawn Particle Effect
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (mouseRay, out _rayHit)) {
			ParticleEffectsLibrary.GlobalAccess.SpawnParticleEffect (_rayHit.point);
		}
	}

	/// <summary>
	/// User interfaces the button click.
	/// </summary>
	/// <param name="buttonTypeClicked">Button type clicked.</param>
	public void UIButtonClick(ButtonTypes buttonTypeClicked) {
		switch (buttonTypeClicked) {
		case ButtonTypes.Previous:
			// Select Previous Prefab
			SelectPreviousPe();
			break;
		case ButtonTypes.Next:
			// Select Next Prefab
			SelectNextPe();
			break;
		default:
			// Nothing
			break;
		}
	}
}
