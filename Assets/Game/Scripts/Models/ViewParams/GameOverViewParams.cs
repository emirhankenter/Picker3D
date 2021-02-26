using System;
using System.Collections;
using System.Collections.Generic;
using MekNavigation;
using UnityEngine;

public class GameOverViewParams : ViewParams
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
