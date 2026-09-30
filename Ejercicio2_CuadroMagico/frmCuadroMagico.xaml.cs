using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ejercicio2_CuadroMagico.Servicios;

namespace Ejercicio2_CuadroMagico
{
    public partial class frmCuadroMagico : Window
    {
        private int dimensionN = 3;
        private TextBox[,] celdas = new TextBox[3, 3];
        private TextBlock[] lblSumasFilas = new TextBlock[3];
        private TextBlock[] lblSumasColumnas = new TextBlock[3];

        public frmCuadroMagico()
        {
            InitializeComponent();
            ReconstruirTablero(3);
            CargarMatriz(CuadroMagicoService.ObtenerEjemploPDF());
            EjecutarAnalisis();
        }

        private void Tamano_Changed(object sender, RoutedEventArgs e)
        {
            if (gridTableroCompleto == null || rb3x3 == null || rb4x4 == null || rb5x5 == null) return;

            if (rb3x3.IsChecked == true) dimensionN = 3;
            else if (rb4x4.IsChecked == true) dimensionN = 4;
            else if (rb5x5.IsChecked == true) dimensionN = 5;

            ReconstruirTablero(dimensionN);
            if (dimensionN % 2 != 0)
            {
                CargarMatriz(CuadroMagicoService.GenerarCuadroMagicoImpar(dimensionN));
            }
            EjecutarAnalisis();
        }

        private void ReconstruirTablero(int n)
        {
            gridTableroCompleto.Children.Clear();
            gridTableroCompleto.RowDefinitions.Clear();
            gridTableroCompleto.ColumnDefinitions.Clear();

            celdas = new TextBox[n, n];
            lblSumasFilas = new TextBlock[n];
            lblSumasColumnas = new TextBlock[n];

            // n filas para celdas + 1 fila para encabezado de columnas + 1 fila para sumas inferiores
            // Total filas = n + 2
            // n columnas para celdas + 1 columna para sumas laterales + 1 columna lateral
            // Total columnas = n + 2

            for (int r = 0; r < n + 2; r++)
            {
                gridTableroCompleto.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            for (int c = 0; c < n + 2; c++)
            {
                gridTableroCompleto.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            // Encabezado lateral: Título SumFils
            Border badgeFils = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F5F9")),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(8, 4, 8, 4),
                Margin = new Thickness(6, 4, 6, 4),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            badgeFils.Child = new TextBlock { Text = "SumFils", FontWeight = FontWeights.Bold, FontSize = 12, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#475569")) };
            Grid.SetRow(badgeFils, 0);
            Grid.SetColumn(badgeFils, n + 1);
            gridTableroCompleto.Children.Add(badgeFils);

            // Celdas de la matriz
            for (int r = 0; r < n; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    TextBox txt = new TextBox
                    {
                        Width = 56,
                        Height = 50,
                        FontSize = 16,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B")),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(4),
                        MaxLength = 4,
                        Text = "0"
                    };

                    Grid.SetRow(txt, r + 1);
                    Grid.SetColumn(txt, c);
                    gridTableroCompleto.Children.Add(txt);
                    celdas[r, c] = txt;
                }

                // Celda de Suma de Fila (SumFils)
                Border bdrFilaSuma = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECFDF5")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A7F3D0")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Width = 56,
                    Height = 50,
                    Margin = new Thickness(6, 4, 6, 4)
                };
                TextBlock lblSumaF = new TextBlock
                {
                    Text = "0",
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                bdrFilaSuma.Child = lblSumaF;
                lblSumasFilas[r] = lblSumaF;

                Grid.SetRow(bdrFilaSuma, r + 1);
                Grid.SetColumn(bdrFilaSuma, n + 1);
                gridTableroCompleto.Children.Add(bdrFilaSuma);
            }

            // Sumas de columnas (SumCols)
            for (int c = 0; c < n; c++)
            {
                Border bdrColSuma = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECFDF5")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A7F3D0")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Width = 56,
                    Height = 44,
                    Margin = new Thickness(4, 6, 4, 6)
                };
                TextBlock lblSumaC = new TextBlock
                {
                    Text = "0",
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                bdrColSuma.Child = lblSumaC;
                lblSumasColumnas[c] = lblSumaC;

                Grid.SetRow(bdrColSuma, n + 1);
                Grid.SetColumn(bdrColSuma, c);
                gridTableroCompleto.Children.Add(bdrColSuma);
            }
        }

