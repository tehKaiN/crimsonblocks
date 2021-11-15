using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrossHair : MonoBehaviour
{
	private Vector2 m_MousePos;
	private Plane m_Plane;
	private float? m_fPulseTime = null;

	private const float m_fScaleDefault = .2f;
	private const float m_fAnimTime = .5f;
	private const float m_fAnimScale = .3f;

	void OnMouse(InputValue Movement)
	{
		m_MousePos = Movement.Get<Vector2>();
		// Debug.LogFormat("New mouse input {0}", m_MousePos);
	}

	void Start()
	{
		m_Plane = new Plane(Vector3.up, new Vector3(0, .5f, 0));
	}

	void Update()
	{
		if(m_fPulseTime.HasValue) {
			var fDelta = Time.time - m_fPulseTime.Value;
			if(fDelta < m_fAnimTime) {
				transform.localScale = Vector3.one * (
					m_fScaleDefault + (m_fAnimTime - fDelta) / m_fAnimTime * m_fAnimScale
				);
			}
			else {
				m_fPulseTime = null;
				transform.localScale = Vector3.one * m_fScaleDefault;
			}
		}
		transform.Rotate(Time.deltaTime * new Vector3(.0f, 180.0f, .0f));
	}

	void FixedUpdate()
	{
		var R = Camera.main.ScreenPointToRay(m_MousePos);
		float Distance;
		if(m_Plane.Raycast(R, out Distance)) {
			var Pos = R.GetPoint(Distance);
			// Debug.LogFormat("Cursor is at point {0}", Pos);
			transform.position = Pos;
		}
	}

	public void pulseSize()
	{
		m_fPulseTime = Time.time;
	}
}
