using Entity.Player.Abilities;
using Generell.SoundManagement;
using Obstacles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Entity.Player
{
    public class PlayerManager : Entity1, IDamageHandler
    {
        public PlayerInfo PInfo;
        public SimplePlayerMovement SPMovement;

        public List<MovementAbility> Moves;

        public int AllowedMoves { get; set; } = 0;
        public bool WantStandOnPlatt { get; set; } = true;
        //-1 = nichts
        // 0 = Normal Default Herumstehen 
        // 1 = In der Lufe
        // 2 = Grappling Hook
        // 3 = BetterJump
        // 4 = Rutschen
        // 5 = LongJump

        public Transform groundTrans;
        public LayerMask groundMask;
        public float MaxGroundDis = 1.5f;
        public AudioSource DieSound;

        public string added { get; set; }
        public float XAxis { get; set; }
        public float YAxis { get; set; }
        public bool Run { get; set; }
        public bool JumpPressed { get; set; }
        public bool Jump { get; set; }
        public bool Fire3Pressed { get; set; }
        public bool Fire3Up { get; set; }
        public bool Fire3 { get; set; }
        public bool BumperDown { get; set; }
        public bool Bumper { get; set; }
        public bool BumperUp { get; set; }

        private bool BumperWasDown = false;

        public bool OnGround { get; set; }
        public Vector3 OnGroundNormal { get; set; }
        public bool Invincible { get; set; } = false;

        public int aktLeben { get; private set; }

        public override IDamageHandler DamageHandler => this;

        public int Startleben = 2;
        private Vector3 StartScale;

        private void Start()
        {
            StartScale = transform.localScale;
            aktLeben = Startleben;

            GameObject ob = PInfo.PlayerSkins[(int)PInfo.Skin];
            ob.SetActive(true);
            Skin skin = ob.GetComponent<Skin>();
            PInfo.Anim = skin.Anim;

            for (int i = 0; i < 3; i++) {
                if (PlayerPrefs.GetInt(("Ab" + i)) == 0)
                {
                    MovementAbility Ab = null;
                    switch (i)
                    {
                        case 0:
                            Ab = GetComponent<GrapplingHook>();
                        break;
                        case 1:
                            Ab = GetComponent<LongJump>();
                            break;
                        case 2:
                            Ab = GetComponent<HighJump>();
                            break;
                    }
                    Moves.Remove(Ab);
                    Ab.enabled = false;
                }
            }
        }

        private void Reset()
        {
            PInfo = GetComponent<PlayerInfo>();
            SPMovement = GetComponent<SimplePlayerMovement>();
        }

        private void Update()
        {
            float y = PInfo.Rb.velocity.y;

            added = (PInfo.ControllerNum == 0) ? "" : PInfo.ControllerNum.ToString();
            XAxis = Input.GetAxis(PInfo.XAxis + added);
            YAxis = Input.GetAxis(PInfo.YAxis + added);
            Run = Input.GetButton(PInfo.B + added);
            JumpPressed = Input.GetButtonDown(PInfo.A + added);
            Jump = Input.GetButton(PInfo.A + added);
            Fire3Pressed = Input.GetButtonDown(PInfo.C + added);
            Fire3 = Input.GetButton(PInfo.C + added);
            Fire3Up = Input.GetButtonUp(PInfo.C + added);
            float Bumper1 = Input.GetAxis(PInfo.Bumper + added);

            BumperUp = Mathf.Abs(Bumper1) < 0.1f && !BumperWasDown;
            BumperDown = (Mathf.Abs(Bumper1) > 0.8f) && BumperWasDown;
            Bumper = (Mathf.Abs(Bumper1) > 0.8f);

           //Debug.Log($"Bumper:{Bumper} BumperDown:{BumperDown} BumerpUp:{BumperUp}");
           // Debug.Log(Bumper1);

            if((Mathf.Abs(Bumper1) > 0.8f))
            {
                BumperWasDown = false;
            }
            if (Mathf.Abs(Bumper1) < 0.1f)
            {
                BumperWasDown = true;
            }

            Tuple<bool, Vector3> t = checkGround(MaxGroundDis);
            OnGround = t.Item1;
            OnGroundNormal = t.Item2;

            float abnahme = 0.97f;
            if (XAxis == 0 && YAxis == 0)
            {
                abnahme = 0.94f;
            }
            if(AllowedMoves != 5 && AllowedMoves != 2)PInfo.Rb.velocity = new Vector3(PInfo.Rb.velocity.x * abnahme, (y < 0f && y > -100) ? y * 1.03f : y, PInfo.Rb.velocity.z * abnahme);
            FrameAnim();

            InteractPlayer[] InteractPlayersA = FindObjectsOfType<InteractPlayer>();
            List<InteractPlayer> InteractPlayers = new List<InteractPlayer>(InteractPlayersA);
            InteractPlayers.Sort(new comp());
            for (int i = 0; i < InteractPlayers.Count; i++)
            {
                InteractPlayer InteractPlayer = InteractPlayers[i];
                float dis = Vector3.Distance(InteractPlayer.transform.position, transform.position);
                Moves.ForEach((MovementAbility ability) =>
                {
                    if (ability.NeedObject)
                        ability.Active(dis, InteractPlayer, ability.Allowed(AllowedMoves));
                });
            }

            Moves.ForEach((MovementAbility ability) =>
            {
                if (!ability.NeedObject)
                {
                    ability.Active(-1, null, ability.Allowed(AllowedMoves));
                }
            });

            if (JumpPressed && AllowedMoves == 0 && OnGround) SPMovement.InitJump();
            if (AllowedMoves == 0) SPMovement.SimpleMovement(XAxis, YAxis, Run, 1);
            if (AllowedMoves == 1) SPMovement.SimpleMovement(XAxis, YAxis, Run, PInfo.SpeedMultInAir);
            // MaxSpeed();
        }

        private void FrameAnim()
        {
            Animator Anim = PInfo.Anim;
            Tuple<bool,Vector3> t = checkGround(MaxGroundDis * 3);
            bool animBoden = t.Item1;
            Anim.SetBool("onGround", animBoden);
            Anim.SetBool("isWalking", false);
            Anim.SetInteger("stateRunning", 0);
        }

        private void MaxSpeed()
        {
            Rigidbody rigidbody = PInfo.Rb;
            if (rigidbody.velocity.magnitude > PInfo.MaxSpeed)
            {
                Vector3 newV = rigidbody.velocity.normalized * PInfo.MaxSpeed;
                rigidbody.velocity = new Vector3(newV.x, rigidbody.velocity.y, newV.z);
            }
        }

        private Tuple<bool,Vector3> checkGround(float length)
        {
            RaycastHit hit;
            bool hit1 = Physics.Raycast(groundTrans.position, -transform.up, out hit, length, groundMask);

            return new Tuple<bool,Vector3>(hit1, hit.normal);
        }

        public void Spawn(Vector3 pos)
        {
            transform.position = pos;
            aktLeben = Startleben;
            scale(StartScale);
        }

        public void scale(Vector3 Scale)
        {
            Transform trans = transform.parent;
            transform.parent = null;
            gameObject.transform.localScale = Scale;
            transform.parent = trans;
        }

        public void Die()
        {
            DieSound.Play();
            GameObject ob = Instantiate(PInfo.DieParikel, transform.position, transform.rotation);
            StartCoroutine(DestroyParticle(ob.GetComponent<ParticleSystem>()));
            Invincible = true;
            StartCoroutine(DoInNSec(() =>
            {
                gameObject.SetActive(false);
                Invincible = false;
            }, 1));
        }

        public void Heal()
        {
            aktLeben++;
            transform.localScale = new Vector3(StartScale.x , StartScale.y , StartScale.z );
        }

        public void Hit(Vector3 EintrittsDir, Vector3 Rueckstoss, bool showAnim, int mult)
        {
            if (!Invincible)
            {
                aktLeben -= mult;
                if (aktLeben <= 0)
                {
                    Die();
                }
                else
                {
                    if (showAnim)
                    {
                        AllowedMoves = -1;
                        PInfo.Collider.enabled = false;
                        PInfo.Rb.isKinematic = true;

                        StartCoroutine("BecomeInvincible");
                        StartCoroutine(DoInNSec(() =>
                        {
                            AllowedMoves = 0;
                            PInfo.Collider.enabled = true;
                            PInfo.Rb.isKinematic = false;
                        }, 1));
                        PInfo.Anim.Play("RFallen", 0);
                    }
                    PInfo.Rb.velocity = Vector3.zero;
                    PInfo.Rb.AddForce(Rueckstoss);

                    scale(new Vector3(StartScale.x * 0.9f, StartScale.y * 0.5f, StartScale.z * 0.9f));
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag == "Hit1")
            {
                Hit(new Vector3(0, 0, 0), new Vector3(0, 0, 0), true, 1);
            }   
        }

        public static IEnumerator DoInNSec(Action ac, int n)
        {
            yield return new WaitForSeconds(n);
            ac();
        }

        public IEnumerator BecomeInvincible()
        {
            Invincible = true;
            for(int i = 0; i < 150; i++)
            {
                PInfo.Renderer.enabled = !PInfo.Renderer.enabled;
                Invincible = true;
                yield return new WaitForSeconds(0.02f);
            }
            PInfo.Renderer.enabled = true;
            Invincible = false;
        }

        IEnumerator DestroyParticle(ParticleSystem Sys)
        {
            Sys.Play();
            while (Sys.isPlaying)
            {
                yield return null;
            }
            Destroy(Sys.gameObject);
        }
    }

    public class comp : IComparer<InteractPlayer>
    {
        public int Compare(InteractPlayer x, InteractPlayer y)
        {
            return x.transform.position.magnitude.CompareTo(y.transform.position.magnitude);
        }
    }
}