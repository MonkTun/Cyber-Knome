using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProgressSave
{
    public static void SaveProgress(ProgressData data)
    {
        ES3.Save("save_character", data.character);
        ES3.Save("save_health", data.health);
        ES3.Save("save_scene", data.scene);
        ES3.Save("save_position", data.position);
        ES3.Save("save_weaponInventory", data.weaponInventory);
    }

    public static ProgressData LoadProgress()
	{
        int _character = ES3.Load("save_character", 0);

        int _health = ES3.Load("save_health", 200); //TODO: health system and effects

        int _scene = ES3.Load("save_scene", 1); //TODO: level selector?

        Vector2 _position = ES3.Load("save_position", Vector2.zero);

        List<int> defaultWeaponInventory = new List<int>();
        List<int> _weaponInventory = ES3.Load("save_weaponInventory", defaultWeaponInventory);

        return (new ProgressData(_character, _health, _scene, _position, _weaponInventory));
    }
}

public class ProgressData
{
    public int character;
    public int health;
    public int scene;
    public Vector2 position;
    public List<int> weaponInventory;

    public ProgressData (int _character, int _health, int _scene, Vector2 _position, List<int> _weaponInventory)
	{
        character = _character;
        health = _health;
        scene = _scene;
        position = _position;
        weaponInventory = _weaponInventory;
    }
}
