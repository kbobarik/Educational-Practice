using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace REG_MARK_LIB
{
    public class REG_MARK_CLASS
    {
        /// <summary>
        /// Класс для работы с регистрационными номерами автомобилей.
        /// </summary>
        private static readonly char[] ValidLetters = { 'A', 'B', 'E', 'K', 'M', 'H', 'O', 'P', 'C', 'T', 'Y', 'X' };
        private static readonly int[] ValidRegions = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99,
102, 113, 116, 121, 123, 124, 125, 126, 134, 136, 138, 142, 147, 150, 152, 154, 159, 161, 163, 164, 173, 174, 177, 178, 186, 190, 196, 197, 198, 199,
702, 716, 750, 761, 763, 777, 790, 797, 799, 830, 877, 890, 897, 916, 924, 930, 936, 944, 950, 958, 964, 977, 984, 988, 997};

        private readonly Regex MarkPattern = new Regex(@"^[ABEKMHOPCTYX]\d{3}[ABEKMHOPCTYX]{2}\d{2,3}$");

        /// <summary>
        /// Проверяет, является ли номерной знак корректным.
        /// </summary>
        /// <param name="mark">Регистрационный номер автомобиля.</param>
        /// <returns>Возвращает true, если номер корректен, иначе false.</returns>
        public bool CheckMark(string mark)
        {
            if (!MarkPattern.IsMatch(mark)) return false;

            try
            {
                string letters = $"{mark[0]}{mark[4]}{mark[5]}";
                int region = int.Parse(mark.Substring(6));

                return letters.All(c => ValidLetters.Contains(c)) && ValidRegions.Contains(region);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Возвращает следующий номерной знак после указанного.
        /// </summary>
        /// <param name="mark">Регистрационный номер автомобиля, от которого будет вычисляться следующий.</param>
        /// <returns>Следующий регистрационный номер.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если формат номера неверен.</exception>
        public string GetNextMarkAfter(string mark)
        {
            if (!CheckMark(mark)) throw new ArgumentException("Неверный формат номера");

            try
            {
                char letter1 = mark[0];
                char letter2 = mark[4];
                char letter3 = mark[5];
                int number = int.Parse(mark.Substring(1, 3));
                int region = int.Parse(mark.Substring(6));

                number++;
                if (number > 999)
                {
                    number = 0;
                    letter3 = GetNextLetter(letter3);
                    if (letter3 == 'A')
                    {
                        letter2 = GetNextLetter(letter2);
                        if (letter2 == 'A')
                        {
                            letter1 = GetNextLetter(letter1);
                        }
                    }
                }

                return $"{letter1}{number:D3}{letter2}{letter3}{region:D2}";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Возвращает следующий номерной знак в пределах указанного диапазона.
        /// </summary>
        /// <param name="prevMark">Предыдущий номерной знак.</param>
        /// <param name="rangeStart">Начало диапазона.</param>
        /// <param name="rangeEnd">Конец диапазона.</param>
        /// <returns>Следующий номерной знак в пределах диапазона, если он существует, иначе возвращает "out of stock".</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если формат номера неверен.</exception>
        public string GetNextMarkAfterInRange(string prevMark, string rangeStart, string rangeEnd)
        {
            if (!CheckMark(prevMark) || !CheckMark(rangeStart) || !CheckMark(rangeEnd))
                throw new ArgumentException("Неверный формат номера");
            try
            {

                string nextMark = GetNextMarkAfter(prevMark);
                return CompareMarks(nextMark, rangeStart) > 0 && CompareMarks(nextMark, rangeEnd) < 0
                    ? nextMark
                    : "out of stock";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Считает количество комбинаций в диапазоне между двумя номерными знаками.
        /// </summary>
        /// <param name="mark1">Первый номерной знак.</param>
        /// <param name="mark2">Второй номерной знак.</param>
        /// <returns>Количество возможных комбинаций между двумя номерными знаками.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если формат номера неверен.</exception>
        public int GetCombinationsCountInRange(string mark1, string mark2)
        {
            if (!CheckMark(mark1) || !CheckMark(mark2))
                throw new ArgumentException("Неверный формат номера\"");

            try
            {
                return Math.Max(0, Math.Abs(GetMarkIndex(mark1) - GetMarkIndex(mark2)) + 1);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Вспомогательный метод: Получает следующий символ из списка допустимых букв.
        /// </summary>
        /// <param name="current">Текущая буква.</param>
        /// <returns>Следующая буква из списка допустимых.</returns>
        private char GetNextLetter(char current)
        {
            try
            {
                int index = Array.IndexOf(ValidLetters, current);
                return index == ValidLetters.Length - 1 ? 'A' : ValidLetters[index + 1];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Вспомогательный метод: Сравнивает два регистрационных номера.
        /// </summary>
        /// <param name="mark1">Первый номерной знак.</param>
        /// <param name="mark2">Второй номерной знак.</param>
        /// <returns>Результат сравнения двух номерных знаков.</returns>
        private int CompareMarks(string mark1, string mark2) =>
            string.Compare(mark1, mark2, StringComparison.Ordinal);

        /// <summary>
        /// Вспомогательный метод: Получает числовой индекс регистрационного номера.
        /// </summary>
        /// <param name="mark">Номерной знак.</param>
        /// <returns>Числовой индекс для регистрационного номера.</returns>
        private int GetMarkIndex(string mark)
        {
            try
            {
                char letter1 = mark[0];
                char letter2 = mark[4];
                char letter3 = mark[5];
                int number = int.Parse(mark.Substring(1, 3));

                return Array.IndexOf(ValidLetters, letter1) * 12 * 12 * 1000 +
                       Array.IndexOf(ValidLetters, letter2) * 12 * 1000 +
                       Array.IndexOf(ValidLetters, letter3) * 1000 + number;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
