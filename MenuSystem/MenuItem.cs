using System;

namespace MenuSystem
{
    public sealed class MenuItem
    {
        public MenuItem(string label, Func<int> methodToExecute)
        {
            Label = label.Trim();
            MethodToExecute = methodToExecute;
        }

        public string Label { get; set; }


        public Func<int> MethodToExecute { get; set; } = null!;

        public bool OutOfMenu { get; set; }

        public string GetLabel()
        {
            return Label;
        }


        public override string ToString()
        {
            return $"This is a meny Item with the label of {Label}";
        }
    }
}