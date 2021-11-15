using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptLevel : MonoBehaviour
{
	private float m_fNextSpawnTime;
	private float m_fNextSpawnDelta;
	private int m_SpawnCount;
	private int m_NextSpawnCount;
	private int m_Kills;

	private ScriptEnemies m_Enemies;

	public GameObject PrefabZombie;
	public GameObject Player;
	public Camera Cam;
	private ScriptUi m_Ui;

	public int Kills {
		get {return m_Kills;}
	}

	void Start()
	{
		m_Enemies = GameObject.Find("Enemies").GetComponent<ScriptEnemies>();
		m_Ui = GameObject.Find("Canvas").GetComponent<ScriptUi>();

		reset();
	}

	public void reset()
	{
		// Restart the default values
		m_fNextSpawnDelta = 1.0f;
		m_fNextSpawnTime = Time.time + m_fNextSpawnDelta;
		m_SpawnCount = 0;
		m_NextSpawnCount = 1;
		m_Kills = 0;

		// Reset the player state
		Player.GetComponent<PlayerStuff>().reset();
		m_Enemies.reset();
	}

	void Update()
	{
		var Now = Time.time;
		if(Now >= m_fNextSpawnTime) {
			bool isSpawned = false;

			// Try spawning each enemy 3 times in case something blocks the position
			for(int SpawnIdx = 0; SpawnIdx < m_NextSpawnCount; ++SpawnIdx) {
				for(int i = 0; i < 3; ++i) {
					var Pos = new Vector3(Random.Range(-50.0f, 50.0f), 0.5f, Random.Range(-50.0f, 50.0f));
					var CamPos = Cam.WorldToViewportPoint(Pos);
					if(
						// Check if outside the screen
						((CamPos.x < .0f || 1.0f < CamPos.x) && (CamPos.y < .0f || 1.0f < CamPos.y)) ||
						CamPos.z < 0
						// TODO: check if no other enemies in similar pos
					) {
						// No objects in range
						isSpawned = m_Enemies.spawn(PrefabZombie, Pos, Player);
						break;
					}
				}
			}

			if(isSpawned) {
				++m_SpawnCount;
				m_NextSpawnCount = Mathf.Clamp(m_SpawnCount / 10, 1, 100);
				m_fNextSpawnTime += m_fNextSpawnDelta;
			}
		}

		if(
			!Player.GetComponent<PlayerStuff>().isAlive &&
			m_Ui.State == ScriptUi.MenuState.Off
		) {
			m_Ui.State = ScriptUi.MenuState.Dead;
		}
	}

	public void scoreKill(int Exp)
	{
		++m_Kills;
	}
}
