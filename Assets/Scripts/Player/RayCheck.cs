using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlsRitter.EventFrame;
using AlsRitter.EventFrame.CustomEvent;
using AlsRitter.GenerateMap.CustomTileFrame;
using AlsRitter.GenerateMap.CustomTileFrame.TileScripts;
using AlsRitter.PlayerController.FSM;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AlsRitter.PlayerController
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerFSMSystem))]
    public class RayCheck : MonoBehaviour
    {
        private PlayerFSMSystem pm;
        public Tilemap tileMap; // 只需取得可碰撞的那个 TileMap


        [Header("当前需要检查的Layer")]
        public LayerMask groundLayer; // 当前需要检查的“地面”的 Layer


        private float footDistance; // 脚距离中心点的距离
        private float handDistance;
        private float topDistance;

        private HandCollision handCollision;
        private HeadCollision headCollision;

        private readonly PlayerStateEventData onGroundEvent;
        private readonly PlayerStateEventData graspWallEvent;
        private readonly PlayerStateEventData onHeadWallEvent;
        private readonly PlayerStateEventData onTopWallEvent;

        // 保存本地状态，只有在状态不一样时才需要更新状态
        private bool isOnGround;
        private bool isOnHeadWall;
        private bool graspWall;
        private bool isOnHeadTop;


        public RayCheck()
        {
            onGroundEvent = new PlayerStateEventData(EventID.OnGround);
            graspWallEvent = new PlayerStateEventData(EventID.GraspWall);
            onHeadWallEvent = new PlayerStateEventData(EventID.OnHeadWall);
            onTopWallEvent = new PlayerStateEventData(EventID.OnTopWall);
        }

        private void Awake()
        {
            pm = GetComponent<PlayerFSMSystem>();

            handCollision = pm.hand.GetComponent<HandCollision>();
            headCollision = pm.head.GetComponent<HeadCollision>();
            pm.handDirection = gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        // Start is called before the first frame update
        private void Start()
        {
            footDistance =
                Mathf.Abs(pm.leftFoot.transform.position.y - pm.coll.transform.TransformPoint(pm.coll.offset).y);
            handDistance = Mathf.Abs(pm.hand.transform.position.x - pm.coll.transform.TransformPoint(pm.coll.offset).x);
            topDistance =
                Mathf.Abs(pm.headTop.transform.position.y - pm.coll.transform.TransformPoint(pm.coll.offset).y);
        }

        private void FixedUpdate()
        {
            PhysicsCheck(); // 射线检查
            HandGetTag();
        }


        /// <summary>
        /// 脚底检查
        /// </summary>
        private void PhysicsCheck()
        {
            Vector2 topPos = pm.headTop.transform.position;
            Vector2 leftPos = pm.leftFoot.transform.position;
            Vector2 rightPos = pm.rightFoot.transform.position;

            var topCheck = Physics2D.Raycast(topPos, Vector2.down, topDistance, groundLayer);

            var leftCheck =
                Physics2D.Raycast(leftPos, Vector2.up, footDistance, groundLayer);

            var rightCheck =
                Physics2D.Raycast(rightPos, Vector2.up, footDistance, groundLayer);

            var footBottom =
                Physics2D.Raycast(leftPos, pm.handDirection, Mathf.Abs(leftPos.x - rightPos.x), groundLayer);


            Debug.DrawRay(leftPos, pm.handDirection * Mathf.Abs(leftPos.x - rightPos.x),
                footBottom ? Color.red : Color.green);

            Debug.DrawRay(topPos, Vector2.down * topDistance, topCheck ? Color.red : Color.green);
            Debug.DrawRay(leftPos, Vector2.up * footDistance, leftCheck ? Color.red : Color.green);
            Debug.DrawRay(rightPos, Vector2.up * footDistance,
                rightCheck ? Color.red : Color.green);

            // 判断当前是否在地面（这里使用了隐式转换，RaycastHit2D 转成 bool类型 标识它是否被击中）
            var temp = leftCheck || rightCheck || footBottom;

            if (isOnGround != temp)
            {
                isOnGround = temp;
                onGroundEvent.UpdateState(isOnGround);
            }

            var temp2 = topCheck;

            if (isOnHeadTop != temp2)
            {
                isOnHeadTop = temp2;
                onTopWallEvent.UpdateState(isOnHeadTop);
            }
        }

        /// <summary>
        /// 横向取得的 砖块 Tag 并修改当前的状态
        /// </summary>
        public void HandGetTag()
        {
            var isLadder = false;

            if (handCollision.handIsTrigger)
            {
                var tileCell = tileMap.WorldToCell(pm.hand.transform.position);
                var tile = tileMap.GetTile<CustomBaseTile>(tileCell);

                if (tile != null)
                {
                    var tagValue = tile.tags.Aggregate(TileTag.Wall, (current, tileTag) => current | tileTag);

                    if ((tagValue & TileTag.Wall) > 0)
                    {
                        //Debug.Log("墙");
                    }

                    isLadder = (tagValue & TileTag.Ladder) > 0;
                }
            }

            // 每帧都需要更新，所以提取出来
            ClimbLadderCheck(isLadder);
        }


        private void ClimbLadderCheck(bool handCheck)
        {
            var blockedCheck = false;
            // 如果当前碰到了楼梯
            if (handCheck && headCollision.headIsTrigger)
            {
                // 还需要检查头部的位置
                var tileCell = tileMap.WorldToCell(pm.head.transform.position);
                var tile = tileMap.GetTile<CustomBaseTile>(tileCell);
                if (tile != null)
                {
                    var tagValue = tile.tags.Aggregate(TileTag.Wall, (current, tileTag) => current | tileTag);
                    //头顶的检查，用于判断是否刚好够到岩壁（头顶不应该被遮住）
                    if ((tagValue & TileTag.Ladder) > 0 || (tagValue & TileTag.Wall) > 0)
                    {
                        blockedCheck = true;
                    }
                }
            }

            Debug.DrawRay(pm.head.transform.position, -pm.handDirection * handDistance,
                blockedCheck ? Color.red : Color.green);
            Debug.DrawRay(pm.hand.transform.position, -pm.handDirection * handDistance,
                handCheck ? Color.red : Color.green);

            // 原本是： pm.graspWall = handCheck;
            if (graspWall != handCheck)
            {
                graspWall = handCheck;
                graspWallEvent.UpdateState(handCheck);
            }

            // 原本是：pm.isOnHeadWall = !blockedCheck;
            if (isOnHeadWall == blockedCheck)
            {
                isOnHeadWall = !blockedCheck;
                onHeadWallEvent.UpdateState(!blockedCheck);
            }
        }
    }
}