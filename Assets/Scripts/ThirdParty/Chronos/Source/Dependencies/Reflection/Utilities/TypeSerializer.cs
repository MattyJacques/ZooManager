using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

// Inspired from FullSerializer by Jacob Dufault
// From file: Reflection/fsTypeLookup.cs

namespace Chronos.Reflection
{
	/// <summary>
	/// An utility class to simply serialize and deserialize .NET types in a Unity context.
	/// </summary>
	public static class TypeSerializer
	{
		/// <summary>
		/// Serializes the specified type to its full name.
		/// </summary>
		public static string Serialize(Type type)
		{
			return type.FullName;
		}

		/// <summary>
		/// Deserializes the specified full type name to a type.
		/// </summary>
		public static Type Deserialize(string fullName)
		{
			// Look for the type if the default assembly

			Type type = Type.GetType(fullName, false, false);

			if (type != null)
			{
				return type;
			}

#if (!UNITY_METRO) // No AppDomain on WinRT

			// Look for types in other loaded assemblies

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = assembly.GetType(fullName);

				if (type != null)
				{
					return type;
				}
			}
#endif

			throw new Exception("Failed to deserialize type: " + fullName);
		}
	}
}
