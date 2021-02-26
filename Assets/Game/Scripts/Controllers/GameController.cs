using Mek.Controllers;
using Mek.Localization;

namespace Game.Scripts.Controllers
{
    public class GameController : GameBaseController
    {
        protected override void Awake()
        {
            base.Awake();

            LocalizationManager.Init();
            LocalizationManager.SetLanguage(Language.Turkish);
        }
    }
}
