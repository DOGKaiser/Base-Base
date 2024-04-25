using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public static class BaseTools {

	static Dictionary<string, Type> mAssemblies = new Dictionary<string, Type>();

	static BaseTools() { }

	public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class/*, IComparable<T>*/ {
		List<T> objects = new List<T>();
		foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
			if (!mAssemblies.ContainsKey(type.Name)) {
				mAssemblies.Add(type.Name, type);
			}
			objects.Add((T)Activator.CreateInstance(type, constructorArgs));
		}

		return objects;
	}

	public static Type GetEnumerableOfType(string className) {
		foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).Where(myType => myType.IsClass && !myType.IsAbstract && myType.Name == className)) {
			if (!mAssemblies.ContainsKey(className)) {
				mAssemblies.Add(type.Name, type);
				return type;
			}
		}

		return null;
	}

	public static object GetInstance(string className) {
		if (!mAssemblies.TryGetValue(className, out Type t)) {
			t = GetEnumerableOfType(className);
		}
		return GetInstance(t);
	}
	
	public static object GetInstance(Type type) {
		return Activator.CreateInstance(type);
	}

	public static List<Type> GetAllInheritorsOfType<T>(params object[] constructorArgs) where T : class {
		return AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(a => a.GetTypes())
			.Where(t => typeof(T).IsAssignableFrom(t) && t != typeof(T))
			.ToList();
		/*
		List<Type> types = new List<Type>();
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		Type interfaceType = typeof(T);
		foreach (Assembly assembly in assemblies) {
			// Get the types in the assembly that implement the interface
			IEnumerable<Type> typesAssembly = assembly.GetTypes().Where(type => interfaceType.IsAssignableFrom(type) && type != interfaceType);

			types.AddRange(typesAssembly);
		}
		
		return types;
		*/
	}

	public static void CallStaticFunctionOfClass(string assemblyName, string className, string methodName) {
		Assembly assembly = Assembly.Load( assemblyName );
		if (assembly == null) {
			Debug.LogWarning("No Assembly by name "+ assemblyName);
		}
		
		Type type = assembly?.GetType(className);
		if (type == null) {
			Debug.LogWarning("No Class by name "+ className);
		}
		MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
		if (method == null) {
			Debug.LogWarning($"Class {className} does not have the method {methodName}");
		}
		method?.Invoke(null, null);
	}

	public static bool TimerReady(DateTime timeStarted, TimeSpan timeToWait) {
		TimeSpan timeSinceStarted = DateTime.UtcNow.Subtract(timeStarted);

		if (timeToWait.TotalMilliseconds - timeSinceStarted.TotalMilliseconds <= 0)
			return true;

		return false;
	}

    public static void WriteToFile(string file, string text, bool writeIfFileExists) {
        if (!writeIfFileExists && File.Exists(file)) {
	        return;
        }
        
        using (StreamWriter writer = File.AppendText(file))
        {
	        writer.WriteLine(text);
        }
    }

    public static void StackDeleteMid<T>(Stack<T> st, T n) {
        // If stack is empty or all  
        // items are traversed 
        if (st.Count == 0)
            return;

        // Remove current item 
        T x = st.Peek();
        st.Pop();

        // Remove other items 
        StackDeleteMid(st, n);

        // Put all items  
        // back except middle 
        if (!x.Equals(n))
            st.Push(x);
    }

	public static string FormatMilliSecondsToString(double milliseconds, int precision = 2) {
		var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
		var parts = new[]
		{
			$"{timeSpan.Days} day{(timeSpan.Days > 1 ? "s" : "")}",
			$"{timeSpan.Hours} hr{(timeSpan.Hours > 1 ? "s" : "")}",
			$"{timeSpan.Minutes} min{(timeSpan.Minutes > 1 ? "s" : "")}",
			$"{timeSpan.Seconds} sec{(timeSpan.Seconds > 1 ? "s" : "")}"
		};

		return string.Join(" ", parts.Where(p => !p.StartsWith("0 ")).Take(precision));
	}

	public static bool IsAllOfListInFirstInSecond<T>(T[] arrayA, T[] arrayB) {
		foreach (T t in arrayB) {
			if (!arrayA.Contains(t)) {
				return false;
			}
		}
		return true;
	}
	
	public static bool IsAnyOfListInFirstInSecond<T>(T[] arrayA, T[] arrayB) {
		foreach (T t in arrayB) {
			if (arrayA.Contains(t)) {
				return true;
			}
		}
		return false;
	}
	
	public static void CreateEntryFromAny(this ZipArchive archive, string sourceName, string entryName = "") {
		string fileName = Path.GetFileName(sourceName);
		if (File.GetAttributes(sourceName).HasFlag(FileAttributes.Directory)) {
			archive.CreateEntryFromDirectory(sourceName, Path.Combine(entryName, fileName));
		} else {
			archive.CreateEntryFromFile(sourceName, Path.Combine(entryName, fileName));
		}
	}

	public static void CreateEntryFromDirectory(this ZipArchive archive, string sourceDirName, string entryName = "") {
		string[] files = Directory.GetFiles(sourceDirName).Concat(Directory.GetDirectories(sourceDirName)).ToArray();
		foreach (string file in files) {
			archive.CreateEntryFromAny(file, entryName);
		}
	}
}
