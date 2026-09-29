using System;

namespace Ejercicio4_MatrizIdentidad.Servicios
{
    /// <summary>
    /// Servicio independiente encargado de la lógica algorítmica para la 
    /// generación y análisis de matrices identidad de orden N x N.
    /// </summary>
    public static class MatrizIdentidadService
    {
        /// <summary>
        /// MÉTODO ESPECÍFICO: GenerarMatrizIdentidad
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO PASO A PASO:
        /// 1. Valida que el orden N sea un entero positivo mayor o igual a 1.
        /// 2. Instancia un arreglo bidimensional 'matriz' de dimensiones [n, n].
        /// 3. Recorre cada fila i desde 0 hasta n-1.
        /// 4. Recorre cada columna j desde 0 hasta n-1.
        /// 5. Evalúa la condición de diagonal principal:
        ///    - Si i == j: Se asigna 1 (la diagonal principal).
        ///    - Si i != j: Se asigna 0 (posiciones no diagonales).
        /// 6. Complejidad temporal: O(N^2) con N^2 iteraciones exactas.
        /// 7. Retorna la matriz identidad resultante.
        /// </summary>
        /// <param name="n">Orden o dimensión de la matriz cuadrada.</param>
        /// <returns>Arreglo bidimensional int[n, n] con la matriz identidad.</returns>
        public static int[,] GenerarMatrizIdentidad(int n)
        {
            if (n < 1)
            {
                throw new ArgumentException("El tamaño de la matriz debe ser al menos 1x1.", nameof(n));
            }

            int[,] matriz = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        matriz[i, j] = 1; // Diagonal principal
                    }
                    else
                    {
                        matriz[i, j] = 0; // Demás posiciones
                    }
                }
            }

            return matriz;
        }

        /// <summary>
        /// Comprueba si una matriz dada cumple la definición formal de matriz identidad.
        /// </summary>
        public static bool EsMatrizIdentidad(int[,] matriz)
        {
            if (matriz == null) return false;
            int filas = matriz.GetLength(0);
            int columnas = matriz.GetLength(1);

            if (filas != columnas || filas == 0) return false;

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    if (i == j && matriz[i, j] != 1) return false;
                    if (i != j && matriz[i, j] != 0) return false;
                }
            }

            return true;
        }
    }
}
