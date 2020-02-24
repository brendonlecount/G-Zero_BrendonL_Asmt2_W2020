using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			Application.Quit();
		}
		else if (Input.GetButtonDown("Scene00"))
		{
			SceneManager.LoadScene(0);
		}
		else if (Input.GetButtonDown("Scene01"))
		{
			SceneManager.LoadScene(1);
		}
	}

}
