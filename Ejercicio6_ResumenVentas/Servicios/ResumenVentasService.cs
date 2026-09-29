using System;

namespace Ejercicio6_ResumenVentas.Servicios
{
    public class ReporteVentas
    {
        public double VentaMenor { get; set; }
        public string MesMenor { get; set; }
        public string DiaMenor { get; set; }
        public int FilaMenor { get; set; }
        public int ColumnaMenor { get; set; }

        public double VentaMayor { get; set; }
        public string MesMayor { get; set; }
        public string DiaMayor { get; set; }
        public int FilaMayor { get; set; }
        public int ColumnaMayor { get; set; }

        public double VentaTotal { get; set; }
        public double[] VentasPorDia { get; set; }
        public double[] VentasPorMes { get; set; }
    }

    /// <summary>
    /// Servicio desacoplado de la interfaz gráfica para el procesamiento de arreglos 
    /// bidimensionales aplicados a análisis comercial de ventas anuales.
    /// </summary>
    public static class ResumenVentasService
    {
        public static readonly string[] NombresMeses = new string[12]
        {
            "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
            "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
        };

        public static readonly string[] NombresDias = new string[5]
        {
            "Lunes", "Martes", "Miércoles", "Jueves", "Viernes"
        };

        /// <summary>
        /// INCISO A: Declara e inicializa un arreglo con la información de la tabla del PDF.
        /// Matriz de 12 renglones (meses) x 5 columnas (días).
        /// </summary>
        public static double[,] ObtenerVentasIniciales()
        {
            return new double[12, 5]
            {
                { 5,  16, 10, 12, 24 }, // Enero
                { 40, 55, 10, 11, 18 }, // Febrero
                { 15, 41, 78, 14, 51 }, // Marzo
                { 35, 22, 81, 15, 12 }, // Abril
                { 50, 12, 71, 10, 20 }, // Mayo
                { 70, 40, 60, 28, 22 }, // Junio
                { 50, 50, 50, 36, 25 }, // Julio
                { 40, 70, 40, 11, 20 }, // Agosto
                { 20, 20, 30, 12, 18 }, // Septiembre
                { 10, 40, 32, 13, 16 }, // Octubre
                { 50, 3,  24, 15, 82 }, // Noviembre
                { 40, 46, 15, 46, 22 }  // Diciembre
            };
        }

        /// <summary>
        /// MÉTODO ESPECÍFICO: AnalizarVentas
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO PASO A PASO:
        /// Resuelve los incisos (b), (c), (d) y (e):
        /// 1. Inicializa ventaMenor con matriz[0,0] y ventaMayor con matriz[0,0].
        /// 2. Inicializa ventaTotal = 0 y un arreglo ventasPorDia de tamaño 5 (para las 5 columnas).
        /// 3. Recorre cada fila i (12 meses) y cada columna j (5 días):
        ///    - Suma cada venta al acumulador global ventaTotal (Inciso d).
        ///    - Suma a ventasPorDia[j] la venta actual (Inciso e).
        ///    - Compara si matriz[i,j] < ventaMenor: actualiza valor, mes y día (Inciso b).
        ///    - Compara si matriz[i,j] > ventaMayor: actualiza valor, mes y día (Inciso c).
        /// 4. Retorna el objeto ReporteVentas consolidado.
        /// </summary>
        public static ReporteVentas AnalizarVentas(double[,] matriz)
        {
            if (matriz == null) throw new ArgumentNullException(nameof(matriz));
            int meses = matriz.GetLength(0);
            int dias = matriz.GetLength(1);

            double ventaMenor = matriz[0, 0];
            int mesMenorIdx = 0, diaMenorIdx = 0;

            double ventaMayor = matriz[0, 0];
            int mesMayorIdx = 0, diaMayorIdx = 0;

            double ventaTotal = 0;
            double[] ventasPorDia = new double[dias];
            double[] ventasPorMes = new double[meses];

            for (int i = 0; i < meses; i++)
            {
                double subtotalMes = 0;
                for (int j = 0; j < dias; j++)
                {
                    double valor = matriz[i, j];

                    // Inciso d: Venta total acumulada
                    ventaTotal += valor;
                    subtotalMes += valor;

                    // Inciso e: Venta por día (acumulación por columna)
                    ventasPorDia[j] += valor;

                    // Inciso b: Menor venta
                    if (valor < ventaMenor)
                    {
                        ventaMenor = valor;
                        mesMenorIdx = i;
                        diaMenorIdx = j;
                    }

                    // Inciso c: Mayor venta
                    if (valor > ventaMayor)
                    {
                        ventaMayor = valor;
                        mesMayorIdx = i;
                        diaMayorIdx = j;
                    }
                }
                ventasPorMes[i] = subtotalMes;
            }

            return new ReporteVentas
            {
                VentaMenor = ventaMenor,
                MesMenor = mesMenorIdx < NombresMeses.Length ? NombresMeses[mesMenorIdx] : $"Mes {mesMenorIdx + 1}",
                DiaMenor = diaMenorIdx < NombresDias.Length ? NombresDias[diaMenorIdx] : $"Día {diaMenorIdx + 1}",
                FilaMenor = mesMenorIdx,
                ColumnaMenor = diaMenorIdx,

                VentaMayor = ventaMayor,
                MesMayor = mesMayorIdx < NombresMeses.Length ? NombresMeses[mesMayorIdx] : $"Mes {mesMayorIdx + 1}",
                DiaMayor = diaMayorIdx < NombresDias.Length ? NombresDias[diaMayorIdx] : $"Día {diaMayorIdx + 1}",
                FilaMayor = mesMayorIdx,
                ColumnaMayor = diaMayorIdx,

                VentaTotal = ventaTotal,
                VentasPorDia = ventasPorDia,
                VentasPorMes = ventasPorMes
            };
        }
    }
}
