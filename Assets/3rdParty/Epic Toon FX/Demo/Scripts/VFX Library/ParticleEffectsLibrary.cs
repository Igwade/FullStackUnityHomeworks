using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class ParticleEffectsLibrary : MonoBehaviour {
	public static ParticleEffectsLibrary GlobalAccess;
	void Awake () {
		GlobalAccess = this;

		_currentActivePeList = new List<Transform> ();

		totalEffects = particleEffectPrefabs.Length;

		currentParticleEffectNum = 1;

		// Warn About Lengths of Arrays not matching
		if (particleEffectSpawnOffsets.Length != totalEffects) {
			Debug.LogError ("ParticleEffectsLibrary-ParticleEffectSpawnOffset: Not all arrays match length, double check counts.");
		}
		if (particleEffectPrefabs.Length != totalEffects) {
			Debug.LogError ("ParticleEffectsLibrary-ParticleEffectPrefabs: Not all arrays match length, double check counts.");
		}

		// Setup Starting PE Name String
		_effectNameString = particleEffectPrefabs [currentParticleEffectIndex].name + " (" + currentParticleEffectNum.ToString() + " of " + totalEffects.ToString() + ")";
	}

	// Stores total number of effects in arrays - NOTE: All Arrays must match length.
	[FormerlySerializedAs("TotalEffects")] public int totalEffects = 0;
	[FormerlySerializedAs("CurrentParticleEffectIndex")] public int currentParticleEffectIndex = 0;
	[FormerlySerializedAs("CurrentParticleEffectNum")] public int currentParticleEffectNum = 0;
//	public string[] ParticleEffectDisplayNames;
	[FormerlySerializedAs("ParticleEffectSpawnOffsets")] public Vector3[] particleEffectSpawnOffsets;
	// How long until Particle Effect is Destroyed - 0 = never
	[FormerlySerializedAs("ParticleEffectLifetimes")] public float[] particleEffectLifetimes;
	[FormerlySerializedAs("ParticleEffectPrefabs")] public GameObject[] particleEffectPrefabs;

	// Storing for deleting if looping particle effect
	#pragma warning disable 414
	private string _effectNameString = "";
	#pragma warning disable 414
	private List<Transform> _currentActivePeList;

	void Start () {
	}

	public string GetCurrentPeNameString() {
		return particleEffectPrefabs [currentParticleEffectIndex].name + " (" + currentParticleEffectNum.ToString() + " of " + totalEffects.ToString() + ")";
	}

	public void PreviousParticleEffect() {
		// Destroy Looping Particle Effects
		if (particleEffectLifetimes [currentParticleEffectIndex] == 0) {
			if (_currentActivePeList.Count > 0) {
				for (int i = 0; i < _currentActivePeList.Count; i++) {
					if (_currentActivePeList [i] != null) {
						Destroy (_currentActivePeList [i].gameObject);
					}
				}
				_currentActivePeList.Clear ();
			}
		}

		// Select Previous Particle Effect
		if (currentParticleEffectIndex > 0) {
			currentParticleEffectIndex -= 1;
		} else {
			currentParticleEffectIndex = totalEffects - 1;
		}
		currentParticleEffectNum = currentParticleEffectIndex + 1;

		// Update PE Name String
		_effectNameString = particleEffectPrefabs [currentParticleEffectIndex].name + " (" + currentParticleEffectNum.ToString() + " of " + totalEffects.ToString() + ")";
	}
	public void NextParticleEffect() {
		// Destroy Looping Particle Effects
		if (particleEffectLifetimes [currentParticleEffectIndex] == 0) {
			if (_currentActivePeList.Count > 0) {
				for (int i = 0; i < _currentActivePeList.Count; i++) {
					if (_currentActivePeList [i] != null) {
						Destroy (_currentActivePeList [i].gameObject);
					}
				}
				_currentActivePeList.Clear ();
			}
		}

		// Select Next Particle Effect
		if (currentParticleEffectIndex < totalEffects - 1) {
			currentParticleEffectIndex += 1;
		} else {
			currentParticleEffectIndex = 0;
		}
		currentParticleEffectNum = currentParticleEffectIndex + 1;

		// Update PE Name String
		_effectNameString = particleEffectPrefabs [currentParticleEffectIndex].name + " (" + currentParticleEffectNum.ToString() + " of " + totalEffects.ToString() + ")";
	}

	private Vector3 _spawnPosition = Vector3.zero;
	public void SpawnParticleEffect(Vector3 positionInWorldToSpawn) {
		// Spawn Currently Selected Particle Effect
		_spawnPosition = positionInWorldToSpawn + particleEffectSpawnOffsets[currentParticleEffectIndex];
		GameObject newParticleEffect = GameObject.Instantiate(particleEffectPrefabs[currentParticleEffectIndex], _spawnPosition, particleEffectPrefabs[currentParticleEffectIndex].transform.rotation) as GameObject;
		newParticleEffect.name = "PE_" + particleEffectPrefabs[currentParticleEffectIndex];
		// Store Looping Particle Effects Systems
		if (particleEffectLifetimes [currentParticleEffectIndex] == 0) {
			_currentActivePeList.Add (newParticleEffect.transform);
		}
		_currentActivePeList.Add(newParticleEffect.transform);
		// Destroy Particle Effect After Lifetime expired
		if (particleEffectLifetimes [currentParticleEffectIndex] != 0) {
			Destroy(newParticleEffect, particleEffectLifetimes[currentParticleEffectIndex]);
		}
	}
}
