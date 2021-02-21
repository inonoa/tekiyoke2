using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class WarpController : MonoBehaviour
{
    [SerializeField] WarpParams params_;
    
    [SerializeField] WarpDoor rightDoor;
    [SerializeField] WarpDoor leftDoor;
    
    void Start()
    {
        rightDoor.HeroEnter
            .Merge(leftDoor.HeroEnter)
            .ThrottleFirstFrame(3)
            .Subscribe(door =>
            {
                var otherDoor = (door == rightDoor) ? leftDoor : rightDoor;
                Vector2 dst = otherDoor.transform.position + new Vector3(otherDoor == rightDoor ? 3 : -3, 0);
                HeroDefiner.currentHero.WarpPos(dst);

                CameraController.Current.Freeze(params_.CameraFreezeSeconds);
            });
    }
}
