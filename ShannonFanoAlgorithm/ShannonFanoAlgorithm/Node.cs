using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonFanoAlgorithm
{
    public class Node
    {
        public Lexem Symbol { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node(Lexem symbol) : this (symbol, null, null) { }
        public Node(Lexem symbol, Node left, Node right)
        {
            Symbol = symbol;
            Left = left;
            Right = right;
        }
    }
}
