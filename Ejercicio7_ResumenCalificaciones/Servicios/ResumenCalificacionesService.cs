using System;

namespace Ejercicio7_ResumenCalificaciones.Servicios
{
    public class ReporteCalificaciones
    {
        public double[] PromediosAlumnos { get; set; }
        public double PromedioMasAlto { get; set; }
        public int AlumnoPromedioAltoIdx { get; set; }
        public double PromedioMasBajo { get; set; }
        public int AlumnoPromedioBajoIdx { get; set; }
        public int TotalParcialesReprobados { get; set; }
        public int[] DistribucionFrecuencias { get; set; }
    }

    /// <summary>
    /// Servicio desacoplado que contiene la lógica estadística para el análisis 
    /// de matrices de calificaciones escolares.
    /// </summary>
    public static class ResumenCalificacionesService
    {
        public const int TOTAL_ALUMNOS = 12;
        public const int TOTAL_PARCIALES = 3;

        public static readonly string[] NombresRangos = new string[6]
        {
            "0 – 4.9",
            "5.0 – 5.9",
            "6.0 – 6.9",
            "7.0 – 7.9",
            "8.0 – 8.9",
            "9.0 – 10.0"
        };

        /// <summary>
        /// Retorna la matriz de calificaciones de 12 alumnos x 3 parciales 
        /// correspondiente a la tabla oficial del PDF.
        /// </summary>
        public static double[,] ObtenerCalificacionesIniciales()
        {
            return new double[TOTAL_ALUMNOS, TOTAL_PARCIALES]
            {
                { 5.5,  8.6, 10.0 }, // Alumno 1
                { 8.0,  5.5, 10.0 }, // Alumno 2
                { 9.0,  4.1,  7.8 }, // Alumno 3
                { 10.0, 2.2,  8.1 }, // Alumno 4
                { 7.0,  9.2,  7.1 }, // Alumno 5
                { 9.0,  4.0,  6.0 }, // Alumno 6
                { 6.5,  5.0,  5.0 }, // Alumno 7
                { 4.0,  7.0,  4.0 }, // Alumno 8
                { 8.0,  8.0,  9.0 }, // Alumno 9
                { 10.0, 9.0,  9.2 }, // Alumno 10
                { 5.0,  10.0, 8.4 }, // Alumno 11
                { 9.0,  4.6,  7.5 }  // Alumno 12
            };
        }

        /// <summary>
        /// MÉTODO ESPECÍFICO: AnalizarCalificaciones
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO PASO A PASO:
        /// Resuelve los 5 incisos solicitados:
        /// 1. Inciso (a): Recorre cada alumno i (fila) calculando la suma de sus 3 parciales j y dividiendo entre 3.
        ///    Guarda el promedio en un arreglo unidimensional 'promedios'.
        /// 2. Inciso (b) y (c): Recorre el arreglo de promedios buscando el valor máximo y mínimo, y los índices de alumno correspondientes.
        /// 3. Inciso (d): Recorre cada celda matriz[i,j]; si el parcial es menor estricto que 7.0, incrementa 'totalReprobados'.
        /// 4. Inciso (e): Clasifica cada promedio final en los 6 intervalos definidos en la rúbrica:
        ///    [0, 4.9], [5.0, 5.9], [6.0, 6.9], [7.0, 7.9], [8.0, 8.9], [9.0, 10.0].
        /// </summary>
        public static ReporteCalificaciones AnalizarCalificaciones(double[,] matriz)
        {
            if (matriz == null) throw new ArgumentNullException(nameof(matriz));
            int numAlumnos = matriz.GetLength(0);
            int numParciales = matriz.GetLength(1);

            double[] promedios = new double[numAlumnos];
            int totalReprobados = 0;

            // 1. Inciso a: Promedio de cada alumno y conteo de parciales reprobados (Inciso d)
            for (int i = 0; i < numAlumnos; i++)
            {
                double sumaAlumno = 0;
                for (int j = 0; j < numParciales; j++)
                {
                    double nota = matriz[i, j];
                    sumaAlumno += nota;

                    // Inciso d: Parciales menores a 7.0
                    if (nota < 7.0)
                    {
                        totalReprobados++;
                    }
                }
                promedios[i] = Math.Round(sumaAlumno / numParciales, 2);
            }

            // 2. Incisos b y c: Promedio más alto y más bajo
            double promAlto = promedios[0];
            int idxAlto = 0;

            double promBajo = promedios[0];
            int idxBajo = 0;

            for (int i = 0; i < numAlumnos; i++)
            {
                if (promedios[i] > promAlto)
                {
                    promAlto = promedios[i];
                    idxAlto = i;
                }

                if (promedios[i] < promBajo)
                {
                    promBajo = promedios[i];
                    idxBajo = i;
                }
            }

            // 3. Inciso e: Distribución de frecuencias de calificaciones finales
            // Rangos: [0] 0-4.9, [1] 5.0-5.9, [2] 6.0-6.9, [3] 7.0-7.9, [4] 8.0-8.9, [5] 9.0-10
            int[] distribucion = new int[6];

            for (int i = 0; i < numAlumnos; i++)
            {
                double p = promedios[i];
                if (p < 5.0) distribucion[0]++;
                else if (p < 6.0) distribucion[1]++;
                else if (p < 7.0) distribucion[2]++;
                else if (p < 8.0) distribucion[3]++;
                else if (p < 9.0) distribucion[4]++;
                else distribucion[5]++;
            }

            return new ReporteCalificaciones
            {
                PromediosAlumnos = promedios,
                PromedioMasAlto = promAlto,
                AlumnoPromedioAltoIdx = idxAlto,
                PromedioMasBajo = promBajo,
                AlumnoPromedioBajoIdx = idxBajo,
                TotalParcialesReprobados = totalReprobados,
                DistribucionFrecuencias = distribucion
            };
        }
    }
}
