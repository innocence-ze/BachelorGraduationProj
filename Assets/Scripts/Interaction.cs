using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButton(0))
		{
			HandleInput();
		}
	}

	void HandleInput()
	{
		Debug.Log(123);
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(inputRay, out RaycastHit hit))
		{
			GameObject deformer = hit.collider.GetComponent<GameObject>();
			if (deformer)
			{
				Vector3 point = hit.point;
				Debug.Log(123 + deformer.name);
			}
			Debug.Log(111 + deformer.name);
		}
	}
}
