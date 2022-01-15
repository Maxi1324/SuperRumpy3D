using UnityEngine;
using Entity.Player;
using Obstacles;
using Obstacles.MoveableBlocks;
using UI.InGameUi;
using System.Collections;
using System;

namespace Entity.Player.Abilities
{
    public class MoveBlocks : MovementAbility
    {
        public PlayerManager Player;
        public float MinDis = 1;

        private GameObject Tooltip;
        private bool IsFollowing;
        private bool TooltipTrunedOff;

        private InteractPlayer isShowing;

        private void Reset()
        {
            Player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            NeedObject = true;
        }

        public override bool Active(float distance, InteractPlayer ob, bool allowed)
        {
            if (ob.key == "Block")
            {
                if (distance <= MinDis)
                {
                    if (Tooltip == null && allowed && isShowing == null && !TooltipTrunedOff)
                    {
                        Tooltip = WorldSpaceUiFunctions.Instance.ShowTooltip(UI.InGameUi.Tooltip.A, Player.PInfo.PHandTransform.position);
                        StartCoroutine(WorldSpaceUiFunctions.Instance.FollowTransform(Player.PInfo.PHandTransform, Tooltip.transform));
                        isShowing = ob;
                        TooltipTrunedOff = true;
                    }

                    if (Player.Fire3Pressed && allowed)
                    {
                        Destroy(Tooltip);

                        MoveableBlock MB = ob.GetComponent<MoveableBlock>();


                        Vector3 dir = MB.Move(transform, () => {
                            st();
                        });
                        //if (dir != Vector3.zero) return false;
                        dir = new Vector3(dir.x, 0, dir.z).normalized;
                        Player.transform.parent = ob.transform;
                        transform.position = ob.transform.position - (dir * 4.5f);

                        StartCoroutine(Follow(transform, ob.transform, (-dir * 13) + Vector3.down * 2));

                        Player.AllowedMoves = -1;

                        Player.transform.LookAt(ob.transform);

                        Vector3 euler = Player.transform.rotation.eulerAngles;
                        Player.transform.rotation = Quaternion.Euler(0, euler.y, 0);

                        Player.PInfo.Anim.Play("OnKisteSurfen");
                        Player.PInfo.Anim.SetBool("OnKistSurfen", true);


                        StartCoroutine(DoInNSec(() =>
                        {
                            StartCoroutine(Wait());
                        }, 1));
                        return true;
                    }
                }
                else
                {
                    if(ob == isShowing)
                    {
                        TooltipTrunedOff = false;
                        isShowing = null;
                        if (Tooltip != null)
                        {
                            Destroy(Tooltip);
                        }
                    }
                }
            }
            return false;
        }

        public void st()
        {
            Player.AllowedMoves = 0;
            Player.PInfo.Anim.SetBool("OnKistSurfen", false);
            Player.WantStandOnPlatt = true;
            Player.PInfo.Rb.detectCollisions = true;
            Player.PInfo.Rb.isKinematic = false;
            Player.transform.parent = null;
            IsFollowing = false;
        }

        public override bool Allowed(int allowedMoves)
        {
            return allowedMoves == 0 || allowedMoves == 1;
        }

        IEnumerator Wait()
        {
            while (IsFollowing)
            {
                if(Player.XAxis != 0 || Player.YAxis != 0)
                {
                    st();
                }
                yield return null;
            }
        }

        IEnumerator Follow(Transform trans,Transform Goal, Vector3 offset)
        {
            IsFollowing = true;
            while (IsFollowing)
            {
                trans.position = Goal.position + offset;
                yield return null;
            }
        }

        public static IEnumerator DoInNSec(Action ac, int n)
        {
            yield return new WaitForSeconds(n);
            ac();
        }
    }
}
