using System;

namespace UnityEngine.Productivity.Attributes
{
    public enum ButtonAvailability
    {
        Always,
        Editor,
        Play
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string Name { get; private set; }
        public ButtonAvailability Availability { get; private set; }
        
        public ButtonAttribute() {}

        public ButtonAttribute(string name)
        {
            Name = name;
        }
        
        public ButtonAttribute(ButtonAvailability availability)
        {
            Availability = availability;
        }

        /// <summary>
        /// Adds a button to the inspector to invoke the method with this attribute
        /// </summary>
        /// <param name="name">Name that shows up on the button</param>
        /// <param name="availability">The availability of the button (Design Time, Play or both).</param>
        public ButtonAttribute(string name, ButtonAvailability availability)
        {
            Name = name;
            Availability = availability;
        }
    }
}
