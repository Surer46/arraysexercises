using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ejercicio9_MultiplicacionAvanzada.Servicios;

namespace Ejercicio9_MultiplicacionAvanzada
{
    public partial class frmAlgebraMatricial : Window
    {
        private int dimN = 2; // Filas A
        private int dimM = 2; // Cols A / Filas B
        private int dimP = 2; // Cols B

        private TextBox[,] celdasA;
        private TextBox[,] celdasB;
        private TextBlock[,] celdasC;

        public frmAlgebraMatricial()
        {
            InitializeComponent();
            ConfigurarDimensiones(2, 2, 2);
            CargarValoresPorDefecto();
            EjecutarMultiplicacion();
        }

        private void ConfigurarDimensiones(int n, int m, int p)
        {
            dimN = n;
            dimM = m;
            dimP = p;

            lblTituloA.Text = $"Matriz A ({dimN} × {dimM})";
            lblTituloB.Text = $"Matriz B ({dimM} × {dimP})";
            lblTituloC.Text = $"Producto C ({dimN} × {dimP})";

            ConstruirGridEntrada(gridMatrizA, dimN, dimM, out celdasA);
            ConstruirGridEntrada(gridMatrizB, dimM, dimP, out celdasB);
            ConstruirGridSalida(gridMatrizC, dimN, dimP, out celdasC);
        }

        private void ConstruirGridEntrada(Grid grid, int filas, int cols, out TextBox[,] matrizControles)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            matrizControles = new TextBox[filas, cols];

