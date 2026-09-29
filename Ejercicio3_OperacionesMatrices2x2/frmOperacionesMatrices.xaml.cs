using System;
using System.Globalization;
using System.Windows;
using Ejercicio3_OperacionesMatrices2x2.Servicios;

namespace Ejercicio3_OperacionesMatrices2x2
{
    public partial class frmOperacionesMatrices : Window
    {
        public frmOperacionesMatrices()
        {
            InitializeComponent();
            CargarEjemploPDF();
        }

        private void CargarEjemploPDF()
        {
            var (m1, m2) = Matrices2x2Service.ObtenerEjemploPDF();

            txtA00.Text = m1[0, 0].ToString(CultureInfo.InvariantCulture);
            txtA01.Text = m1[0, 1].ToString(CultureInfo.InvariantCulture);
            txtA10.Text = m1[1, 0].ToString(CultureInfo.InvariantCulture);
            txtA11.Text = m1[1, 1].ToString(CultureInfo.InvariantCulture);

            txtB00.Text = m2[0, 0].ToString(CultureInfo.InvariantCulture);
            txtB01.Text = m2[0, 1].ToString(CultureInfo.InvariantCulture);
            txtB10.Text = m2[1, 0].ToString(CultureInfo.InvariantCulture);
            txtB11.Text = m2[1, 1].ToString(CultureInfo.InvariantCulture);

            CalcularOperaciones();
        }

        private double[,] ObtenerMatrizA()
        {
            return new double[2, 2]
            {
                { ParsearNumero(txtA00.Text, "Matriz 1 [1,1]"), ParsearNumero(txtA01.Text, "Matriz 1 [1,2]") },
                { ParsearNumero(txtA10.Text, "Matriz 1 [2,1]"), ParsearNumero(txtA11.Text, "Matriz 1 [2,2]") }
            };
        }

        private double[,] ObtenerMatrizB()
        {
            return new double[2, 2]
            {
                { ParsearNumero(txtB00.Text, "Matriz 2 [1,1]"), ParsearNumero(txtB01.Text, "Matriz 2 [1,2]") },
                { ParsearNumero(txtB10.Text, "Matriz 2 [2,1]"), ParsearNumero(txtB11.Text, "Matriz 2 [2,2]") }
            };
        }

        private double ParsearNumero(string texto, string nombreCampo)
        {
            string limpio = texto.Trim();
            if (string.IsNullOrEmpty(limpio))
            {
                throw new FormatException($"Hay datos faltantes en el campo '{nombreCampo}'. Completa todos los valores.");
            }

            if (!double.TryParse(limpio, NumberStyles.Any, CultureInfo.InvariantCulture, out double valor) &&
                !double.TryParse(limpio, out valor))
            {
                throw new FormatException($"El valor '{limpio}' en '{nombreCampo}' no es un número válido. Introducir sólo números.");
            }

            return valor;
        }

        private void CalcularOperaciones()
        {
            try
            {
                double[,] matrizA = ObtenerMatrizA();
                double[,] matrizB = ObtenerMatrizB();

                // LLAMADAS A LOS MÉTODOS SEPARADOS DEL SERVICIO
                double[,] suma = Matrices2x2Service.Sumar(matrizA, matrizB);
                double[,] resta = Matrices2x2Service.Restar(matrizA, matrizB);
                double[,] producto = Matrices2x2Service.ProductoSimple(matrizA, matrizB);

                lblSum00.Text = suma[0, 0].ToString("0.##");
                lblSum01.Text = suma[0, 1].ToString("0.##");
                lblSum10.Text = suma[1, 0].ToString("0.##");
                lblSum11.Text = suma[1, 1].ToString("0.##");

                lblRes00.Text = resta[0, 0].ToString("0.##");
                lblRes01.Text = resta[0, 1].ToString("0.##");
                lblRes10.Text = resta[1, 0].ToString("0.##");
                lblRes11.Text = resta[1, 1].ToString("0.##");

                lblProd00.Text = producto[0, 0].ToString("0.##");
                lblProd01.Text = producto[0, 1].ToString("0.##");
                lblProd10.Text = producto[1, 0].ToString("0.##");
                lblProd11.Text = producto[1, 1].ToString("0.##");

                try
                {
                    double[,] division = Matrices2x2Service.DivisionSimple(matrizA, matrizB);
                    lblDiv00.Text = division[0, 0].ToString("0.##");
                    lblDiv01.Text = division[0, 1].ToString("0.##");
                    lblDiv10.Text = division[1, 0].ToString("0.##");
                    lblDiv11.Text = division[1, 1].ToString("0.##");
                }
                catch (DivideByZeroException divEx)
                {
                    lblDiv00.Text = "Div/0";
                    lblDiv01.Text = "Div/0";
                    lblDiv10.Text = "Div/0";
                    lblDiv11.Text = "Div/0";
                    MessageBox.Show(divEx.Message, "Error de División", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                lblStatus.Text = "Operaciones calculadas correctamente con éxito.";
            }
            catch (FormatException fEx)
            {
                MessageBox.Show(fEx.Message, "Validación de Datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                lblStatus.Text = "Aviso: " + fEx.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        private void btnCargarPDF_Click(object sender, RoutedEventArgs e)
        {
            CargarEjemploPDF();
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            CalcularOperaciones();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtA00.Text = "0";
            txtA01.Text = "0";
            txtA10.Text = "0";
            txtA11.Text = "0";

            txtB00.Text = "1";
            txtB01.Text = "1";
            txtB10.Text = "1";
            txtB11.Text = "1";

            CalcularOperaciones();
        }
    }
}
