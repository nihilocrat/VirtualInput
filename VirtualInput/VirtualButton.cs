using UnityEngine;
using System.Collections.Generic;
using System;

public class VirtualButton : MonoBehaviour
{
	public string buttonName = "VirtualButton_1";
	public Color inactiveColor;
	public Color activeColor;
	
	private GUITexture button;
	private VirtualInput.ButtonState lastFrameState;
	
	public VirtualInput.ButtonState state
	{
		get; protected set;
	}
	
	void Start ()
	{
		button = GetComponent<GUITexture>();
		button.color = inactiveColor;
		
		VirtualInput.AddButton(buttonName);
	}
	
	void Update ()
	{
		var frameState = VirtualInput.ButtonState.Inactive;
		
		foreach(Touch touch in Input.touches)
		{
			Rect myRect = guiTexture.GetScreenRect();
			
			if(myRect.Contains(touch.position))
			{
				if(lastFrameState == VirtualInput.ButtonState.Inactive)
				{
					frameState = VirtualInput.ButtonState.Down;
					Debug.Log("Virtual Button Down: " + buttonName);
				}
				else
				{
					frameState = VirtualInput.ButtonState.Held;
				}
				
				guiTexture.color = activeColor;
				
				break;
			}
		}
		
		if(frameState == VirtualInput.ButtonState.Inactive &&
			(lastFrameState == VirtualInput.ButtonState.Held || lastFrameState == VirtualInput.ButtonState.Down))
		{
			frameState = VirtualInput.ButtonState.Up;
			Debug.Log("Virtual Button Up: " + buttonName);
		}
		
		if(frameState == VirtualInput.ButtonState.Inactive)
		{
			guiTexture.color = inactiveColor;
		}
		
		state = frameState;
		VirtualInput.SetButton(buttonName, state);
		lastFrameState = state;
	}
}
