using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SALLO
{
	/// <summary>
	/// Extension methods for the <see cref="Transform"/> class
	/// </summary>
	public static class ExtendedTransform
	{
		/// <summary>
		/// Set local position and rotation for the caller <see cref="Transform"/>
		/// </summary>
		/// <param name="T"></param>
		/// <param name="newLocalPosition"></param>
		/// <param name="newLocalRotation"></param>
		public static void SetLocalPositionAndRotation(this Transform T, Vector3 newLocalPosition, Quaternion newLocalRotation)
		{			
			T.localPosition = newLocalPosition;
			T.localRotation = newLocalRotation;
		}
	}
}