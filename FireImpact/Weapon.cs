using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;

namespace FireImpact
{
    public class Weapon
    {
        public double muzzleCaliber;
        public Vector3D muzzleDir;
        public Vector3D muzzlePos;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="muzzleCaliber"></param>
        /// <param name="muzzleDir"></param>
        /// <param name="muzzlePos"></param>
        public Weapon(double muzzleCaliber, Vector3D muzzleDir, Vector3D muzzlePos)
        {
            this.muzzleCaliber = muzzleCaliber;
            this.muzzleDir = muzzleDir;
            this.muzzlePos = muzzlePos;
        }

        public List<EffectArea> effectAreas;

        //  public delegate void ApplyBlindnessAndDeafnessDelegate(Soldier[] allUnits);


        //public event EventHandler Fire;

        public void Fires()
        {
            Console.WriteLine("Weapon is fired towards {0} direction when muzzle position was {1}.", muzzleDir, muzzlePos);
        }

    }
}
