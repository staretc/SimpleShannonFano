using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonFanoAlgorithm
{
    public class Lexem : IComparable<Lexem>
    {
        public char Value { get; }
        public int Frequency { get; }
        public Lexem(char value, int frequency)
        {
            Value = value;
            Frequency = frequency;
        }

        public int CompareTo(Lexem other)
        {
            if (Frequency == other.Frequency)
            {
                return other.Value.CompareTo(Value);
            }
            return other.Frequency.CompareTo(Frequency);
        }
    }
}
