using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCamera : MonoBehaviour
{
	public GameObject PlayerToFollow;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

				// Use the player pos, correct it with proper Y pos
				var Pos = PlayerToFollow.transform.position;
				Pos.y = 6;
				Pos.z -= 6;

				transform.position = Pos;
        transform.rotation = Quaternion.LookRotation(PlayerToFollow.transform.position - Pos);
    }
}
