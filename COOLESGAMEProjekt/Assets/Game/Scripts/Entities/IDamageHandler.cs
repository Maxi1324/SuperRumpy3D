using UnityEngine;

namespace Entity
{
    public interface IDamageHandler
    {
        int aktLeben { get;}
        void Hit(Vector3 EintrittsDir, Vector3 Rueckstoss, bool showAnimation, int DamageMultiplikator);
        void Spawn(Vector3 pos);
        void Die();
        void Heal();
    }
}
