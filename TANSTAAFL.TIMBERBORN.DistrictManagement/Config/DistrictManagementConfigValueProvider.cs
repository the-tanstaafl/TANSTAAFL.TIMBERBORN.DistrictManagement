using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.Common;

namespace TANSTAAFL.TIMBERBORN.DistrictManagement.Config
{
    public class DistrictManagementConfigValueProvider
    {
        private static readonly List<int> values = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 16, 18, 20, 25, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100, 125, 150, 175, 200, 250, 300, 350, 400, 500 };

        public static int ObterValor(int posicao)
        {
            return values[posicao - 1];
        }

        public static int ObterPosicao(int value)
        {
            return values.IndexOf(value) + 1;
        }
    }
}
