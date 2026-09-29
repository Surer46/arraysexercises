using System;
using System.Collections.Generic;

namespace Ejercicio9_MultiplicacionAvanzada.Servicios
{
    /// <summary>
    /// Servicio de nivel de dificultad alto para operaciones avanzadas de álgebra 
    /// lineal con arreglos bidimensionales: Multiplicación de dimensiones arbitrarias NxM por MxP, 
    /// Matriz Transpuesta, y Determinante mediante cofactores recursivos.
    /// </summary>
    public static class AlgebraMatricialService
    {
        /// <summary>
        /// MÉTODO ESPECÍFICO: MultiplicarMatrices
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO PASO A PASO:
        /// 1. Comprueba la condición matemática fundamental de compatibilidad dimensional:
        ///    El número de columnas de la Matriz A DEBE ser idéntico al número de filas de la Matriz B.
        /// 2. La matriz resultante C tendrá dimensiones [Filas_A, Columnas_B].
        /// 3. Utiliza un algoritmo de triple ciclo anidado (O(N * M * P)):
        ///    - Ciclo 1 (índice i): Recorre las N filas de la matriz A.
        ///    - Ciclo 2 (índice j): Recorre las P columnas de la matriz B.
        ///    - Ciclo 3 (índice k): Itera las M componentes internas calculando el producto escalar:
        ///      C[i, j] += A[i, k] * B[k, j]
        /// 4. Genera una lista de explicaciones paso a paso mostrando la fórmula desarrollada de cada celda.
        /// </summary>
        public static double[,] MultiplicarMatrices(double[,] matrizA, double[,] matrizB, out List<string> desglosePasos)
        {
            if (matrizA == null || matrizB == null)
                throw new ArgumentNullException("Las matrices no pueden ser nulas.");

            int filasA = matrizA.GetLength(0);
            int colsA = matrizA.GetLength(1);
            int filasB = matrizB.GetLength(0);
            int colsB = matrizB.GetLength(1);

            if (colsA != filasB)
            {
                throw new InvalidOperationException($"Incompatibilidad dimensional: Las columnas de A ({colsA}) no coinciden con las filas de B ({filasB}).");
            }

            double[,] matrizC = new double[filasA, colsB];
            desglosePasos = new List<string>();

            for (int i = 0; i < filasA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    double suma = 0;
                    List<string> terminos = new List<string>();

                    for (int k = 0; k < colsA; k++)
                    {
                        double producto = matrizA[i, k] * matrizB[k, j];
                        suma += producto;
                        terminos.Add($"({matrizA[i, k]} × {matrizB[k, j]})");
                    }

                    matrizC[i, j] = Math.Round(suma, 2);
                    desglosePasos.Add($"C[{i + 1},{j + 1}] = {string.Join(" + ", terminos)} = {matrizC[i, j]}");
                }
            }

            return matrizC;
        }

        /// <summary>
        /// MÉTODO ESPECÍFICO: Transponer
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO:
        /// Invierte filas por columnas en una nueva matriz de dimensiones [Columnas, Filas]:
        /// Transpuesta[j, i] = Original[i, j].
        /// </summary>
        public static double[,] Transponer(double[,] matriz)
        {
            if (matriz == null) throw new ArgumentNullException(nameof(matriz));
            int filas = matriz.GetLength(0);
            int cols = matriz.GetLength(1);

            double[,] transpuesta = new double[cols, filas];
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    transpuesta[j, i] = matriz[i, j];
                }
            }
            return transpuesta;
        }

        /// <summary>
        /// MÉTODO ESPECÍFICO: CalcularDeterminante (Algoritmo Recursivo de Laplace)
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO:
        /// 1. Caso base 1x1: det = A[0,0].
        /// 2. Caso base 2x2: det = (A[0,0] * A[1,1]) - (A[0,1] * A[1,0]).
        /// 3. Para N > 2: Expansión por cofactores a lo largo de la primera fila.
        ///    det += (-1)^j * A[0, j] * det(Submatriz_sin_fila0_sin_colj).
        /// </summary>
        public static double CalcularDeterminante(double[,] matriz)
        {
            if (matriz == null) throw new ArgumentNullException(nameof(matriz));
            int n = matriz.GetLength(0);
            if (n != matriz.GetLength(1))
                throw new InvalidOperationException("Para calcular el determinante la matriz debe ser cuadrada.");

            if (n == 1) return matriz[0, 0];
            if (n == 2) return (matriz[0, 0] * matriz[1, 1]) - (matriz[0, 1] * matriz[1, 0]);

            double det = 0;
            for (int j = 0; j < n; j++)
            {
                double[,] submatriz = ObtenerSubmatriz(matriz, 0, j);
                double signo = (j % 2 == 0) ? 1.0 : -1.0;
                det += signo * matriz[0, j] * CalcularDeterminante(submatriz);
            }

            return Math.Round(det, 4);
        }

        private static double[,] ObtenerSubmatriz(double[,] matriz, int filaExcluida, int colExcluida)
        {
            int n = matriz.GetLength(0);
            double[,] sub = new double[n - 1, n - 1];
            int rSub = 0;

            for (int r = 0; r < n; r++)
            {
                if (r == filaExcluida) continue;
                int cSub = 0;
                for (int c = 0; c < n; c++)
                {
                    if (c == colExcluida) continue;
                    sub[rSub, cSub] = matriz[r, c];
                    cSub++;
                }
                rSub++;
            }

            return sub;
        }
    }
}
