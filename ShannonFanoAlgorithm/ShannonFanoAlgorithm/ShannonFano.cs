using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonFanoAlgorithm
{
    public class ShannonFano
    {
        #region Fields
        /// <summary>
        /// Входящий текст
        /// </summary>
        private string _inputText;
        #endregion

        #region Properties
        /// <summary>
        /// Словарь кодирования символов исходного текста
        /// </summary>
        public Dictionary<char, string> EncodingDictionary { get; private set; }
        /// <summary>
        /// Словарь декодирования символов зашифрованного текста
        /// </summary>
        public Dictionary<string, char> DecodingDictionary { get; private set; }
        /// <summary>
        /// Степень сжатия кодировки
        /// </summary>
        public int CompressionRatio { get; private set; }
        #endregion

        #region Constructors
        public ShannonFano()
        {
            EncodingDictionary = new Dictionary<char, string>();
            DecodingDictionary = new Dictionary<string, char>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Кодирует входящую строку по созданному словарю
        /// </summary>
        public string EncodeString()
        {
            var sb = new StringBuilder();
            var inputTextBitsCount = _inputText.Length * 16;
            var outputTextBitsCount = 0;
            foreach (var lexem in _inputText)
            {
                if (!EncodingDictionary.ContainsKey(lexem))
                {
                    throw new ArgumentNullException("Словарь не содержит код для данного символа!");
                }
                sb.Append(EncodingDictionary[lexem]);
                outputTextBitsCount += EncodingDictionary[lexem].Length;
            }
            return sb.ToString();
        }
        /// <summary>
        /// Декодирует входящиую зашифрованную строку по созданному словарю
        /// </summary>
        public string DecodeString(string path)
        {
            string inputText = File.ReadAllText(path);
            StringBuilder sb = new StringBuilder();
            string currentString = "";

            foreach (var chr in inputText)
            {
                if (!(chr == '1' || chr == '0'))
                {
                    throw new ArgumentException("Файл содержит неверные символы!");
                    
                }
                currentString += chr;
                if (DecodingDictionary.ContainsKey(currentString))
                {
                    sb.Append(DecodingDictionary[currentString]);
                    currentString = "";
                }
            }
            if (currentString != "")
            {
                throw new ArgumentException("Словарь не содержит код для данного символа!");
            }
            return sb.ToString();
        }
        /// <summary>
        /// Создание кодирующего и декодирующего словарей алгоритмом Шеннона-Фано
        /// </summary>
        /// <param name="fileName">Путь к файлу</param>
        public void CreateShannonFanoEncodingDictionary(string fileName)
        {
            _inputText = File.ReadAllText(fileName).ToLower();
            if (_inputText == null)
            {
                throw new ArgumentNullException();
            }
            EncodingDictionary.Clear();
            DecodingDictionary.Clear();
            List<Lexem> priorityChars = MakePriorities(_inputText);
            Node root = CreateBinaryTree(priorityChars);
            EncodeChars(root, "");
            DecodingDictionary = EncodingDictionary.ToDictionary(x => x.Value, x => x.Key);
        }
        /// <summary>
        /// Составление списка символов по приоритету (по частоте появления в тексте)
        /// Less frequency - more priority.
        /// </summary>
        /// <param name="inputText">Текст, который нужно закодировать</param>
        /// <returns></returns>
        private List<Lexem> MakePriorities(string inputText)
        {
            Dictionary<char, int> frequencyDictionary = FrequencyCounter(inputText);
            List<Lexem> priorityChars = new List<Lexem>();
            foreach (var lexem in frequencyDictionary)
            {
                priorityChars.Add(new Lexem(lexem.Key, lexem.Value));
            }
            priorityChars.Sort();
            return priorityChars;
        }
        /// <summary>
        /// Составление частотного словаря по символам входного текста
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns>Текст, который нужно закодировать</returns>
        private Dictionary<char, int> FrequencyCounter(string inputText)
        {
            Dictionary<char, int> counter = new Dictionary<char, int>();
            for (int i = 0; i < inputText.Length; i++)
            {
                if (counter.ContainsKey(inputText[i]))
                {
                    counter[inputText[i]]++;
                }
                else
                {
                    counter[inputText[i]] = 1;
                }
            }
            return counter;
        }
        private Node CreateBinaryTree(List<Lexem> priorityChars)
        {
            if (priorityChars.Count == 1)
            {
                return new Node(null, new Node(priorityChars[0]), null);
            }
            var splittedChars = ArraySplitter.SplitArrayInHalf(priorityChars);
            var node = new Node(null);
            node.Left = splittedChars.Item1.Count == 1 ? new Node(splittedChars.Item1[0]) : CreateBinaryTree(splittedChars.Item1);
            node.Right = splittedChars.Item2.Count == 1 ? new Node(splittedChars.Item2[0]) : CreateBinaryTree(splittedChars.Item2);

            return node;
        }
        /// <summary>
        /// Кодирование символов по дереву
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="code">Текущий код</param>
        private void EncodeChars(Node root, string code)
        {
            if (root == null)
                return;
            if (root.Left == null && root.Right == null)
            {
                EncodingDictionary[root.Symbol.Value] = code;
            }

            EncodeChars(root.Left, code + "0");
            EncodeChars(root.Right, code + "1");
        }
        #endregion
    }
}
