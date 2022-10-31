using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cameraFollow;
    [SerializeField] Transform spawnPoint;

    void Start() //TODO: load save data
    {
        ProgressData data = SavesManager.LoadProgress();

        GameObject player = Instantiate(GameDataBase.Instance.UltimatePlayerList[data.character], data.position == Vector2.zero ? spawnPoint.position : data.position, Quaternion.identity);
        cameraFollow.Follow = player.transform;

        if (player.TryGetComponent(out PlayerBehaviour playerBehaviour))
		{
            playerBehaviour.SetupData(data.health, data.weaponInventory, data.currentWeaponIndex);
        }

        //TODO: play you arrived to .... animation
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
            PlayerBehaviour playerBehaviour = FindObjectOfType<PlayerBehaviour>();

            print("EnvironmentManager - SavesData!");
            SavesManager.SaveProgress(new ProgressData(playerBehaviour.characterCode, playerBehaviour.GetHealth, SceneManager.GetActiveScene().buildIndex, playerBehaviour.transform.position,
                playerBehaviour.GetWeaponInventory, playerBehaviour.currentWeaponIndex)); //TODO: handle player character code and also Scene index later

        }
	}
}
