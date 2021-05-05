﻿using System.Collections;
using System.Collections.Generic;
using EventFrame;
using EventFrame.CustomEvent;
using PlayerController.FSM;
using UnityEngine;


namespace PlayerController
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator), typeof(PlayerFSMSystem))]
    public class PlayerAnimation : MonoBehaviour, IEventObserver
    {
        private Animator anim;
        private PlayerFSMSystem pm;
        private Rigidbody2D rb;

        private int isWalkId;
        private int isCrouchingId;
        private int isRunId;
        private int isDeathId;
        private int verticalVelocityId;
        private int inTheAirId;
        private int inTheClimbId;
        private int isClimbingId;
        // 用于告诉 antState 当前是否更新了状态
        private int isStateUpdateId;
        // 因为状态改变可能需要两帧才反应过来，所以应该加个缓存
        [Header("缓存状态更新帧")]
        public int updateNumber = 5;
        private int stateUpdateCacheNumber;
        private bool stateUpdateCache = false;


        // 临时存储
        private bool isWalk;
        private bool isClimbing;

        private void Awake()
        {
            EventManager.Register(this, EventID.IsCrouching, EventID.Run, EventID.InTheAir, EventID.GraspWall);
        }

        private void Start()
        {
            anim = GetComponent<Animator>();
            pm = GetComponent<PlayerFSMSystem>();
            rb = GetComponentInParent<Rigidbody2D>();

            isWalkId = Animator.StringToHash("isWalk");
            isClimbingId = Animator.StringToHash("isClimbing");
            isCrouchingId = Animator.StringToHash("isCrouching");
            isRunId = Animator.StringToHash("isRun");
            isDeathId = Animator.StringToHash("isDeath");
            inTheAirId = Animator.StringToHash("inTheAir");
            inTheClimbId = Animator.StringToHash("inTheClimb");
            verticalVelocityId = Animator.StringToHash("verticalVelocity");
            isStateUpdateId = Animator.StringToHash("isStateUpdateId");
        }

        // Update is called once per frame
        private void Update()
        {
            // 注意！！！ 这个只管根状态，子状态不管（如果需要则拓展）
            if (stateUpdateCacheNumber >= 1)
            {
                stateUpdateCacheNumber--;
            }


            if (stateUpdateCacheNumber < 1)
            {
                stateUpdateCache = false;
            }

            anim.SetBool(isStateUpdateId, stateUpdateCache);

/*            // 只有状态改变了才调用 SetBool 方法（因为每次调用 SetBool 都会通知 anyState）
            // 反编译看这个 SetBool 方法内部是  MethodImplOptions.InternalCall     所以应该避免更新无意义的动画状态
            if (anim.GetBool(isCrouchingId) != pm.isCrouching) anim.SetBool(isCrouchingId, pm.isCrouching);
            if (anim.GetBool(isRunId) != pm.isRun) anim.SetBool(isRunId, pm.isRun);
            if (anim.GetBool(inTheAirId) != pm.inTheAir) anim.SetBool(inTheAirId, pm.inTheAir);
            if (anim.GetBool(inTheClimbId) != pm.graspWall) anim.SetBool(inTheClimbId, pm.graspWall);
*/


            anim.SetFloat(verticalVelocityId, rb.velocity.y);

            isWalk = Mathf.Abs(pm.xVelocity) > 0.01;
            isClimbing = Mathf.Abs(pm.yVelocity) > 0.01;

            anim.SetBool(isWalkId, isWalk);
            anim.SetBool(isClimbingId, isClimbing);
        }

        public void HandleEvent(EventData resp)
        {
            stateUpdateCacheNumber = updateNumber;
            stateUpdateCache = true;
            anim.SetBool(isStateUpdateId, stateUpdateCache);
            
            switch (resp.eid)
            {
                case EventID.IsCrouching:
                    // anim.SetBool(isCrouchingId, pm.isCrouching);
                    anim.SetBool(isCrouchingId, ((PlayerStateEventData) resp).trigger);
                    break;
                case EventID.Run:
                    // anim.SetBool(isRunId, pm.isRun);
                    anim.SetBool(isRunId, ((PlayerStateEventData) resp).trigger);
                    break;
                case EventID.InTheAir:
                    //  anim.SetBool(inTheAirId, pm.inTheAir);
                    anim.SetBool(inTheAirId, ((PlayerStateEventData) resp).trigger);
                    break;
                case EventID.GraspWall:
                    //  anim.SetBool(inTheClimbId, pm.graspWall);
                    anim.SetBool(inTheClimbId, ((PlayerStateEventData) resp).trigger);
                    break;
            }
        }
    }
}