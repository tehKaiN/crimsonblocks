using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScriptUi : MonoBehaviour
{
	private Text m_TextKills;
	private Slider m_SliderHp;
	private ScriptLevel m_Level;
	private PlayerStuff m_Player;
	private GameObject m_PanelPause;
	private GameObject m_PanelDead;

	public enum MenuState {
		Off,
		Dead,
		Paused,
		Main,
	};

	private MenuState m_eState;

	public MenuState State {
		get {
			return m_eState;
		}

		set {
			// First, turn off all panels
			m_PanelPause.SetActive(false);
			m_PanelDead.SetActive(false);

			if(value == MenuState.Off) {
				// Unpause
				Time.timeScale = 1.0f;
				Cursor.visible = false;
			}
			else {
				Cursor.visible = true;
				switch(value) {
					case MenuState.Paused:
						// Show only pause menu
						m_PanelPause.SetActive(true);

						// Pause the game
						Time.timeScale = 0.0f;
						break;
					case MenuState.Dead:
						// Show only dead menu
						m_PanelDead.SetActive(true);
						break;
					case MenuState.Main:
						// Show only main menu
						break;
				}
			}
			m_eState = value;
		}
	}

	void Start()
	{
		m_TextKills = GameObject.Find("TextKills").GetComponent<Text>();
		m_SliderHp = GameObject.Find("SliderHp").GetComponent<Slider>();
		m_Level = GameObject.Find("LevelState").GetComponent<ScriptLevel>();
		m_Player = GameObject.Find("Player").GetComponent<PlayerStuff>();
		m_PanelPause = GameObject.Find("PanelPause");
		m_PanelDead = GameObject.Find("PanelDead");

		State = MenuState.Off;
	}

	void Update()
	{
		m_TextKills.text = "☠: " + m_Level.Kills;
		m_SliderHp.value = m_Player.Hp;
	}

	void OnTogglePause(InputValue Pause)
	{
		if(State == MenuState.Paused) {
			State = MenuState.Off;
		}
		else if(State == MenuState.Off) {
			State = MenuState.Paused;
		}
	}

	public void OnButtonExitGame()
	{
	#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
	#else
		Application.Quit();
	#endif
	}

	public void OnRestartGame()
	{
		m_Level.reset();
		State = MenuState.Off;
	}

	public void OnButtonResume()
	{
		State = MenuState.Off;
	}
}
