using System;

namespace Ejercicio3_OperacionesMatrices2x2.Servicios
{
    /// <summary>
    /// Servicio desacoplado para realizar operaciones aritméticas elementales 
    /// entre arreglos bidimensionales (matrices) de tamaño 2x2.
    /// </summary>
    public static class Matrices2x2Service
    {
        /// <summary>
        /// MÉTODO ESPECÍFICO: SumarMatrices
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO:
        /// Recorre cada renglón i y columna j sumando los elementos correspondientes:
        /// C[i, j] = A[i, j] + B[i, j]
        /// </summary>
        public static double[,] Sumar(double[,] matrizA, double[,] matrizB)
        {
            ValidarDimensiones(matrizA, matrizB);
            double[,] resultado = new double[2, 2];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    resultado[i, j] = matrizA[i, j] + matrizB[i, j];
                }
            }

            return resultado;
        }

        /// <summary>
        /// MÉTODO ESPECÍFICO: RestarMatrices
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO:
        /// Resta término a término los elementos de la primera matriz menos la segunda:
        /// C[i, j] = A[i, j] - B[i, j]
        /// </summary>
        public static double[,] Restar(double[,] matrizA, double[,] matrizB)
        {
            ValidarDimensiones(matrizA, matrizB);
            double[,] resultado = new double[2, 2];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    resultado[i, j] = matrizA[i, j] - matrizB[i, j];
                }
            }

            return resultado;
        }

        /// <summary>
        /// MÉTODO ESPECÍFICO: ProductoSimple
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO:
        /// Realiza el producto Hadamard o elemento a elemento entre ambas matrices:
        /// C[i, j] = A[i, j] * B[i, j]
        /// </summary>
        public static double[,] ProductoSimple(double[,] matrizA, double[,] matrizB)
        {
            ValidarDimensiones(matrizA, matrizB);
            double[,] resultado = new double[2, 2];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    resultado[i, j] = matrizA[i, j] * matrizB[i, j];
                }
            }

            return resultado;
        }

        /// <summary>
        /// MÉTODO ESPECÍFICO: DivisionSimple
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO:
        /// Divide elemento a elemento la matriz A entre la matriz B:
        /// C[i, j] = A[i, j] / B[i, j]
        /// Valida explícitamente que ningún divisor sea 0 para prevenir excepciones aritméticas.
        /// </summary>
        public static double[,] DivisionSimple(double[,] matrizA, double[,] matrizB)
        {
            ValidarDimensiones(matrizA, matrizB);
            double[,] resultado = new double[2, 2];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (Math.Abs(matrizB[i, j]) < 0.0000001)
                    {
                        throw new DivideByZeroException($"División por cero en posición [{i + 1}, {j + 1}]: B[{i + 1}, {j + 1}] es 0.");
                    }
                    resultado[i, j] = Math.Round(matrizA[i, j] / matrizB[i, j], 2);
                }
            }

            return resultado;
        }

        private static void ValidarDimensiones(double[,] a, double[,] b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Las matrices no pueden ser nulas.");

            if (a.GetLength(0) != 2 || a.GetLength(1) != 2 || b.GetLength(0) != 2 || b.GetLength(1) != 2)
                throw new ArgumentException("Ambas matrices deben tener tamaño exacto 2 x 2.");
        }

        /// <summary>
        /// Retorna las matrices del ejemplo del PDF:
        /// Matriz 1 = [10, 5; 8, 2]
        /// Matriz 2 = [2, 4; 6, 8]
        /// </summary>
        public static (double[,] m1, double[,] m2) ObtenerEjemploPDF()
        {
            double[,] m1 = new double[2, 2]
            {
                { 10, 5 },
                { 8, 2 }
            };

            double[,] m2 = new double[2, 2]
            {
                { 2, 4 },
                { 6, 8 }
            };

            return (m1, m2);
        }
    }
}
