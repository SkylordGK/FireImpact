using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using System;

namespace FireImpact
{
    /// <summary>
    /// Class <c>EffectArea</c> models a cone shaped area
    /// </summary>
    /// <remarks>Uses Degrees for Angles</remarks>
    public class EffectArea
    {

        public double Rotation { get; private set; }
        public double ConeAngle { get; private set; }
        public Weapon Weapon { get; }
        private double LengthFactor { get; set; }
        public double ImpactFactorAtConeTip { get; private set; }
        public double ImpactFactorAtConeBase { get; private set; }



        /// <summary>This constructor initializes a new  <c>EffectArea</c> in which the soldiers will be temprorily blinded and/or deafened
        /// when the owner weapon of this  <c>EffectArea</c> is fired.
        /// </summary>
        /// <param name="rotation">Rotation RELATIVE to muzzle direction, in Degrees</param>
        /// <param name="coneAngle">Arc of Cone. Default set to 50.</param>
        /// <param name="lengthFactor">This parameter will be multiplied by the muzzle caliber and used to find the cone Length. (aka. r, Radius at the documentation image)</param>
        /// <param name="impactFactorAtConeTip">This parameter is used to calculate MAX. blindness and deafness durations of this <c>EffectArea</c>.
        /// Durations will decrease linearly as the distance from muzzle increases and will reach to MIN. at the cone base.</param>
        /// <param name="impactFactorAtConeBase">This parameter is used to calculate MIN. blindness and deafness durations of this <c>EffectArea</c>.
        /// Durations will increase linearly as the distance from muzzle decreases and will reach to MAX. at the cone tip.</param>
        public EffectArea(ref Weapon weapon, double rotation, double lengthFactor, double impactFactorAtConeTip, double impactFactorAtConeBase, double coneAngle = 50)
        {
            Rotation = rotation;
            LengthFactor = lengthFactor;
            ImpactFactorAtConeTip = impactFactorAtConeTip;
            ImpactFactorAtConeBase = impactFactorAtConeBase;
            ConeAngle = coneAngle;
            Weapon = weapon;

        }

        /// <summary>
        /// Multiplies the LengthFactor of this <c>EffectArea</c> and the muzzleCaliber of the <c>Weapon</c> it's attached to.
        /// </summary>
        /// <returns>Length of <c>EffectArea in meters. (AKA "r, radius" at documentation image.)</c></returns>
        public double GetLength()
        {
            //Convert muzzleCaliber from milimeters to meters, then multiply by LengthFactor 
            return Weapon.muzzleCaliber / 1000 * LengthFactor;
        }
        public Vector3D GetDirection()
        {

            return Weapon.muzzleDir.Rotate(UnitVector3D.ZAxis, Angle.FromDegrees(this.Rotation));
        }
        public double CalculateBlindnessTime(double distanceToMuzzle)
        {

            double maxTime = 2.0;
            var impactRate = distanceToMuzzle / this.GetLength() * ImpactFactorAtConeBase;
            impactRate = impactRate > 1 ? 1 : impactRate;

            return maxTime * impactRate;
        }
        public bool IsPointInsideEffectArea(Vector3D point)
        {

            var distance = point - Weapon.muzzlePos;
            var cone_dist = distance.DotProduct(Weapon.muzzleDir);

            var h = GetLength();
            var r = h * Math.Tan((Math.PI / 180) * 50);

            var cone_radius = (cone_dist / h) * r;


            if (cone_dist <= 0 || cone_dist >= h)
            {
                return false;
            }


            var orth_distance = (distance - cone_dist * Weapon.muzzleDir).Length;
            var is_point_inside_cone = (orth_distance < cone_radius);

            return is_point_inside_cone;
        }

    }

}
