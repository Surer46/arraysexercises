using System;

namespace Ejercicio5_EstadisticasMatriz5x10.Servicios
{
    /// <summary>
    /// Modelo de datos que almacena la matriz principal y los 4 arreglos unidimensionales resultantes (A, B, C, D).
    /// </summary>
    public class ReporteEstadisticasMatriz
    {
        public int[,] Matriz { get; set; }
        /// <summary>Arreglo A: Suma de cada fila (tamaño 5)</summary>
        public int[] ArregloA_SumaFilas { get; set; }
        /// <summary>Arreglo B: Promedio de cada fila (tamaño 5)</summary>
        public double[] ArregloB_PromedioFilas { get; set; }
        /// <summary>Arreglo C: Suma de cada columna (tamaño 10)</summary>
        public int[] ArregloC_SumaColumnas { get; set; }
        /// <summary>Arreglo D: Promedio de cada columna (tamaño 10)</summary>
        public double[] ArregloD_PromedioColumnas { get; set; }
    }

    /// <summary>
    /// Servicio desacoplado que implementa los algoritmos de suma y promedio por fila y columna.
    /// </summary>
    public static class EstadisticasMatrizService
    {
        public const int FILAS = 5;
        public const int COLUMNAS = 10;

        /// <summary>
        /// MÉTODO ESPECÍFICO: CalcularEstadisticas
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO PASO A PASO:
        /// 1. Inicializa los 4 arreglos unidimensionales:
        ///    - Arreglo A (sumas de filas, tamaño = filas).
        ///    - Arreglo B (promedios de filas, tamaño = filas).
        ///    - Arreglo C (sumas de columnas, tamaño = columnas).
        ///    - Arreglo D (promedios de columnas, tamaño = columnas).
        /// 2. Cálculo por filas (Arreglos A y B):
        ///    - Para cada fila i de 0 a 4:
        ///      - Itera sus 10 columnas j acumulando la suma en 'sumaFila'.
        ///      - Asigna ArregloA[i] = sumaFila.
        ///      - Asigna ArregloB[i] = sumaFila / 10.0 (promedio).
        /// 3. Cálculo por columnas (Arreglos C y D):
        ///    - Para cada columna j de 0 a 9:
        ///      - Itera sus 5 filas i acumulando la suma en 'sumaCol'.
        ///      - Asigna ArregloC[j] = sumaCol.
        ///      - Asigna ArregloD[j] = sumaCol / 5.0 (promedio).
        /// 4. Retorna el objeto contenedor con los 4 arreglos calculados.
        /// </summary>
        public static ReporteEstadisticasMatriz CalcularEstadisticas(int[,] matriz)
        {
            if (matriz == null)
                throw new ArgumentNullException(nameof(matriz), "La matriz no puede ser nula.");

            int numFilas = matriz.GetLength(0);
            int numCols = matriz.GetLength(1);

            int[] arregloA = new int[numFilas];
            double[] arregloB = new double[numFilas];
            int[] arregloC = new int[numCols];
            double[] arregloD = new double[numCols];

            // 1. Recorrido por FILAS -> Llenado de Arreglos A y B
            for (int i = 0; i < numFilas; i++)
            {
                int sumaFila = 0;
                for (int j = 0; j < numCols; j++)
                {
                    sumaFila += matriz[i, j];
                }
                arregloA[i] = sumaFila;
                arregloB[i] = Math.Round((double)sumaFila / numCols, 2);
            }

            // 2. Recorrido por COLUMNAS -> Llenado de Arreglos C y D
            for (int j = 0; j < numCols; j++)
            {
                int sumaCol = 0;
                for (int i = 0; i < numFilas; i++)
                {
                    sumaCol += matriz[i, j];
                }
                arregloC[j] = sumaCol;
                arregloD[j] = Math.Round((double)sumaCol / numFilas, 2);
            }

            return new ReporteEstadisticasMatriz
            {
                Matriz = matriz,
                ArregloA_SumaFilas = arregloA,
                ArregloB_PromedioFilas = arregloB,
                ArregloC_SumaColumnas = arregloC,
                ArregloD_PromedioColumnas = arregloD
            };
        }

        /// <summary>
        /// Retorna la matriz 5x10 visualizada en el formato de práctica:
        /// Fila 0: 2 4 5 1 2 1 1 4 4 2 (Suma: 26, Prom: 2.6)
        /// Fila 1: 3 4 4 8 4 2 3 6 5 4
        /// Fila 2: 5 2 9 4 6 7 7 8 4 8
        /// Fila 3: 8 8 7 9 3 8 6 2 9 3
        /// Fila 4: 9 4 2 6 1 5 2 7 2 5
        /// Columna 0: 2, 3, 5, 8, 9 (Suma: 27, Prom: 5.4)
        /// </summary>
        public static int[,] ObtenerMatrizEjemploPDF()
        {
            return new int[5, 10]
            {
                { 2, 4, 5, 1, 2, 1, 1, 4, 4, 2 },
                { 3, 4, 4, 8, 4, 2, 3, 6, 5, 4 },
                { 5, 2, 9, 4, 6, 7, 7, 8, 4, 8 },
                { 8, 8, 7, 9, 3, 8, 6, 2, 9, 3 },
                { 9, 4, 2, 6, 1, 5, 2, 7, 2, 5 }
            };
        }

        /// <summary>
        /// Genera una matriz 5x10 con enteros aleatorios en el rango [1, 9].
        /// </summary>
        public static int[,] GenerarMatrizAleatoria()
        {
            Random rng = new Random();
            int[,] matriz = new int[FILAS, COLUMNAS];
            for (int i = 0; i < FILAS; i++)
            {
                for (int j = 0; j < COLUMNAS; j++)
                {
                    matriz[i, j] = rng.Next(1, 10);
                }
            }
            return matriz;
        }
    }
}
