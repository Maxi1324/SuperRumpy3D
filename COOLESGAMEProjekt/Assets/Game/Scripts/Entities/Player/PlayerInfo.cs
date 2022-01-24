using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Entity.Player
{
    public class PlayerInfo : MonoBehaviour
    {
        public int ControllerNum = 0;
        public int PlayerNum = 0;
        public PlayerSkin Skin = 0;
        public string XAxis = "Horizontal";
        public string YAxis = "Vertical";
        public string A = "Fire1";
        public string B = "Fire2";
        public string C = "Fire3";
        public string Bumper = "Bumper";

        public float Beschleunigung = 40000;
        public float WalkSpeed = 30;
        public float RunSpeed = 60;
        public float RunFastSpeed = 70;
        public float SpeedMultInAir = 0.6f;

        public Vector3 JumpForce = new Vector3(0, 1139.24f, 0);

        public PhysicMaterial PMaterialNormal;
        public PhysicMaterial PMaterialSliding;

        public Rigidbody Rb;
        public Transform Camera;
        public LineRenderer LineRenderer;
        public Animator Anim;
        public Renderer Renderer;
        public Collider Collider;
        public GameObject DieParikel;
        public Transform PHandTransform;
        public List<GameObject> PlayerSkins;

        public float MaxSpeed = 100;

        public object Client { get; internal set; }

        public void CloneInfo(PlayerInfo NewInfo)
        {
            NewInfo.Camera = Camera;
        }

        private void Reset()
        {
            Rb = GetComponent<Rigidbody>();
        }
    }
}