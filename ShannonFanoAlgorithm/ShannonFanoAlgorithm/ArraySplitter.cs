using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonFanoAlgorithm
{
    /// <summary>
    /// Класс деления массива данных
    /// </summary>
    public static class ArraySplitter
    {
        /// <summary>
        /// Разбиение массива на максимально равные части по частотам
        /// </summary>
        /// <param name="chars">Список элементов, которые нужно разбить пополам</param>
        /// <returns></returns>
        public static Tuple<List<Lexem>, List<Lexem>> SplitArrayInHalf(List<Lexem> chars)
        {
            int totalSum = chars.Sum(item => item.Frequency);   // сумма частот 
            int targetSum = totalSum / 2;   // целевая сумма, которую стремимся получить

            // Матрица анализа массива:
            // - Строки представляют элементы массива(от 0 до n, где n — количество элементов)
            // - Столбцы представляют возможные суммы(от 0 до targetSum, где targetSum — половина суммы всех элементов массива)
            // 
            // Каждая ячейка dp[i, j] содержит булево значение:
            // - true, если можно получить сумму j с использованием первых i элементов массива
            // - false, если такую сумму получить нельзя
            bool[,] dataMatrix = new bool[chars.Count + 1, targetSum + 1];

            // Инициализация
            for (int elemNum = 0; elemNum <= chars.Count; elemNum++)
                dataMatrix[elemNum, 0] = true;

            // Для каждого элемента массива(numbers[indxNum - 1]) и каждой возможной суммы sum:
            // - Если текущий элемент меньше или равен sum, то проверяем,
            // можно ли получить сумму sum без этого элемента (dataMatrix[indxNum - 1, sum])
            // или с ним (dataMatrix[indxNum - 1, sum - numbers[indxNum - 1]])
            // - Если текущий элемент больше sum, то его нельзя использовать, и результат равен dataMatrix[indxNum - 1, sum]
            for (int indxNum = 1; indxNum <= chars.Count; indxNum++)
            {
                var currentNum = chars[indxNum - 1].Frequency;
                for (int sum = 1; sum <= targetSum; sum++)
                {
                    if (sum >= currentNum)
                    {
                        dataMatrix[indxNum, sum] = dataMatrix[indxNum - 1, sum] || dataMatrix[indxNum - 1, sum - currentNum];
                    }
                    else
                    {
                        dataMatrix[indxNum, sum] = dataMatrix[indxNum - 1, sum];
                    }
                }
            }

            // Находим максимальную сумму, которую можно получить, не превышая targetSum
            // Это будет сумма первой части
            int maxSum = 0;
            for (int j = targetSum; j >= 0; j--)
            {
                if (dataMatrix[chars.Count, j])
                {
                    maxSum = j;
                    break;
                }
            }

            // Восстанавливаем подмножество
            // Проходим по массиву с конца
            // - Если текущий элемент может быть частью подмножества с суммой remainingSum,
            // т.е dataMatrix[i - 1, remainingSum - numbers[i - 1]] == true,
            // добавляем его в первую часть и уменьшаем remainingSum
            // - Иначе добавляем элемент во вторую часть
            List<Lexem> firstPart = new List<Lexem>();
            List<Lexem> secondPart = new List<Lexem>();
            int remainingSum = maxSum;

            for (int i = chars.Count; i > 0; i--)
            {
                if (remainingSum >= chars[i - 1].Frequency && dataMatrix[i - 1, remainingSum - chars[i - 1].Frequency])
                {
                    firstPart.Add(chars[i - 1]);
                    remainingSum -= chars[i - 1].Frequency;
                }
                else
                {
                    secondPart.Add(chars[i - 1]);
                }
            }

            return new Tuple<List<Lexem>, List<Lexem>>(firstPart, secondPart);
        }
    }
}
