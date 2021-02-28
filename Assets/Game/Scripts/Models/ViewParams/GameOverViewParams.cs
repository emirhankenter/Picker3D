using System;
using MekNavigation;

namespace Game.Scripts.Models.ViewParams
{
    public class GameOverViewParams : MekNavigation.ViewParams
    {
        public Action RewardClaimed;
        public bool IsSuccess;
        public float EarnAmount;

        public GameOverViewParams(bool isSuccess, float earnAmount, Action onRewardClaimed) : base(ViewTypes.GameOverPanel)
        {
            RewardClaimed = onRewardClaimed;
            IsSuccess = isSuccess;
            EarnAmount = earnAmount;
        }
    }
}
