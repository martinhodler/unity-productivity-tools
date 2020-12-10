using System;

namespace UnityEngine.Productivity.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public float Min { get; private set; }
        public float Max { get; private set; }
        
        public float StepSize { get; private set; }
        
        public MinMaxRangeAttribute(float min, float max, float stepSize = 1f)
        {
            StepSize = stepSize;

            if (Min <= Max)
            {
                Min = min;
                Max = max;
            }
            else
            {
                Min = max;
                Max = min;
            }
        }
    }
}