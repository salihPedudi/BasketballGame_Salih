using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasketballGameDefinition
{
    // kullunıcıya ait tutulacak key leri burada tanımla
    public enum PlayerPrefKey
    {
        PlayerNickName,
        PlayerBestScore,
        PlayerLevel,
        PlayerSettings
    }


    // tag lar 
    public enum Tags
    {
        BasketballHoop,
        Target,
        Plane,
        Other
    }

    public enum PlayerSettings
    {
        Sound,
        Music,
        Vibration,      // ileride kullanılabilir
        Notification,   // ileride kullanılabilir
    }

    public enum CrowdAnimationClips
    {
        Idle = 0,
        Happy,
        Unhappy
    }

    // mixer ile ses kontrolü yapmak istersek;
    public enum AudioMixerTypes
    {
        Master = 0,
        SoundEffect,
        Music
    }

    public enum SoundEffect
    {        
        SuccessfullBall,
        ShootBall,
        HitBall,
        CrowdIdle,
        CrowdHappy,
        CrowdUnhappy,
        SuccessLevel,
        FailedLevel
    }

    public enum Panels
    {
        StartPanel=0,
        FailedPanel,
        SuccessPanel,
        EndPanel
    }

}

