using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cameraFollow;

    void Start() //TODO: load save data
    {
        ProgressData data = ProgressSave.LoadProgress();

        GameObject player = Instantiate(GameDataBase.Instance.UltimatePlayerList[data.character], data.position, Quaternion.identity);
        cameraFollow.Follow = player.transform;

        if (player.TryGetComponent(out PlayerBehaviour playerBehaviour))
		{
            playerBehaviour.SetupData(data.health, data.weaponInventory);
        }
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
            PlayerBehaviour playerBehaviour = FindObjectOfType<PlayerBehaviour>();

            print("SavesData!");
            ProgressSave.SaveProgress(new ProgressData(0, playerBehaviour.GetHealth, 1, playerBehaviour.transform.position,
                playerBehaviour.GetWeaponInventory)); //TODO: handle player character code and also Scene index later

        }
	}
}
