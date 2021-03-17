using Enumerators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuController : MonoBehaviour 
    {
        [SerializeField]
        public float startingRadius     = 5.0f;
        public float supplyRefill       = 15.0f;
        public int playerCount          = 2;
        public int borderSteps          = 72;
        public int nitroCount           = 15;
        public int crateAmount          = 15;
        public int rockCount            = 10;
        public int treeCount            = 150;
        public int mapSize              = 100;
        public int snowIntensity        = 25;
        public bool useKeyboard         = false;
        public bool dayNightCycle       = false;

        public GameSettings GameSettings { get; set; }

        public void Start() 
            => DontDestroyOnLoad(transform.gameObject);

        public void Update() {
            if (Input.GetKey(KeyCode.Space) || GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any))
                LaunchGame();
        }

        public void LaunchGame()
        {
            var configs = new List<PlayerConfiguration>
            {
                new PlayerConfiguration {
                    Color       = TankColor.Blue,
                    PlayerIndex = PlayerIndex.First,
                    Controller  = ControllerType.Gamepad,
                    TankType    = TankType.Viking},
                new PlayerConfiguration {
                    Color       = TankColor.Green,
                    PlayerIndex = PlayerIndex.Second,
                    Controller  = ControllerType.Keyboard,
                    TankType    = TankType.Viking},
        };
            GameSettings = new GameSettings
            {
                MapType = MapType.Meadow,
                BorderSteps = 72,
                CrateCount = crateAmount,
                DayNightCycle = dayNightCycle,
                MapSize = mapSize,
                NitroCount = nitroCount,
                PlayerCount = playerCount,
                RockCount = rockCount,
                SnowIntensity = snowIntensity,
                StartingRadius = startingRadius,
                SupplyRefill = supplyRefill,
                TreeCount = treeCount,
                PlayerConfigurations = configs
            };

            SceneManager.LoadScene("Level");
        }
    }
}
