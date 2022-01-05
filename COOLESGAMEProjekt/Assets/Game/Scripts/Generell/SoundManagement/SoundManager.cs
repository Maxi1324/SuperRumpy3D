using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Generell.SoundManagement
{
    public class SoundManager:MonoBehaviour
    {
        private static SoundManager Instance;

        [Serializable]
        public struct AudioClips1
        {
            public string Name;
            public AudioClip Clip;
            public bool PlayOnAwake;
            public bool PlayAsync;
            public bool Loop;
            public Sound3D is3D;
            public float volume;
        }

        [Serializable]
        public struct Sound3D{
            public bool Is3D;
            public int min;
            public int max;
        }

        public List<AudioClips1 > Sounds = new List<AudioClips1>();

        public struct Source
        {
            public AudioSource source { get; set; }
            public Func<AudioSource, AudioSource> Play { get; set; }
        }
        public Dictionary<string, Source> Clips = new Dictionary<string, Source>();

        private void Start()
        {
            Instance = this;
            Sounds.ForEach(AC =>
            {
                AudioSource s = null;
                if (!AC.PlayAsync)
                {
                    GameObject ob = new GameObject();
                    ob.transform.parent = transform;
                    ob.AddComponent<AudioSource>();
                    s = ob.GetComponent<AudioSource>();
                    s.clip = AC.Clip;
                    s.playOnAwake = AC.PlayOnAwake;
                    s.volume = AC.volume + 1;
                    if (AC.is3D.Is3D)
                    {
                        s.minDistance = AC.is3D.min;
                        s.maxDistance = AC.is3D.max;
                        s.spatialBlend = 1;
                        s.rolloffMode = AudioRolloffMode.Linear;
                    }
                    else
                    {
                        s.spatialBlend = 0;
                    }
                    if (AC.PlayOnAwake)
                    {
                        s.Play();
                    }
                    s.loop = AC.Loop;
                }
                Func<AudioSource, AudioSource> Play1 =(s3) =>
                {
                    s3.Play();
                    return s3;
                };
                Func<AudioSource,AudioSource> PlayAsnc = (s3) =>
                {
                    GameObject ob1 = new GameObject();
                    ob1.transform.parent = transform;
                    ob1.AddComponent<AudioSource>();
                    AudioSource s1 = ob1.GetComponent<AudioSource>();
                    s1.clip = AC.Clip;
                    s1.playOnAwake = AC.PlayOnAwake;
                    s1.loop = AC.Loop;
                    s1.volume = AC.volume +1;
                    if (AC.is3D.Is3D)
                    {
                        s1.minDistance = AC.is3D.min;
                        s1.maxDistance = AC.is3D.max;
                        s1.spatialBlend = 1;
                        s1.rolloffMode = AudioRolloffMode.Linear;
                    }
                    else
                    {
                        s1.spatialBlend = 0;
                    }
                    s1.Play();
                    StartCoroutine(deleteAudios(s1));
                    s1.Play();

                    return s1;
                };
                Clips.Add(AC.Name, (new Source() { source = s, Play = !AC.PlayAsync?Play1: PlayAsnc }) );
            });
        }

        public static AudioSource Play(string Name, Vector3 pos = new Vector3())
        {
            Source S = Instance.Clips[Name];
            AudioSource source = S.source;
           
            AudioSource wow = S.Play(source);
            if (pos != null && wow != null)
            {
                wow.transform.position = pos;
            }
            return wow;
        }

        IEnumerator deleteAudios(AudioSource AS)
        {
            while (AS.isPlaying)
            {
                yield return null;
            }
            Destroy(AS.gameObject);
        }
    }
}
