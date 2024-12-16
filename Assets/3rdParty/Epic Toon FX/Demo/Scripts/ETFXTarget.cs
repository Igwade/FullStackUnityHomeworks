using UnityEngine;
using System.Collections;

namespace EpicToonFX
{

public class EtfxTarget : MonoBehaviour
{
    [Header("Effect shown on target hit")]
	public GameObject hitParticle;
    [Header("Effect shown on target respawn")]
	public GameObject respawnParticle;
	private Renderer _targetRenderer;
	private Collider _targetCollider;

    void Start()
    {
		_targetRenderer = GetComponent<Renderer>();
		_targetCollider = GetComponent<Collider>();
    }

    void SpawnTarget()
    {
        _targetRenderer.enabled = true; //Shows the target
		_targetCollider.enabled = true; //Enables the collider
		GameObject respawnEffect = Instantiate(respawnParticle, transform.position, transform.rotation) as GameObject; //Spawns attached respawn effect
		Destroy(respawnEffect, 3.5f); //Removes attached respawn effect after x seconds
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Missile") // If collider is tagged as missile
        {
            if (hitParticle)
            {
				//Debug.Log("Target hit!");
				GameObject destructibleEffect = Instantiate(hitParticle, transform.position, transform.rotation) as GameObject; // Spawns attached hit effect
				Destroy(destructibleEffect, 2f); // Removes hit effect after x seconds
				_targetRenderer.enabled = false; // Hides the target
				_targetCollider.enabled = false; // Disables target collider
				StartCoroutine(Respawn()); // Sets timer for respawning the target
            }
        }
    }
	
	IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
		SpawnTarget();
    }
}
}