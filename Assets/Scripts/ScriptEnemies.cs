using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptEnemies : MonoBehaviour
{
	public int MaxEnemies;
	void Start()
	{

	}

	void Update()
	{

	}

	public void reset()
	{
		foreach(Transform Child in transform) {
			GameObject.Destroy(Child.gameObject);
		}
	}

	public bool spawn(GameObject Prefab, Vector3 Pos, GameObject Homing)
	{
		bool isSpawned = false;
		if(transform.childCount < MaxEnemies) {
			var Enemy = Instantiate(
				Prefab, Pos, Quaternion.Euler(.0f, .0f, .0f), this.transform
			);
			var EnemyZombie = Enemy.GetComponent<ScriptZombie>();

			EnemyZombie.Initialize(Homing);
			// Debug.LogFormat("Spawned at {0}", Pos);
			isSpawned = true;
		}
		return isSpawned;
	}
}
