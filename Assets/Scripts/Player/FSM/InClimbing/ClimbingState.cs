using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.EventFrame.CustomEvent;
using UnityEngine;

namespace AlsRitter.PlayerController.FSM
{
    public class ClimbingState : PlayerBaseState, IEventObserver
    {
        public readonly InClimbState parentState;

        public override string name => "Climbing";

        private bool isOnGround = false;
        private bool isHeadWall = false;

        public ClimbingState(InClimbState parentState)
        {
            this.parentState = parentState;
            // 注册自己
            EventManager.Register(this, EventID.OnGround, EventID.OnHeadWall);
        }


        public override void Update(PlayerFSMSystem player)
        {
            if (Mathf.Abs(player.yVelocity) < 0.01)
            {
                TransitionOtherState(parentState, parentState.hangWallState, player);
            }
        }

        public override void FixedUpdate(PlayerFSMSystem player)
        {
            if (isOnGround && player.yVelocity < 0)
            {
                return;
            }

            if (isHeadWall && player.yVelocity > 0)
            {
                return;
            }

            player.transform.position =
                new Vector2(player.transform.position.x,
                    player.transform.position.y + player.climbSpeed * player.yVelocity);
        }

        public void HandleEvent(EventData resp)
        {
            switch (resp.eid)
            {
                case EventID.OnHeadWall:
                    isHeadWall = ((PlayerStateEventData) resp).trigger;
                    break;
                case EventID.OnGround:
                    isOnGround = ((PlayerStateEventData) resp).trigger;
                    break;
            }
        }

        ~ClimbingState() 
        {  
            EventManager.Remove(this);
        }
    }
}