        private void CargarMatriz(int[,] datos)
        {
            int n = datos.GetLength(0);
            if (n != dimensionN)
            {
                dimensionN = n;
                if (n == 3) rb3x3.IsChecked = true;
                else if (n == 4) rb4x4.IsChecked = true;
                else if (n == 5) rb5x5.IsChecked = true;
                ReconstruirTablero(n);
            }

            for (int r = 0; r < n; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    celdas[r, c].Text = datos[r, c].ToString();
                }
            }
        }

        private int[,] ObtenerMatriz()
        {
            int[,] matriz = new int[dimensionN, dimensionN];

            for (int r = 0; r < dimensionN; r++)
            {
                for (int c = 0; c < dimensionN; c++)
                {
                    string valor = celdas[r, c].Text.Trim();
                    if (string.IsNullOrEmpty(valor))
                    {
                        throw new FormatException($"Hay datos faltantes en la fila {r + 1}, columna {c + 1}. Completa la celda.");
                    }

                    if (!int.TryParse(valor, out int num))
                    {
                        throw new FormatException($"El valor '{valor}' en casilla [{r + 1},{c + 1}] es inválido. Introducir sólo números.");
                    }

                    matriz[r, c] = num;
                }
            }

            return matriz;
        }

        private void EjecutarAnalisis()
        {
            try
            {
                int[,] matriz = ObtenerMatriz();

                // LLAMADA AL SERVICIO SEPARADO
                ResultadoCuadroMagico res = CuadroMagicoService.AnalizarCuadroMagico(matriz);

                // Actualizar sumas en pantalla
                for (int i = 0; i < dimensionN; i++)
                {
                    lblSumasFilas[i].Text = res.SumasFilas[i].ToString();
                    lblSumasColumnas[i].Text = res.SumasColumnas[i].ToString();
                }

                lblDiagPrincipal.Text = res.SumaDiagonalPrincipal.ToString();
                lblDiagSecundaria.Text = res.SumaDiagonalSecundaria.ToString();
                lblExplicacion.Text = res.ExplicacionDetallada;

                if (res.EsCuadroMagico)
                {
                    cardVeredicto.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECFDF5"));
                    cardVeredicto.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A7F3D0"));
                    lblEstadoBadge.Text = "DIAGNÓSTICO POSITIVO";
                    lblEstadoBadge.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                    lblVeredicto.Text = "¡Es Cuadro Mágico!";
                    lblVeredicto.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#047857"));
                    lblConstanteMagica.Text = res.ConstanteMagica.ToString();
                    lblStatus.Text = $"Excelente. Se verificó con éxito el Cuadrado Mágico con constante {res.ConstanteMagica}.";
                }
                else
                {
                    cardVeredicto.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF2F2"));
                    cardVeredicto.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FECACA"));
                    lblEstadoBadge.Text = "DIAGNÓSTICO NEGATIVO";
                    lblEstadoBadge.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#991B1B"));
                    lblVeredicto.Text = "No es Cuadro Mágico";
                    lblVeredicto.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                    lblConstanteMagica.Text = "N/A";
                    lblStatus.Text = "Las sumas de filas, columnas o diagonales no son coincidentes.";
                }
            }
            catch (FormatException fEx)
            {
                MessageBox.Show(fEx.Message, "Validación de Datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                lblStatus.Text = "Aviso: " + fEx.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        private void btnCargarPDF_Click(object sender, RoutedEventArgs e)
        {
            CargarMatriz(CuadroMagicoService.ObtenerEjemploPDF());
            EjecutarAnalisis();
        }

        private void btnGenerarMagico_Click(object sender, RoutedEventArgs e)
        {
            if (dimensionN % 2 != 0)
            {
                CargarMatriz(CuadroMagicoService.GenerarCuadroMagicoImpar(dimensionN));
                EjecutarAnalisis();
            }
            else
            {
                MessageBox.Show("El generador automático integrado está optimizado para dimensiones impares (3x3, 5x5). Puedes editar el cuadro 4x4 manualmente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            for (int r = 0; r < dimensionN; r++)
            {
                for (int c = 0; c < dimensionN; c++)
                {
                    celdas[r, c].Text = "0";
                }
            }
            EjecutarAnalisis();
        }

        private void btnVerificar_Click(object sender, RoutedEventArgs e)
        {
            EjecutarAnalisis();
        }
    }
}
