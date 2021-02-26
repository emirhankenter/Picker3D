using Mek.Models;

namespace Game.Scripts.Models
{
    public class GameConfig : MekGameConfig
    {
        public string GameName = "My Test Game";
        public float GameVersion = 1.0f;

        public GameConfig()
        {

        }

        #region Singleton

        private static readonly GameConfig _instance = new GameConfig();

        public static GameConfig Instance
        {
            get { return _instance; }
        }

        #endregion 
    }
}
