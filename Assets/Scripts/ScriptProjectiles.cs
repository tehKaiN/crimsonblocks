using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptProjectiles : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void reset()
	{
		foreach(Transform Child in transform) {
			GameObject.Destroy(Child.gameObject);
		}
	}

	public void add(GameObject Prefab, Transform Source, float AngleRange)
	{
		var Rotation = Source.rotation.eulerAngles;
		Rotation.y += Random.Range(-AngleRange, AngleRange);
		var BulletObj = Instantiate(
			Prefab,
			Source.position + Source.rotation * new Vector3(0.0f, 0.0f, 1.0f),
			Quaternion.Euler(Rotation), this.transform
		);

		var Bullet = BulletObj.GetComponent<ScriptBullet>();
		Bullet.initialize(1000.0f);
	}
}
