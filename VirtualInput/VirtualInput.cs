using UnityEngine;
using System.Collections.Generic;
using System;

public class VirtualInput
{
	public enum ButtonState
	{
		Inactive = 0,
		Down,
		Up,
		Held
	}
	
	public static bool enabled = false;
	
	private static Dictionary<string,ButtonState> buttonStates;
	private static Dictionary<string,float> axes;
	
	public static void Init()
	{
		enabled = true;
		
		if(buttonStates == null)
		{
			buttonStates = new Dictionary<string, ButtonState>();	
		}
		if(axes == null)
		{
			axes = new Dictionary<string, float>();	
		}
	}
	
	public static void AddButton(string buttonName)
	{
		if(buttonStates == null)
		{
			Init();
		}
		
		try
		{
			buttonStates.Add(buttonName, ButtonState.Inactive);
		}
		catch(ArgumentException e)
		{
		}
	}
	
	public static void AddAxis(string axisName)
	{
		if(axes == null)
		{
			Init();
		}
		
		try
		{
			axes.Add(axisName, 0.0f);
		}
		catch(ArgumentException e)
		{
		}
	}
	
	public static void SetButton(string buttonName, ButtonState state)
	{
		if(!buttonStates.ContainsKey(buttonName))
		{
			AddButton(buttonName);
		}
		
		buttonStates[buttonName] = state;
	}
	public static void SetAxis(string axisName, float pos)
	{
		if(!axes.ContainsKey(axisName))
		{
			AddAxis(axisName);
		}
		
		axes[axisName] = pos;
	}
	
	public static float GetAxis(string axisName)
	{
		if(enabled)
		{
			try
			{
				return axes[axisName];
			}
			catch(KeyNotFoundException e)
			{
				Debug.LogError("Virtual Input Axis doesn't exist: '" + axisName + "'");
			}
		}
		
		return 0.0f;
	}
	
	public static bool GetButton(string buttonName)
	{
		return (enabled && buttonStates[buttonName] == ButtonState.Held);
	}
	public static bool GetButtonDown(string buttonName)
	{
		return (enabled && buttonStates[buttonName] == ButtonState.Down);
	}
	public static bool GetButtonUp(string buttonName)
	{
		return (enabled && buttonStates[buttonName] == ButtonState.Up);
	}
	
	
	static public bool pointInsideAABB(Vector2 min, Vector2 max, Vector2 point)
	{
		return (point.x >= min.x && point.x <= max.x) &&
			(point.y >= min.y && point.y <= max.y);
	}
	
	static public bool AABBcollide(Vector2 min, Vector2 max, Vector2 min2, Vector2 max2)
	{
		return (min.x <= max2.x && max.x >= min2.x) &&
			(max.y >= min2.y && min.y <= max2.y);
	}
	
}