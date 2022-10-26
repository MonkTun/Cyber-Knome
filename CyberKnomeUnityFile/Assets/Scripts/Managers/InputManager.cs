using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public void OnInputMovement(Vector2 input) //TODO: put all input here also option for pc
	{
		PlayerInput.SetPlayerInputMovement(input);
	}

	public void OnInputAim(Vector2 input)
	{
		PlayerInput.SetPlayerInputAim(input);
	}

	public void OnInteract()
	{
		//print("onInteract");
		PlayerInput.SetInteractInput(true);
	}
}

public static class PlayerInput
{
	public static Vector2 MovementInput { get; private set; }
	public static Vector2 AimInput { get; private set; }

	public static bool InteractInput { get; private set; }

	public static void SetPlayerInputMovement(Vector2 input)
	{
		MovementInput = input;
	}

	public static void SetPlayerInputAim(Vector2 input)
	{
		AimInput = input;
	}

	public static void SetInteractInput(bool input)
	{
		InteractInput = input;
	}
} 
