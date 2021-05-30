using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using System;
using System.Collections.Generic;

namespace FireImpact
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a weapon
            Weapon weapon = new Weapon(muzzleCaliber: 40, muzzleDir: new Vector3D(1, 0, 0), muzzlePos: new Vector3D(0, 0, 2));

            //Create Effect Areas as shown at the documentation image
            EffectArea eaFlash = new EffectArea(ref weapon, rotation: 0, lengthFactor: 50, impactFactorAtConeTip: 1, impactFactorAtConeBase: 0.8);
            EffectArea eaHigh = new EffectArea(ref weapon, rotation: 0, lengthFactor: 250, impactFactorAtConeTip: 1, impactFactorAtConeBase: 0.4);
            EffectArea eaLow_1 = new EffectArea(ref weapon, rotation: 90, lengthFactor: 150, impactFactorAtConeTip: 0.7, impactFactorAtConeBase: 0.2);
            EffectArea eaLow_2 = new EffectArea(ref weapon, rotation: -90, lengthFactor: 150, impactFactorAtConeTip: 0.7, impactFactorAtConeBase: 0.2);

            //Attach Effect Areas to weapon
            List<EffectArea> effectAreas = new List<EffectArea>() { eaFlash, eaHigh, eaLow_1, eaLow_2 };
            weapon.effectAreas = effectAreas;

            //Create some randomly placed soldiers - X and Y values will be in range of -12 and 12, z = 1.8 
            GameUtils.CreateTestUnits(count: 30, maxDistance: 12);

            //TODO: weapon fire
            ApplyBlindnessAndDeafness(GameUtils.allUnits, weapon);


        }

        /// <summary>
        /// This method iterates <paramref name="allUnits"/> and applies deafness and blindness if necessary.
        /// </summary>
        /// <param name="allUnits">Array of all soldiers to iterate through.</param>
        public static void ApplyBlindnessAndDeafness(Soldier[] allUnits, Weapon weaponFired)
        {
            //TODO: add priority and name

            if (allUnits == null || weaponFired == null)
                return;


            //Sort Effect Areas from highest priority to lowest. 
            //To be sure about Iteration will occure in priority order. 
            weaponFired.effectAreas.Sort((x, y) =>
                    x.ImpactFactorAtConeTip.CompareTo(y.ImpactFactorAtConeTip));


            foreach (var soldier in allUnits)
            {
                foreach (var zone in weaponFired.effectAreas)
                {
                    if (zone.IsPointInsideEffectArea(soldier.Positon))
                    {
                        var duration = GetDuration(soldier, zone, GameUtils.MaxBlindnessDuration);
                        GameUtils.SetBlindness(soldier, duration);

                        duration = GetDuration(soldier, zone, GameUtils.MaxBlindnessDuration);
                        GameUtils.SetDeafness(soldier, duration);


                        Console.WriteLine("Sorry Soldier_{0}. Your position {1} was in the {2} zone. \n ", soldier.Id, soldier.Positon);


                        //Effect Areas can overlap each other
                        //If a soldier is inside of multiple Effect Areas, only the highest proirity EffectArea will be taken into account.
                        //Effect Area Iteration is from highest priorirty to lowest.
                        //So after a soldier is affected by an Effect Area, we will "break" this iteration to prevent lower priority Effect Areas making affects.
                        //Soldier Iteration will continue with next soldier.
                        break;

                    }
                }

            }

            Console.ReadLine();

        }


        /// <summary>
        /// Calculates durations for blindness/deafness.
        /// 
        /// </summary>
        /// <param name="soldier">Soldier who will be blinded/deafened</param>
        /// <param name="effecArea">Effect Area that soldier is in.</param>
        /// <param name="maxDuration">Maximum Duration for blindness or deafness effect.</param>
        /// <returns></returns>
        private static double GetDuration(Soldier soldier, EffectArea effecArea, double maxDuration)
        {
            var distance = Math.Abs((soldier.Positon - effecArea.Weapon.muzzlePos).Length);
            var distanceRate = distance / effecArea.GetLength();
            var impactFactor = (effecArea.ImpactFactorAtConeTip - effecArea.ImpactFactorAtConeBase) * distanceRate + effecArea.ImpactFactorAtConeBase;

            //If somehow an improper impactFactor is entered for the zone, that will prevent exceeding maximum duration allowed.
            impactFactor = impactFactor > 1 ? 1 : impactFactor; 

            return impactFactor * maxDuration;
        }

      
    }



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


    /// <summary>
    /// This is a helper class to simulate data will be fetched from Ganme Environment.
    /// </summary>
    static class GameUtils
    {
        /// <summary>
        /// SetBlindness funciton as stated in the Co
        /// </summary>
        /// <param name="soldier">Soldier who will be blinded.</param>
        /// <param name="duration">Duration of Blindness</param>
        /// <remarks>Writes info about soldier and duration to console</remarks>
        public static void SetBlindness(Soldier soldier, double duration)
        {
            if(duration > 0)
            Console.WriteLine("Soldier_{0} will be BLIND for next {1} seconds", soldier.Id, duration);
        }

        /// <summary>
        /// SetBlindness funciton as stated in the Co
        /// </summary>
        /// <param name="soldier">Soldier who will be blinded.</param>
        /// <param name="duration">Duration of Deafness</param>
        /// <remarks>Writes info about soldier and duration to console</remarks>
        public static void SetDeafness(Soldier soldier, double duration)
        {
            Console.WriteLine("Soldier_{0} will be DEAF for next {1} seconds", soldier.Id, duration);

        }


        public const double MaxBlindnessDuration = 2.0d;
        public const double MaxDeafnessDuration = 60.0d;

        //Array of all soldiers in Game Environmetn.. 
        public static Soldier[] allUnits;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">Number of soldiers that will be initialized.</param>
        /// <param name="maxDistance"> X and Y values of soldier positon will </param>
        public static void CreateTestUnits(int count, int maxDistance)
        {

            allUnits = new Soldier[count];
            Random r = new Random();

            var z = 1.8; //all soldiers are considered as standing, and they are 1.8 meters.

            for (int i = 0; i < count; i++)
            {
                var x = r.Next(-maxDistance, maxDistance);
                var y = r.Next(-maxDistance, maxDistance);
                allUnits[i] = new Soldier(i, new Vector3D(x, y, z));
            }


        }

    }


    /// <summary>
    /// <c>Soldier</c> class   . Soldiers are represented as point.
    /// </summary>
    public class Soldier
    {
        public int Id { get; set; }
        public Vector3D Positon { get; set; }

        public Soldier(int id, Vector3D positon)
        {
            Id = id;
            Positon = positon;
        }

        // standing ducked crawling

    }



}