            for (int r = 0; r < filas; r++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(46) });
            }
            for (int c = 0; c < cols; c++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(46) });
            }

            for (int r = 0; r < filas; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    TextBox txt = new TextBox
                    {
                        Width = 40,
                        Height = 38,
                        FontSize = 14,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B")),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(2),
                        Text = "1"
                    };

                    Grid.SetRow(txt, r);
                    Grid.SetColumn(txt, c);
                    grid.Children.Add(txt);
                    matrizControles[r, c] = txt;
                }
            }
        }

        private void ConstruirGridSalida(Grid grid, int filas, int cols, out TextBlock[,] matrizControles)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            matrizControles = new TextBlock[filas, cols];

            for (int r = 0; r < filas; r++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(46) });
            }
            for (int c = 0; c < cols; c++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(56) });
            }

            for (int r = 0; r < filas; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Border bdr = new Border
                    {
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECFDF5")),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A7F3D0")),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(8),
                        Width = 50,
                        Height = 38,
                        Margin = new Thickness(2)
                    };

                    TextBlock tb = new TextBlock
                    {
                        Text = "0",
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669")),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    bdr.Child = tb;
                    Grid.SetRow(bdr, r);
                    Grid.SetColumn(bdr, c);
                    grid.Children.Add(bdr);
                    matrizControles[r, c] = tb;
                }
            }
        }

        private void CargarValoresPorDefecto()
        {
            if (dimN == 2 && dimM == 2 && dimP == 2)
            {
                celdasA[0, 0].Text = "3"; celdasA[0, 1].Text = "2";
                celdasA[1, 0].Text = "1"; celdasA[1, 1].Text = "4";

                celdasB[0, 0].Text = "5"; celdasB[0, 1].Text = "1";
                celdasB[1, 0].Text = "2"; celdasB[1, 1].Text = "3";
            }
        }

        private double[,] ObtenerValores(TextBox[,] celdas, string nombreMatriz)
        {
            int f = celdas.GetLength(0);
            int c = celdas.GetLength(1);
            double[,] mat = new double[f, c];

            for (int r = 0; r < f; r++)
            {
                for (int col = 0; col < c; col++)
                {
                    string txt = celdas[r, col].Text.Trim();
                    if (string.IsNullOrEmpty(txt))
                    {
                        throw new FormatException($"Hay datos faltantes en {nombreMatriz} [{r + 1},{col + 1}].");
                    }

                    if (!double.TryParse(txt, NumberStyles.Any, CultureInfo.InvariantCulture, out double val) &&
                        !double.TryParse(txt, out val))
                    {
                        throw new FormatException($"El valor '{txt}' en {nombreMatriz} [{r + 1},{col + 1}] es inválido. Introducir sólo números.");
                    }

                    mat[r, col] = val;
                }
            }

            return mat;
        }

        private void EjecutarMultiplicacion()
        {
            try
            {
                double[,] matrizA = ObtenerValores(celdasA, "Matriz A");
                double[,] matrizB = ObtenerValores(celdasB, "Matriz B");

                // LLAMADA AL SERVICIO SEPARADO
                double[,] matrizC = AlgebraMatricialService.MultiplicarMatrices(matrizA, matrizB, out List<string> pasos);

                // Actualizar matriz C
                for (int r = 0; r < dimN; r++)
                {
                    for (int c = 0; c < dimP; c++)
                    {
                        celdasC[r, c].Text = matrizC[r, c].ToString("0.##", CultureInfo.InvariantCulture);
                    }
                }

                // Actualizar desglose paso a paso
                panelDesglose.Children.Clear();
                foreach (var paso in pasos)
                {
                    TextBlock tbPaso = new TextBlock
                    {
                        Text = paso,
                        FontSize = 11,
                        FontFamily = new FontFamily("Consolas"),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155")),
                        Margin = new Thickness(0, 0, 0, 4)
                    };
                    panelDesglose.Children.Add(tbPaso);
                }

                // Determinante de A
                if (dimN == dimM)
                {
                    double det = AlgebraMatricialService.CalcularDeterminante(matrizA);
                    lblDetA.Text = det.ToString("0.##", CultureInfo.InvariantCulture);
                }
                else
                {
                    lblDetA.Text = "N/A (No cuadrada)";
                }

                // Transpuesta de A
                double[,] transA = AlgebraMatricialService.Transponer(matrizA);
                MostrarTranspuesta(transA);

                lblStatus.Text = "Multiplicación de matrices O(N·M·P) y análisis álgebraico realizados correctamente.";
            }
            catch (FormatException fEx)
            {
                MessageBox.Show(fEx.Message, "Validación de Matrices", MessageBoxButton.OK, MessageBoxImage.Warning);
                lblStatus.Text = "Aviso: " + fEx.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        private void MostrarTranspuesta(double[,] trans)
        {
            gridTranspuestaA.Children.Clear();
            gridTranspuestaA.RowDefinitions.Clear();
            gridTranspuestaA.ColumnDefinitions.Clear();

            int f = trans.GetLength(0);
            int c = trans.GetLength(1);

            for (int r = 0; r < f; r++)
            {
                gridTranspuestaA.RowDefinitions.Add(new RowDefinition { Height = new GridLength(32) });
            }
            for (int col = 0; col < c; col++)
            {
                gridTranspuestaA.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(42) });
            }

            for (int r = 0; r < f; r++)
            {
                for (int col = 0; col < c; col++)
                {
                    Border bdr = new Border
                    {
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F5F9")),
                        CornerRadius = new CornerRadius(6),
                        Margin = new Thickness(2)
                    };
                    TextBlock tb = new TextBlock
                    {
                        Text = trans[r, col].ToString("0.##", CultureInfo.InvariantCulture),
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155")),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    bdr.Child = tb;
                    Grid.SetRow(bdr, r);
                    Grid.SetColumn(bdr, col);
                    gridTranspuestaA.Children.Add(bdr);
                }
            }
        }

        private void btnPreset2x2_Click(object sender, RoutedEventArgs e)
        {
            ConfigurarDimensiones(2, 2, 2);
            CargarValoresPorDefecto();
            EjecutarMultiplicacion();
        }

        private void btnPreset2x3_Click(object sender, RoutedEventArgs e)
        {
            ConfigurarDimensiones(2, 3, 2);
            EjecutarMultiplicacion();
        }

        private void btnPreset3x3_Click(object sender, RoutedEventArgs e)
        {
            ConfigurarDimensiones(3, 3, 3);
            EjecutarMultiplicacion();
        }

        private void btnAleatorio_Click(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            for (int r = 0; r < dimN; r++)
                for (int c = 0; c < dimM; c++)
                    celdasA[r, c].Text = rng.Next(1, 9).ToString();

            for (int r = 0; r < dimM; r++)
                for (int c = 0; c < dimP; c++)
                    celdasB[r, c].Text = rng.Next(1, 9).ToString();

            EjecutarMultiplicacion();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            for (int r = 0; r < dimN; r++)
                for (int c = 0; c < dimM; c++)
                    celdasA[r, c].Text = "0";

            for (int r = 0; r < dimM; r++)
                for (int c = 0; c < dimP; c++)
                    celdasB[r, c].Text = "0";

            EjecutarMultiplicacion();
        }

        private void btnMultiplicar_Click(object sender, RoutedEventArgs e)
        {
            EjecutarMultiplicacion();
        }
    }
}
