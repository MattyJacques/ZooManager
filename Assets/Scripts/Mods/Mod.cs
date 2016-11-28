using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using LuaFramework;

namespace ZooManager
{

	[Serializable]
	public class Mod
	{
		public string version;
		public string name;
		public string author;
		public string path;
		public string decription;
		public string id;

		public List<string> tags;
		public List<string> scripts;

		private Script script;
		private Table modfile;
		private GameState gameState;
		private GameHooks gameHooks;

		public Mod() {
			UserData.RegisterAssembly();
		}

		public void Register(string config_path, GameState state, GameHooks hooks) {
			// Set the root path for Mod files
			path = Path.GetDirectoryName (config_path);
			// Set which function to execute when "print" is called inside lua
			Script.DefaultOptions.DebugPrint = s => Debug.Log ("[LUA] " + s.ToLower ());
			// Initialize lua
			script = new Script ();
			// Tell lua to ignore LUA_HOME environment variable for include statements
			((ScriptLoaderBase)script.Options.ScriptLoader).IgnoreLuaPathGlobal = true;
			// Assign a custom file loader which looks for files in the current mod folder
			script.Options.ScriptLoader = new LuaLoader() { 
				ModulePaths = new string[] { 
					Path.Combine(Environment.CurrentDirectory, "Libs/lua/?.lua"), 
					path + "/?.lua"
				}
			};
			// GameState
			gameState = state;
			// GameHooks
			gameHooks = hooks;
			// Setup lua global goodies
			SetupLuaGlobals ();
		}

		public void SetupLuaGlobals() {
			// Initialize global game State;
			script.Globals["Game"] = gameState;
			// Initialize event hooks;
			script.Globals["Hooks"] = gameHooks;
		}

		public void Load() {
			try {
				foreach (string scr in scripts) {
					script.DoFile(Path.Combine (path, scr));
				}
				modfile = script.Globals.Get("Mod").Table;
				script.Call(modfile.Get("onLoad"));
			} catch (ScriptRuntimeException ex) {
				Debug.LogError ("[LUA] Error: "+ ex.DecoratedMessage);
			}
		}

		public void Unload() {
			script = null;
			modfile = null;
		}

		public void Init() {
			try {
				script.Call(modfile.Get("onInit"));
			} catch (ScriptRuntimeException ex) {
				Debug.LogError ("[LUA] Error: "+ ex.DecoratedMessage);
			}
		}

		public override string ToString() {
			return "[MOD] " + name + " " + id + "@" + version + " (" + path + ")";
		}
	}
}

class LuaLoader : ScriptLoaderBase
{
	public override object LoadFile(string file, Table globalContext) {	
		try {
			//Debug.Log(string.Format("[LUA] A request to load '{0}' has been made", file));
			return File.ReadAllText (file);
		} catch (Exception ex) {
			Debug.LogError (ex.Message);
			return "";
		}
	}
	public override bool ScriptFileExists(string name) {
		//Debug.Log ("Check if exists" + name);
		return File.Exists (name);
	}
}