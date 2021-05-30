using MathNet.Spatial.Euclidean;
using System;

namespace FireImpact
{
    /// <summary>
    /// This is a helper class to simulate data will be fetched from Ganme Environment.
    /// </summary>
    public static class GameEnvironment
    {
        /// <summary>
        /// SetBlindness funciton as stated in the Co
        /// </summary>
        /// <param name="soldier">Soldier who will be blinded.</param>
        /// <param name="duration">Duration of Blindness</param>
        /// <remarks>Writes info about soldier and duration to console</remarks>
        public static void SetBlindness(Soldier soldier, double duration)
        {
            if (duration > 0)
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



}
