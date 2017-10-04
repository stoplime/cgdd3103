using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Help {

	public static bool isPause;

	/// <summary>
	/// float version of PI.
	/// </summary>
	public const float PIf = 3.14159265359f;

	/// <summary>
	/// Clamps an angle to [-180, 180) for degrees and [-PI, PI] for radians. 
	/// </summary>
	/// <param name="input">Value of the angle to be clamped.</param>
	/// <param name="degree">True for degrees, false for radians.</param>
	/// <returns></returns>
	public static float angleClamp(float input, bool degree)
	{
		if (degree)
		{
			while (input > 180f)
			{
				input -= 360;
			}
			while (input < -180f)
			{
				input += 360;
			}
		}
		else
		{
			while (input > PIf)
			{
				input -= 2*PIf;
			}
			while (input < -PIf)
			{
				input += 2*PIf;
			}
		}
		
		return input;
	}
}
