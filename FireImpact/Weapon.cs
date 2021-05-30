using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;

namespace FireImpact
{
    /// <summary>
    /// Class <c>Weapon</c> models a simple weapon with a muzzle.
    /// </summary>
    public class Weapon
    {
        public double muzzleCaliber;
        public Vector3D muzzleDir;
        public Vector3D muzzlePos;
        public List<EffectArea> effectAreas;



        /// <summary>
        /// Initializes a waeapon at <paramref name="muzzlePos"/>
        /// with a muzzle looks toward <paramref name="muzzleDir"/>
        /// with a caliber of <paramref name="muzzleCaliber"/> 
        /// </summary>
        /// <param name="muzzleCaliber">Radius of muzzle, in milimeters.</param>
        /// <param name="muzzleDir">The [X,Y,Z] vector the muzzle is pointing.</param>
        /// <param name="muzzlePos">The [X,Y,Z] position of the muzzle, Z is height.</param>
        public Weapon(double muzzleCaliber, Vector3D muzzleDir, Vector3D muzzlePos)
        {
            this.muzzleCaliber = muzzleCaliber;
            this.muzzleDir = muzzleDir;
            this.muzzlePos = muzzlePos;
        }


        public void Fire()
        {
            Console.WriteLine("Weapon is fired towards {0} direction when muzzle position was {1}. \n", muzzleDir, muzzlePos);

            // If muzzleCaliber is greater dan 15 weapon will be considered as large weapon.
            if (muzzleCaliber > 15)
                FunctionForVBS3.ApplyBlindnessAndDeafness(GameEnvironment.allUnits, this);
        }

    }
}
