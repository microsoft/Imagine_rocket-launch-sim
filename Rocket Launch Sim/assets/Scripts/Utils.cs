using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

/// <summary>
/// Various utility helper functions.
/// </summary>
public static class Utils
{
	/// <summary>
	/// Extension method for duplicating fields and properties of a component via reflection.
	/// </summary>
	/// <returns>The duplicated component.</returns>
	/// <param name="comp">The new component to copy to.</param>
	/// <param name="original">The original component to copy from.</param>
	/// <typeparam name="T">The concrete type of the component to be duplicated.</typeparam>
	public static T Duplicate<T>(this Component comp, T original) where T : Component
	{
		Type type = comp.GetType();
		if (type != original.GetType())
		{
			return null; // type mis-match
		}

		// We are only interested in copying public attributes
		BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

		// Copy properties
		PropertyInfo[] pinfos = type.GetProperties(flags);
		foreach (PropertyInfo pinfo in pinfos)
		{
			// Make sure we only copy attributes that are writable and not obsolete.
			if (pinfo.CanWrite && !pinfo.IsDefined(typeof(ObsoleteAttribute), true))
			{
				try
				{
					pinfo.SetValue(comp, pinfo.GetValue(original, null), null);
				}
				catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
			}
		}

		// Copy fields
		FieldInfo[] finfos = type.GetFields(flags);
		foreach (FieldInfo finfo in finfos)
		{
			finfo.SetValue(comp, finfo.GetValue(original));
		}
		return comp as T;
	}

	/// <summary>
	/// Extension method to add a new component based on a reference copy.
	/// </summary>
	/// <returns>The newly added component.</returns>
	/// <param name="go">The game object to add the component to.</param>
	/// <param name="original">The original reference copy.</param>
	/// <typeparam name="T">The concrete type of the component to be duplicated.</typeparam>
	public static T AddComponent<T>(this GameObject go, T original) where T : Component
	{
		return go.AddComponent<T>().Duplicate(original) as T;
	}
}
