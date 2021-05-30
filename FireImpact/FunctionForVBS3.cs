using System;

namespace FireImpact
{
    public static class FunctionForVBS3
    {
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
                    x.Priority.CompareTo(y.Priority));


            foreach (var soldier in allUnits)
            {
                foreach (var zone in weaponFired.effectAreas)
                {
                    if (zone.IsPointInsideEffectArea(soldier.Positon))
                    {
                        var duration = GetDuration(soldier, zone, GameEnvironment.MaxBlindnessDuration);
                        GameEnvironment.SetBlindness(soldier, duration);

                        duration = GetDuration(soldier, zone, GameEnvironment.MaxDeafnessDuration);
                        GameEnvironment.SetDeafness(soldier, duration);


                        Console.WriteLine("Sorry Soldier_{0}. Your position {1} was in the {2} zone. \n ", soldier.Id, soldier.Positon, zone.Type);


                        //Effect Areas can overlap each other
                        //If a soldier is inside of multiple Effect Areas, only the highest proirity EffectArea will be taken into account.
                        //Effect Area Iteration is from highest priorirty to lowest.
                        //So after a soldier is affected by an Effect Area, we will "break" this iteration to prevent lower priority Effect Areas making affects.
                        //Soldier Iteration will continue with next soldier.
                        break;

                    }
                }

            }
            //To prevent console screen from closing.
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

            return Math.Round( impactFactor * maxDuration, 2);
        }

    }
}
