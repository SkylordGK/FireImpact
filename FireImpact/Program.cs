using FireImpact.Enums;
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
            EffectArea flash = new EffectArea(ref weapon, type: AreaProps.Type.Flash, priority: AreaProps.Priority.Very_High, rotation: 0, lengthFactor: AreaProps.Range.Short, impactFactorAtConeTip: 1, impactFactorAtConeBase: 0.8);
            EffectArea high = new EffectArea(ref weapon, type: AreaProps.Type.High_Effect, priority: AreaProps.Priority.High, rotation: 0, lengthFactor: AreaProps.Range.Long, impactFactorAtConeTip: 0.8, impactFactorAtConeBase: 0.4);
            EffectArea low1 = new EffectArea(ref weapon, type: AreaProps.Type.Low_Effect, priority: AreaProps.Priority.Low, rotation: 90, lengthFactor: AreaProps.Range.Medium, impactFactorAtConeTip: 0.6, impactFactorAtConeBase: 0.2);
            EffectArea low2 = new EffectArea(ref weapon, type: AreaProps.Type.Low_Effect, priority: AreaProps.Priority.Low, rotation: -90, lengthFactor: AreaProps.Range.Medium, impactFactorAtConeTip: 0.6, impactFactorAtConeBase: 0.2);

            //Attach Effect Areas to weapon
            List<EffectArea> effectAreas = new List<EffectArea>() { flash, high, low1, low2 };
            weapon.effectAreas = effectAreas;

            //Create some randomly placed soldiers - X and Y values will be in range of -12 and 12, z = 1.8 
            GameEnvironment.CreateRandomTestUnits(count: 20, maxDistance: 6);

            //Fire!...
            weapon.Fire();

        }

      
    }





}
