using System;

namespace Financials.Infrastructure.Helpers
{
    public static class RandomHelper
    {
        public static string GetRandomName(Random random)
        {
            // Gera um número aleatório entre 0 e 99
            int randomNumber = random.Next(100);

            // Condições baseadas nas probabilidades
            if (randomNumber < 70) // 0-69 corresponde a 70%
            {
                return "Anonymous";
            }
            else if (randomNumber < 80) // 70-79 corresponde a 10%
            {
                return "Adolfo";
            }
            else if (randomNumber < 90) // 80-89 corresponde a 10%
            {
                return "Davi";
            }
            else // 90-99 corresponde a 10%
            {
                return "Lucas";
            }
        }
    }
}
