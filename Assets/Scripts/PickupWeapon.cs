using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
	public enum WeaponType {
		Pistol,
		Shotgun
	};

	public WeaponType m_weaponType {get; private set;} = WeaponType.Pistol;

	public void Initialize(WeaponType weaponType)
	{
		m_weaponType = weaponType;
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "player") {
			var Player = col.GetComponent<PlayerStuff>();
			Player.EquipWeapon(m_weaponType);
			Destroy(gameObject);
		}
	}
}
