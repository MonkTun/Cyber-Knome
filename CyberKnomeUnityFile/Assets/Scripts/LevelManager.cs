using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{	
	[SerializeField] GameObject loadingScreen;

	public void LoadScene(int index)
	{
		StartCoroutine(AsyncLoad(index));
	}

	IEnumerator AsyncLoad(int index)
	{
		loadingScreen.SetActive(true);

		AsyncOperation asy = SceneManager.LoadSceneAsync(index);

		while (!asy.isDone)
		{
			yield return null;
		}

		loadingScreen.SetActive(false);
	} 

	//public async Task 

}
