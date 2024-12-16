using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace EpicToonFX
{

public class EtfxButtonScript : MonoBehaviour
{
	[FormerlySerializedAs("Button")] public GameObject button;
	Text _myButtonText;
	string _projectileParticleName;		// The variable to update the text component of the button

	EtfxFireProjectile _effectScript;		// A variable used to access the list of projectiles
	EtfxProjectileScript _projectileScript;

	public float buttonsX;
	public float buttonsY;
	public float buttonsSizeX;
	public float buttonsSizeY;
	public float buttonsDistance;
	
	void Start ()
	{
		_effectScript = GameObject.Find("ETFXFireProjectile").GetComponent<EtfxFireProjectile>();
		GetProjectileNames();
		_myButtonText = button.transform.Find("Text").GetComponent<Text>();
		_myButtonText.text = _projectileParticleName;
	}

	void Update ()
	{
		_myButtonText.text = _projectileParticleName;
//		print(projectileParticleName);
	}

	public void GetProjectileNames()			// Find and diplay the name of the currently selected projectile
	{
		// Access the currently selected projectile's 'ProjectileScript'
		_projectileScript = _effectScript.projectiles[_effectScript.currentProjectile].GetComponent<EtfxProjectileScript>();
		_projectileParticleName = _projectileScript.projectileParticle.name;	// Assign the name of the currently selected projectile to projectileParticleName
	}

	public bool OverButton()		// This function will return either true or false
	{
		Rect button1 = new Rect(buttonsX, buttonsY, buttonsSizeX, buttonsSizeY);
		Rect button2 = new Rect(buttonsX + buttonsDistance, buttonsY, buttonsSizeX, buttonsSizeY);
		
		if(button1.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) ||
		   button2.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
		{
			return true;
		}
		else
			return false;
	}
}
}