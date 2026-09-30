using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CapturadorEvidencias
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new Application();
            app.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            string outputDir = @"C:\Universidad\Cuatrimestre_4\Estructura de datos\EjerciciosArrays\Evidencias_Capturas";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            Console.WriteLine("Iniciando captura automatizada de ventanas...");

            CapturarVentana(() => new MenuPrincipal_Suite.frmMenuPrincipal(), Path.Combine(outputDir, "captura_menu_principal.png"), 1180, 800);
            CapturarVentana(() => new Ejercicio1_ContadorCeros.frmContadorCeros(), Path.Combine(outputDir, "captura_ejercicio1.png"), 1020, 720);
            CapturarVentana(() => new Ejercicio2_CuadroMagico.frmCuadroMagico(), Path.Combine(outputDir, "captura_ejercicio2.png"), 1060, 760);
            CapturarVentana(() => new Ejercicio3_OperacionesMatrices2x2.frmOperacionesMatrices(), Path.Combine(outputDir, "captura_ejercicio3.png"), 1080, 780);
            CapturarVentana(() => new Ejercicio4_MatrizIdentidad.frmMatrizIdentidad(), Path.Combine(outputDir, "captura_ejercicio4.png"), 1040, 740);
            CapturarVentana(() => new Ejercicio5_EstadisticasMatriz5x10.frmEstadisticasMatriz(), Path.Combine(outputDir, "captura_ejercicio5.png"), 1160, 800);
            CapturarVentana(() => new Ejercicio6_ResumenVentas.frmResumenVentas(), Path.Combine(outputDir, "captura_ejercicio6.png"), 1160, 820);
            CapturarVentana(() => new Ejercicio7_ResumenCalificaciones.frmResumenCalificaciones(), Path.Combine(outputDir, "captura_ejercicio7.png"), 1180, 820);
            CapturarVentana(() => new Ejercicio8_LaberintoPathfinding.frmLaberinto(), Path.Combine(outputDir, "captura_ejercicio8.png"), 1160, 820);
            CapturarVentana(() => new Ejercicio9_MultiplicacionAvanzada.frmAlgebraMatricial(), Path.Combine(outputDir, "captura_ejercicio9.png"), 1200, 840);

            Console.WriteLine("Todas las capturas se generaron exitosamente.");
            app.Shutdown();
        }

        private static void CapturarVentana(Func<Window> factory, string outputPath, int ancho, int alto)
        {
            Window win = null;
            try
            {
                Console.WriteLine($"Renderizando {Path.GetFileName(outputPath)}...");
                win = factory();
                win.Width = ancho;
                win.Height = alto;
                win.WindowStartupLocation = WindowStartupLocation.Manual;
                win.Left = 0;
                win.Top = 0;
                win.Show();

                // Permitir que el Dispatcher procese el renderizado
                DispatcherFrame frame = new DispatcherFrame();
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new DispatcherOperationCallback(f =>
                {
                    ((DispatcherFrame)f).Continue = false;
                    return null;
                }), frame);
                Dispatcher.PushFrame(frame);

                win.UpdateLayout();
                Thread.Sleep(300);

                int w = (int)Math.Max(win.ActualWidth, ancho);
                int h = (int)Math.Max(win.ActualHeight, alto);

                RenderTargetBitmap rtb = new RenderTargetBitmap(w, h, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(win);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));

                using (FileStream fs = File.Create(outputPath))
                {
                    encoder.Save(fs);
                }

                win.Hide();
                win.Close();
                Console.WriteLine($" -> Guardado con éxito: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error capturando {outputPath}: {ex.Message}");
                if (win != null)
                {
                    try { win.Close(); } catch { }
                }
            }
        }
    }
}
