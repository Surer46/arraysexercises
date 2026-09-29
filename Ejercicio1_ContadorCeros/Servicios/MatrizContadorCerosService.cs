using System;

namespace Ejercicio1_ContadorCeros.Servicios
{
    /// <summary>
    /// Clase desacoplada de la interfaz gráfica que contiene la lógica pura 
    /// para el análisis de arreglos bidimensionales (matrices).
    /// </summary>
    public static class MatrizContadorCerosService
    {
        /// <summary>
        /// MÉTODO ESPECÍFICO: ContarCerosPorRenglon
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO PASO A PASO:
        /// 1. Obtiene la cantidad de filas (renglones) mediante matriz.GetLength(0).
        /// 2. Obtiene la cantidad de columnas mediante matriz.GetLength(1).
        /// 3. Crea un arreglo unidimensional 'conteoPorFila' de tamaño igual al número de filas.
        /// 4. Recorre fila por fila (índice i) y dentro de cada fila recorre cada columna (índice j).
        /// 5. Comprueba si el elemento actual matriz[i, j] es igual a cero (0).
        /// 6. Si es cero, incrementa el contador de la fila actual conteoPorFila[i].
        /// 7. Complejidad temporal: O(F * C) donde F es filas y C es columnas.
        /// 8. Retorna el arreglo unidimensional con la cantidad de ceros de cada renglón.
        /// </summary>
        /// <param name="matriz">Arreglo bidimensional de números enteros.</param>
        /// <returns>Arreglo de enteros donde cada posición i representa los ceros de la fila i.</returns>
        public static int[] ContarCerosPorRenglon(int[,] matriz)
        {
            if (matriz == null)
            {
                throw new ArgumentNullException(nameof(matriz), "La matriz proporcionada no puede ser nula.");
            }

            int filas = matriz.GetLength(0);
            int columnas = matriz.GetLength(1);
            int[] conteoPorFila = new int[filas];

            // Recorrido por renglón (fila)
            for (int i = 0; i < filas; i++)
            {
                int contadorRenglon = 0;

                // Recorrido por cada elemento del renglón (columnas)
                for (int j = 0; j < columnas; j++)
                {
                    if (matriz[i, j] == 0)
                    {
                        contadorRenglon++;
                    }
                }

                // Guardamos el resultado en la posición correspondiente al renglón
                conteoPorFila[i] = contadorRenglon;
            }

            return conteoPorFila;
        }

        /// <summary>
        /// Calcula la suma total de ceros en toda la matriz sumando el arreglo de ceros por fila.
        /// </summary>
        public static int ContarTotalCeros(int[] cerosPorRenglon)
        {
            if (cerosPorRenglon == null) return 0;
            int total = 0;
            for (int i = 0; i < cerosPorRenglon.Length; i++)
            {
                total += cerosPorRenglon[i];
            }
            return total;
        }

        /// <summary>
        /// Retorna la matriz de números especificada exactamente en el documento de evaluación:
        /// 0 2 5 7 6
        /// 0 0 0 3 8
        /// 2 9 6 3 4
        /// 1 5 6 1 4
        /// 0 9 2 5 0
        /// </summary>
        public static int[,] ObtenerMatrizOriginalDelProblema()
        {
            return new int[5, 5]
            {
                { 0, 2, 5, 7, 6 },
                { 0, 0, 0, 3, 8 },
                { 2, 9, 6, 3, 4 },
                { 1, 5, 6, 1, 4 },
                { 0, 9, 2, 5, 0 }
            };
        }

        /// <summary>
        /// Genera una matriz de 5x5 con números aleatorios entre 0 y 9 para pruebas de usuario.
        /// </summary>
        public static int[,] GenerarMatrizAleatoria(int filas = 5, int columnas = 5)
        {
            Random rng = new Random();
            int[,] matriz = new int[filas, columnas];
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    // Probabilidad equilibrada de que aparezcan ceros (30% de probabilidad de 0)
                    matriz[i, j] = rng.Next(0, 10) < 3 ? 0 : rng.Next(1, 10);
                }
            }
            return matriz;
        }
    }
}
