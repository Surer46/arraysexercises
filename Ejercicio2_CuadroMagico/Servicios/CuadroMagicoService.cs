using System;
using System.Collections.Generic;

namespace Ejercicio2_CuadroMagico.Servicios
{
    /// <summary>
    /// Modelo de datos con el diagnóstico completo de un análisis de Cuadrado Mágico.
    /// </summary>
    public class ResultadoCuadroMagico
    {
        public bool EsCuadroMagico { get; set; }
        public int ConstanteMagica { get; set; }
        public int[] SumasFilas { get; set; }
        public int[] SumasColumnas { get; set; }
        public int SumaDiagonalPrincipal { get; set; }
        public int SumaDiagonalSecundaria { get; set; }
        public bool RangoNumerosValido { get; set; }
        public string MensajeDiagnostico { get; set; }
        public string ExplicacionDetallada { get; set; }
    }

    /// <summary>
    /// Clase separada de servicio para el procesamiento y validación de Cuadrados Mágicos.
    /// Demuestra el uso de arreglos bidimensionales y unidimensionales para verificación matemática.
    /// </summary>
    public static class CuadroMagicoService
    {
        /// <summary>
        /// MÉTODO ESPECÍFICO: AnalizarCuadroMagico
        /// ----------------------------------------------------------------------
        /// FUNCIONAMIENTO PASO A PASO:
        /// 1. Valida que la matriz sea cuadrada: filas == columnas == n.
        /// 2. Calcula la suma objetivo esperada tomando como referencia la suma de la fila 0.
        /// 3. Recorre cada fila i (de 0 a n-1) sumando sus columnas j y guarda la suma en SumasFilas[i].
        ///    Si alguna suma de fila difiere de la referencia, no es cuadro mágico.
        /// 4. Recorre cada columna j (de 0 a n-1) sumando sus filas i y guarda la suma en SumasColumnas[j].
        ///    Si alguna suma de columna difiere de la referencia, no es cuadro mágico.
        /// 5. Suma la diagonal principal: matriz[i, i] donde i va de 0 a n-1.
        /// 6. Suma la diagonal secundaria: matriz[i, n - 1 - i] donde i va de 0 a n-1.
        /// 7. Comprueba si los números son consecutivos del 1 al n^2 sin repeticiones mediante un conjunto Hash/Arreglo booleano.
        /// 8. Si todas las sumas son idénticas, se confirma el Cuadrado Mágico y se retorna la Constante Mágica.
        /// </summary>
        /// <param name="matriz">Arreglo bidimensional n x n con los enteros a evaluar.</param>
        /// <returns>Objeto ResultadoCuadroMagico con sumas detalladas y veredicto final.</returns>
        public static ResultadoCuadroMagico AnalizarCuadroMagico(int[,] matriz)
        {
            if (matriz == null)
                throw new ArgumentNullException(nameof(matriz), "La matriz no puede ser nula.");

            int filas = matriz.GetLength(0);
            int columnas = matriz.GetLength(1);

            if (filas != columnas)
            {
                return new ResultadoCuadroMagico
                {
                    EsCuadroMagico = false,
                    MensajeDiagnostico = $"La matriz no es cuadrada ({filas}x{columnas}). Para ser cuadro mágico debe ser NxN."
                };
            }

            int n = filas;
            int[] sumasFilas = new int[n];
            int[] sumasColumnas = new int[n];
            int sumaDiagPrincipal = 0;
            int sumaDiagSecundaria = 0;

            // 1. Sumas de filas
            for (int i = 0; i < n; i++)
            {
                int acumuladorFila = 0;
                for (int j = 0; j < n; j++)
                {
                    acumuladorFila += matriz[i, j];
                }
                sumasFilas[i] = acumuladorFila;
            }

            // 2. Sumas de columnas
            for (int j = 0; j < n; j++)
            {
                int acumuladorCol = 0;
                for (int i = 0; i < n; i++)
                {
                    acumuladorCol += matriz[i, j];
                }
                sumasColumnas[j] = acumuladorCol;
            }

            // 3. Sumas de diagonales
            for (int i = 0; i < n; i++)
            {
                sumaDiagPrincipal += matriz[i, i];
                sumaDiagSecundaria += matriz[i, n - 1 - i];
            }

            // 4. Verificación de coincidencia de sumas
            int constanteObjetivo = sumasFilas[0];
            bool sumasCoinciden = true;

            for (int i = 0; i < n; i++)
            {
                if (sumasFilas[i] != constanteObjetivo || sumasColumnas[i] != constanteObjetivo)
                {
                    sumasCoinciden = false;
                    break;
                }
            }

            if (sumaDiagPrincipal != constanteObjetivo || sumaDiagSecundaria != constanteObjetivo)
            {
                sumasCoinciden = false;
            }

            // 5. Comprobación de números del 1 al n^2 sin repetir (definición estándar de cuadro mágico)
            HashSet<int> conjuntoNumeros = new HashSet<int>();
            bool numerosValidos1AnCuadrado = true;
            int maxEsperado = n * n;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int val = matriz[i, j];
                    if (val < 1 || val > maxEsperado || conjuntoNumeros.Contains(val))
                    {
                        numerosValidos1AnCuadrado = false;
                    }
                    else
                    {
                        conjuntoNumeros.Add(val);
                    }
                }
            }

