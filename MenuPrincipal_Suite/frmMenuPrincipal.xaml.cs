using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace MenuPrincipal_Suite
{
    public partial class frmMenuPrincipal : Window
    {
        public frmMenuPrincipal()
        {
            InitializeComponent();
        }

        private void LanzarEjercicio(string nombreProyecto, string tituloVentana)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                // Buscar en las posibles rutas estándar
                string[] rutasPosibles = new string[]
                {
                    Path.Combine(baseDir, $"{nombreProyecto}.exe"),
                    Path.Combine(baseDir, "..", "..", "..", "..", "Ejecutables_EXE", $"{nombreProyecto}.exe"),
                    Path.Combine(baseDir, "..", "..", "..", "..", nombreProyecto, "bin", "Debug", "net10.0-windows", $"{nombreProyecto}.exe"),
                    Path.Combine(baseDir, "..", nombreProyecto, $"{nombreProyecto}.exe"),
                    Path.Combine(@"C:\Universidad\Cuatrimestre_4\Estructura de datos\EjerciciosArrays\Ejecutables_EXE", $"{nombreProyecto}.exe"),
                    Path.Combine(@"C:\Universidad\Cuatrimestre_4\Estructura de datos\EjerciciosArrays", nombreProyecto, "bin", "Debug", "net10.0-windows", $"{nombreProyecto}.exe")
                };

                string rutaEncontrada = null;
                foreach (string ruta in rutasPosibles)
                {
                    string normalizada = Path.GetFullPath(ruta);
                    if (File.Exists(normalizada))
                    {
                        rutaEncontrada = normalizada;
                        break;
                    }
                }

                if (rutaEncontrada != null)
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = rutaEncontrada,
                        WorkingDirectory = Path.GetDirectoryName(rutaEncontrada),
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                    lblStatus.Text = $"Se ha iniciado {tituloVentana} con éxito.";
                }
                else
                {
                    MessageBox.Show($"No se encontró el ejecutable de {nombreProyecto}. Asegúrate de haber compilado la solución o revisa la carpeta 'Ejecutables_EXE'.",
                                    "Ejecutable No Encontrado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    lblStatus.Text = $"No se encontró {nombreProyecto}.exe";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar {nombreProyecto}: {ex.Message}", "Error al Ejecutar", MessageBoxButton.OK, MessageBoxImage.Error);
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        private void btnEje1_Click(object sender, RoutedEventArgs e)
        {
            LanzarEjercicio("Ejercicio1_ContadorCeros", "Ejercicio 1: Contador de Ceros");
        }

        private void btnEje2_Click(object sender, RoutedEventArgs e)
        {
            LanzarEjercicio("Ejercicio2_CuadroMagico", "Ejercicio 2: Cuadrado Mágico");
        }

        private void btnEje3_Click(object sender, RoutedEventArgs e)
        {
            LanzarEjercicio("Ejercicio3_OperacionesMatrices2x2", "Ejercicio 3: Matrices 2x2");
        }

        private void btnEje4_Click(object sender, RoutedEventArgs e)
        {
            LanzarEjercicio("Ejercicio4_MatrizIdentidad", "Ejercicio 4: Matriz Identidad");
        }

        private void btnEje5_Click(object sender, RoutedEventArgs e)
        {
            LanzarEjercicio("Ejercicio5_EstadisticasMatriz5x10", "Ejercicio 5: Matriz 5x10");
        }

        private void btnEje6_Click(object sender, RoutedEventArgs e)
        {
            LanzarEjercicio("Ejercicio6_ResumenVentas", "Ejercicio 6: Resumen de Ventas");
        }

        private void btnEje7_Click(object sender, RoutedEventArgs e)
        {
            LanzarEjercicio("Ejercicio7_ResumenCalificaciones", "Ejercicio 7: Resumen de Calificaciones");
        }

        private void btnEje8_Click(object sender, RoutedEventArgs e)
        {
            LanzarEjercicio("Ejercicio8_LaberintoPathfinding", "Ejercicio 8: Pathfinding 2D");
        }

        private void btnEje9_Click(object sender, RoutedEventArgs e)
        {
            LanzarEjercicio("Ejercicio9_MultiplicacionAvanzada", "Ejercicio 9: Multiplicación Avanzada");
        }

        private void btnAbrirCarpeta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string carpetaExes = @"C:\Universidad\Cuatrimestre_4\Estructura de datos\EjerciciosArrays\Ejecutables_EXE";
                if (!Directory.Exists(carpetaExes))
                {
                    carpetaExes = @"C:\Universidad\Cuatrimestre_4\Estructura de datos\EjerciciosArrays";
                }
                Process.Start("explorer.exe", carpetaExes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir explorador: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("¿Deseas cerrar el Menú Principal?", "Confirmar Salida", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
