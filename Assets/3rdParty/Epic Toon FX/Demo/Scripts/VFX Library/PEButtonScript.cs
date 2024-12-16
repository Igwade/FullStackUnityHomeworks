using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public enum ButtonTypes {
	NotDefined,
	Previous,
	Next
}

public class PeButtonScript : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler {
	#pragma warning disable 414
	private Button _myButton;
	#pragma warning disable 414
	[FormerlySerializedAs("ButtonType")] public ButtonTypes buttonType = ButtonTypes.NotDefined;

	// Use this for initialization
	void Start () {
		_myButton = gameObject.GetComponent<Button> ();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		// Used for Tooltip
		UICanvasManager.GlobalAccess.mouseOverButton = true;
		UICanvasManager.GlobalAccess.UpdateToolTip (buttonType);
	}

	public void OnPointerExit(PointerEventData eventData) {
		// Used for Tooltip
		UICanvasManager.GlobalAccess.mouseOverButton = false;
		UICanvasManager.GlobalAccess.ClearToolTip ();
	}

	public void OnButtonClicked () {
		// Button Click Actions
		UICanvasManager.GlobalAccess.UIButtonClick(buttonType);
	}
}
