using Game.Scripts.Controllers;

namespace Game.Scripts.Behaviours.EventTriggerers
{
    public class FinishLineTriggerer : EventTriggerer
    {
        protected override void TriggerEnter(IPicker picker)
        {
            picker.OnPassedFinishLine();
            gameObject.SetActive(false);
        }
    }
}
