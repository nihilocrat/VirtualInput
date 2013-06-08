using UnityEngine;
using System.Collections;

public class VirtualJoystick : MonoBehaviour//VirtualButton
{
	public string horizontalAxisName = "Horizontal";
	public string verticalAxisName = "Vertical";
	public Color activeColor = Color.white;
	public Color inactiveColor = Color.black;
	public Rect region;
	public float deadzone = 0.2f;
	
	private Vector2 center;
	private float joystickSize;
	
	public bool isJoystickActive
	{
		get; private set;
	}
	
	private bool lastState = false;
	private static readonly Vector2 nullVector = new Vector2(9000.0f, 9000.0f);
	
	void Start()
	{
		VirtualInput.AddAxis(horizontalAxisName);
		VirtualInput.AddAxis(verticalAxisName);
		
		joystickSize = guiTexture.pixelInset.width - guiTexture.pixelInset.left;
		
		HideJoystick();
	}
	
	// remember, inheritance isn't supported on Update()
	// so this runs alongside the VirtualButton update
	void Update()
	{
		if(isJoystickActive)
		{
			var axes = GetAxes();
			
			if(axes != nullVector)
			{
				VirtualInput.SetAxis(horizontalAxisName, axes.x);
				VirtualInput.SetAxis(verticalAxisName, axes.y);
			}
			else
			{
				HideJoystick();
			}
		}
		/*
		else if(lastState)
		{
			HideJoystick();
		}*/
		else
		{
			VirtualInput.SetAxis(horizontalAxisName, 0.0f);
			VirtualInput.SetAxis(verticalAxisName, 0.0f);
			
			foreach(Touch t in Input.touches)
			{
				if(isInsideRegion(t.position))
				{
					ShowJoystick(t.position);
					break;
				}
			}
		}
		
		lastState = isJoystickActive;
	}
	
	void ShowJoystick(Vector2 center)
	{
		guiTexture.transform.position = new Vector2(center.x / Screen.width, center.y / Screen.height);
		guiTexture.color = activeColor;
		
		this.center = center;
		isJoystickActive = true;
	}
	
	void HideJoystick()
	{
		guiTexture.color = inactiveColor;
		isJoystickActive = false;
	}
	
	bool isInsideRegion(Vector2 point)
	{
		var topLeft = new Vector2(region.x * Screen.width, region.y * Screen.height);
		var bottomRight = topLeft + new Vector2(region.width * Screen.width, region.height * Screen.height);
		if(VirtualInput.pointInsideAABB(topLeft, bottomRight, point))
		{
			return true;
		}
		
		return false;
	}
	
	Vector2 GetAxes()
	{
		foreach(Touch t in Input.touches)
		{
			if(isInsideRegion(t.position))
			{
				Vector2 axes = (t.position - center) / joystickSize;
				
				// if the finger exits the joystick area, normalize
				if(axes.magnitude > 1.0f)
				{
					axes.Normalize();
				}
				
				if(axes.magnitude <= deadzone)
				{
					return Vector2.zero;
				}
				
				return axes;
			}
		}
		
		// no longer pressing the joystick
		return nullVector;
	}
}
