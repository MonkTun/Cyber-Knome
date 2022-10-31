using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region field

	public static GameManager Instance;

	[Header("Reference")]
	[SerializeField] LevelManager levelManager;
	[SerializeField] SettingsManager settingsManager;
	[SerializeField] GameObject playPanel, pausedPanel;

	public enum State
	{
		lobby, playing, paused,
	}
	public State gameState { get; private set; } //TODO: levelManager should call set gameState??

	#endregion

	#region start

	private void OnEnable()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else
		{
			Destroy(gameObject);
			return;
		}

		settingsManager.LoadSettings();
	}

	#endregion

	#region update

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && State.lobby != gameState)
		{
			if (gameState == State.paused) ResumeGame();
			else PauseGame();
		}
	}

	#endregion

	#region game application

	public void StartGame()
	{
		levelManager.LoadScene(SavesManager.LoadProgress().scene == -1 ? 1 : SavesManager.LoadProgress().scene); //TODO: change the index accordingly
		PanelManage(playPanel);
		gameState = State.playing;
	}

	public void PauseGame()
	{
		print("pause");
		//Time.timeScale = 0f;
		PanelManage(pausedPanel);
		gameState = State.paused;
	}

	public void ResumeGame()
	{
		//Time.timeScale = 1;
		PanelManage(playPanel);
		gameState = State.playing;
	}

	public void LeaveGame()
	{
		levelManager.LoadScene(0);
		gameState = State.lobby;
		PanelManage(null);
	}

	#endregion

	#region game

	public void TravelScene(int index)
	{
		PlayerBehaviour playerBehaviour = FindObjectOfType<PlayerBehaviour>();

		print("SavesData!");
		SavesManager.SaveProgress(new ProgressData(playerBehaviour.characterCode, playerBehaviour.GetHealth, index, Vector2.zero,
			playerBehaviour.GetWeaponInventory, playerBehaviour.currentWeaponIndex));

		levelManager.LoadScene(index);
	}

	#endregion

	#region tools/utils

	public void PanelManage(GameObject exception)
	{
		playPanel.SetActive(exception == playPanel);
		pausedPanel.SetActive(exception == pausedPanel);
	}

	#endregion
}
