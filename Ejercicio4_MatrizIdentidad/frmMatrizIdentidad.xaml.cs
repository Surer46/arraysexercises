using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ejercicio4_MatrizIdentidad.Servicios;

namespace Ejercicio4_MatrizIdentidad
{
    public partial class frmMatrizIdentidad : Window
    {
        private int dimensionN = 5;

        public frmMatrizIdentidad()
        {
            InitializeComponent();
            ActualizarMatriz();
        }

        private void ActualizarMatriz()
        {
            lblDimensionN.Text = dimensionN.ToString();
            lblMatrizInfo.Text = $"Matriz Identidad I_{dimensionN} ({dimensionN} x {dimensionN})";

            // LLAMADA AL MÉTODO SEPARADO
            int[,] matriz = MatrizIdentidadService.GenerarMatrizIdentidad(dimensionN);

            ConstruirGrid(matriz);

            int unos = dimensionN;
            int ceros = (dimensionN * dimensionN) - dimensionN;
            lblCantidadUnos.Text = unos.ToString();
            lblCantidadCeros.Text = ceros.ToString();
            lblTotalElementos.Text = (dimensionN * dimensionN).ToString();

            lblStatus.Text = $"Matriz identidad de {dimensionN}x{dimensionN} generada correctamente.";
        }

        private void ConstruirGrid(int[,] matriz)
        {
            gridMatrizIdentidad.Children.Clear();
            gridMatrizIdentidad.RowDefinitions.Clear();
            gridMatrizIdentidad.ColumnDefinitions.Clear();

            int n = matriz.GetLength(0);
            double celdaSize = n <= 6 ? 50 : (n <= 8 ? 42 : 36);

            for (int r = 0; r < n; r++)
            {
                gridMatrizIdentidad.RowDefinitions.Add(new RowDefinition { Height = new GridLength(celdaSize) });
            }
            for (int c = 0; c < n; c++)
            {
                gridMatrizIdentidad.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(celdaSize) });
            }

            for (int r = 0; r < n; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    bool esDiagonal = (r == c);

                    Border celdaBorder = new Border
                    {
                        CornerRadius = new CornerRadius(10),
                        Margin = new Thickness(3),
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(esDiagonal ? "#10B981" : "#FFFFFF")),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(esDiagonal ? "#059669" : "#E2E8F0")),
                        BorderThickness = new Thickness(esDiagonal ? 1.5 : 1)
                    };

                    TextBlock celdaTexto = new TextBlock
                    {
                        Text = matriz[r, c].ToString(),
                        FontSize = n <= 6 ? 16 : 14,
                        FontWeight = esDiagonal ? FontWeights.Bold : FontWeights.Normal,
                        Foreground = esDiagonal ? Brushes.White : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8")),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    celdaBorder.Child = celdaTexto;
                    Grid.SetRow(celdaBorder, r);
                    Grid.SetColumn(celdaBorder, c);
                    gridMatrizIdentidad.Children.Add(celdaBorder);
                }
            }
        }

        private void btnMenos_Click(object sender, RoutedEventArgs e)
        {
            if (dimensionN > 2)
            {
                dimensionN--;
                ActualizarMatriz();
            }
        }

        private void btnMas_Click(object sender, RoutedEventArgs e)
        {
            if (dimensionN < 10)
            {
                dimensionN++;
                ActualizarMatriz();
            }
        }
    }
}
