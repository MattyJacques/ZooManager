using UnityEngine;
using System;
using System.Linq;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using LuaFramework;
using System.IO;
using ZooManager;

namespace ZooManager {

	[MoonSharpUserData]
	public class GameState
	{

        private Dictionary<string, Building> buildings = new Dictionary<string, Building>();
        private Dictionary<string, Animal> animals = new Dictionary<string, Animal>();

        public int multiplier = 2;
		public double calcHypotenuse(double a, double b) {
			return Math.Sqrt(a * a + b * b) * multiplier;
		}

        // Entities: buildings
        public void defineBuilding(Table t)
        {
            Building building;
            string id = t.Get("id").ToString();
            if (buildings.TryGetValue(id, out building)) {
                LuaReader.ReadClassData(building, t);
                buildings[id] = building;
            } else {
                building = new Building();
                LuaReader.ReadClassData(building, t);
                buildings.Add(id, building);
            }
        }

        // Entities: animals
        public void defineAnimal(Table t)
        {
            Animal animal;
            string id = t.Get("id").ToString();
            if (animals.TryGetValue(id, out animal))
            {
                LuaReader.ReadClassData(animal, t);
                animals[id] = animal;
            }
            else
            {
                animal = new Animal();
                LuaReader.ReadClassData(animal, t);
                animals.Add(id, animal);
            }
        }

    }

	[MoonSharpUserData]
	public class GameHooks
	{

		private Timer gameTickTimer;
		private int gameTickInterval = 1000;

		private EventHandler _onTest;
		private EventHandler _onGameTick;
		private EventHandler _onGamePause;
		private EventHandler _onGameResume;
		private EventHandler _onGameStart;
		private EventHandler _onGameEnd;

		public GameHooks() {
			gameTickTimer = new Timer(gameTickInterval);
			gameTickTimer.Elapsed += new ElapsedEventHandler(onTimer);
		}
			
		public event EventHandler onTest {
			add { if (_onTest == null || !_onTest.GetInvocationList().Contains(value)) _onTest += value; }
			remove { _onTest -= value; }
		}

		public event EventHandler onGameTick {
			add { if (_onGameTick == null || !_onGameTick.GetInvocationList().Contains(value)) _onGameTick += value; }
			remove { _onGameTick -= value; }
		}

		public event EventHandler onGamePause {
			add { if (_onGamePause == null || !_onGamePause.GetInvocationList().Contains(value)) _onGamePause += value; }
			remove { _onGamePause -= value; }
		}

		public event EventHandler onGameResume {
			add { if (_onGameResume == null || !_onGameResume.GetInvocationList().Contains(value)) _onGameResume += value; }
			remove { _onGameResume -= value; }
		}

		public event EventHandler onGameStart {
			add { if (_onGameStart == null || !_onGameStart.GetInvocationList().Contains(value)) _onGameStart += value; }
			remove { _onGameStart -= value; }
		}

		public event EventHandler onGameEnd {
			add { if (_onGameEnd == null || !_onGameEnd.GetInvocationList().Contains(value)) _onGameEnd += value; }
			remove { _onGameEnd -= value; }
		}

		/* Private Methods */

		private void onTimer(object sender, ElapsedEventArgs e) {
			doGameTick ();
		}

		private void doGameTick() {
			if (_onGameTick != null) _onGameTick(this, EventArgs.Empty);
		}

		/* Public Methods */

		public void doTest() {
			if (_onTest != null) _onTest(this, EventArgs.Empty);
		}

		public void doGameStart() {
			gameTickTimer.Enabled = true;
			if (_onGameStart != null) _onGameStart(this, EventArgs.Empty);
		}

		public void doGamePause() {
			gameTickTimer.Enabled = false;
			if (_onGamePause != null) _onGamePause(this, EventArgs.Empty);
		}

		public void doGameResume() {
			gameTickTimer.Enabled = true;
			if (_onGameResume != null) _onGameResume(this, EventArgs.Empty);
		}

		public void doGameEnd() {
			gameTickTimer.Enabled = false;
			if (_onGameEnd != null) _onGameEnd(this, EventArgs.Empty);
		}

	}

	public class ModManager : MonoBehaviour {

		private List<Mod> mods = new List<Mod>();
		private GameState gameState = new GameState ();
		private GameHooks gameHooks = new GameHooks ();

		void Start () {
			// Register EventArgs type for usage with events
			UserData.RegisterType<EventArgs>();
			// Load
			InitializeScriptLoader ();
			//Debug.Log (Application.persistentDataPath);
			//Debug.Log (Application.dataPath);
			//Debug.Log (Environment.CurrentDirectory);
			PreloadMods (DiscoverMods());
			ListMods ();
			InitMods ();
		}

		void ListMods() {
			foreach (Mod mod in mods) {
				Debug.Log (mod);
			}
		}

		void InitMods() {
			foreach (Mod mod in mods) {
				mod.Init ();
			}
		}

		void PreloadMods(List<string> mod_configs) {
			foreach (string m in mod_configs) {
				string json = File.ReadAllText (m);
				Mod mod = JsonUtility.FromJson<Mod>(json);
				mod.Register (m, gameState, gameHooks);
				mod.Load ();
				mods.Add(mod);
			}
		}

		public string ModListJSON() {
			string retval = "[";
			int i = 0;
			foreach (Mod mod in mods) {
				string json = JsonUtility.ToJson(mod);
				retval += json;
				i++;
				if (i < mods.Count) {
					retval += ",";
				}

			}
			return retval + "]";
		}

		List<string> DiscoverMods() {
			List<string> modsA = DiscoverModsInPath (Path.Combine(Environment.CurrentDirectory, "Mods"));
			List<string> modsB = DiscoverModsInPath (Path.Combine(Application.persistentDataPath, "Mods"));
			List<string> all_mods = modsA.Concat (modsB).ToList ();
			return all_mods;
		}

		List<string> DiscoverModsInPath(string basepath) {
			List<string> fileList = new List<string>();
			try {
				IEnumerable<string> files = Directory.GetFiles(basepath, "*.*", SearchOption.AllDirectories)
				.Where(s => s.EndsWith(".json"));
				foreach (string f in files) {
					fileList.Add(f);
				}
				return fileList;
			} catch (UnauthorizedAccessException UAEx) {
				Console.WriteLine(UAEx.Message);
			} catch (PathTooLongException PathEx) {
				Console.WriteLine(PathEx.Message);
			} catch (Exception Ex) {
				Console.WriteLine(Ex.Message);
			}
			return fileList;
		}

		void InitializeScriptLoader () {
			Dictionary<string, string> scripts = new  Dictionary<string, string>();
			object[] result = Resources.LoadAll("Lua", typeof(TextAsset));
			foreach(TextAsset ta in result.OfType<TextAsset>()) {
				scripts.Add(ta.name, ta.text);
			}
			Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.UnityAssetsScriptLoader(scripts);
		}

		void Update () {
		
		}
	}
}