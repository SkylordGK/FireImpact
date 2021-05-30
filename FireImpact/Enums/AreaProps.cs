using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireImpact.Enums
{

    public struct AreaProps
    {

        public enum Type
        {
            Flash,
            High_Effect,
            Low_Effect
        }


        public enum Priority
        {
            Very_High = 0,
            High = 1,
            Medium = 2,
            Low = 3,
            Very_Low = 4
        }

        public enum Range
        {
            Short = 100,
            Medium = 200,
            Long = 350

        }

    }

}