            ResultadoCuadroMagico resultado = new ResultadoCuadroMagico
            {
                EsCuadroMagico = sumasCoinciden,
                ConstanteMagica = sumasCoinciden ? constanteObjetivo : 0,
                SumasFilas = sumasFilas,
                SumasColumnas = sumasColumnas,
                SumaDiagonalPrincipal = sumaDiagPrincipal,
                SumaDiagonalSecundaria = sumaDiagSecundaria,
                RangoNumerosValido = numerosValidos1AnCuadrado
            };

            if (sumasCoinciden)
            {
                resultado.MensajeDiagnostico = $"¡Es un Cuadrado Mágico! La constante mágica es {constanteObjetivo}.";
                resultado.ExplicacionDetallada = $"Todas las filas suman {constanteObjetivo}, todas las columnas suman {constanteObjetivo} y ambas diagonales suman {constanteObjetivo}." +
                    (numerosValidos1AnCuadrado ? $" Además contiene estrictamente los números del 1 al {maxEsperado} sin repetir." : " (Nota: Utiliza valores personalizados que cumplen la suma armónica).");
            }
            else
            {
                resultado.MensajeDiagnostico = "No es un Cuadrado Mágico.";
                resultado.ExplicacionDetallada = $"Las sumas no coinciden. Fila 1 = {sumasFilas[0]}, Diag Principal = {sumaDiagPrincipal}, Diag Secundaria = {sumaDiagSecundaria}.";
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el ejemplo exacto del PDF de 3x3:
        /// 4 9 2
        /// 3 5 7
        /// 8 1 6
        /// Cuya constante mágica es 15.
        /// </summary>
        public static int[,] ObtenerEjemploPDF()
        {
            return new int[3, 3]
            {
                { 4, 9, 2 },
                { 3, 5, 7 },
                { 8, 1, 6 }
            };
        }

        /// <summary>
        /// Genera un cuadrado mágico válido de tamaño impar (3x3, 5x5, etc.) utilizando el Algoritmo Siamés (De La Loubère).
        /// </summary>
        public static int[,] GenerarCuadroMagicoImpar(int n)
        {
            if (n % 2 == 0)
                throw new ArgumentException("El método siamés aplica para tamaños impares.");

            int[,] matriz = new int[n, n];
            int fila = 0;
            int col = n / 2;

            for (int num = 1; num <= n * n; num++)
            {
                matriz[fila, col] = num;

                int nuevaFila = (fila - 1 + n) % n;
                int nuevaCol = (col + 1) % n;

                if (matriz[nuevaFila, nuevaCol] != 0)
                {
                    fila = (fila + 1) % n;
                }
                else
                {
                    fila = nuevaFila;
                    col = nuevaCol;
                }
            }

            return matriz;
        }
    }
}
