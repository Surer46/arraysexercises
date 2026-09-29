using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ejercicio5_EstadisticasMatriz5x10.Servicios;

namespace Ejercicio5_EstadisticasMatriz5x10
{
    public partial class frmEstadisticasMatriz : Window
    {
        private const int FILAS = 5;
        private const int COLUMNAS = 10;

        private TextBox[,] celdas = new TextBox[FILAS, COLUMNAS];
        private TextBlock[] lblArregloA = new TextBlock[FILAS];
        private TextBlock[] lblArregloB = new TextBlock[FILAS];
        private TextBlock[] lblArregloC = new TextBlock[COLUMNAS];
        private TextBlock[] lblArregloD = new TextBlock[COLUMNAS];

        public frmEstadisticasMatriz()
        {
            InitializeComponent();
            ConstruirTableroCompleto();
            CargarMatriz(EstadisticasMatrizService.ObtenerMatrizEjemploPDF());
            EjecutarCalculos();
        }

        private void ConstruirTableroCompleto()
        {
            gridTablaCompleta.Children.Clear();
            gridTablaCompleta.RowDefinitions.Clear();
            gridTablaCompleta.ColumnDefinitions.Clear();

            // Total Filas:
            // 0: Encabezado Matriz / A / B
            // 1..5: Filas de la matriz y celdas de A y B
            // 6: Espaciador
            // 7: Fila C (Sumas de columnas)
            // 8: Fila D (Promedios de columnas)
            for (int r = 0; r < 9; r++)
            {
                gridTablaCompleta.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Total Columnas:
            // 0: Etiqueta C / D lateral
            // 1..10: Columnas de la matriz y valores de C y D
            // 11: Espaciador
            // 12: Columna A
            // 13: Columna B
            for (int c = 0; c < 14; c++)
            {
                gridTablaCompleta.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            // Encabezado "Matriz"
            TextBlock lblHeaderMatriz = new TextBlock
            {
                Text = "Matriz (5 x 10)",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155")),
                Margin = new Thickness(6, 0, 0, 6)
            };
            Grid.SetRow(lblHeaderMatriz, 0);
            Grid.SetColumn(lblHeaderMatriz, 1);
            Grid.SetColumnSpan(lblHeaderMatriz, 4);
            gridTablaCompleta.Children.Add(lblHeaderMatriz);

            // Encabezados A y B
            TextBlock lblHeaderA = new TextBlock
            {
                Text = "A (Suma)",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46")),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(4, 0, 4, 6)
            };
            Grid.SetRow(lblHeaderA, 0);
            Grid.SetColumn(lblHeaderA, 12);
            gridTablaCompleta.Children.Add(lblHeaderA);

            TextBlock lblHeaderB = new TextBlock
            {
                Text = "B (Prom)",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46")),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(4, 0, 4, 6)
            };
            Grid.SetRow(lblHeaderB, 0);
            Grid.SetColumn(lblHeaderB, 13);
            gridTablaCompleta.Children.Add(lblHeaderB);

            Style inputStyle = (Style)FindResource("CeldaInputStyle");

            // Matriz 5x10 y Arreglos A y B
            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    TextBox txt = new TextBox
                    {
                        Style = inputStyle,
                        MaxLength = 3,
                        Text = "0"
                    };
                    Grid.SetRow(txt, r + 1);
                    Grid.SetColumn(txt, c + 1);
                    gridTablaCompleta.Children.Add(txt);
                    celdas[r, c] = txt;
                }

                // Celda A (Suma Fila r)
                Border bdrA = CrearBadgeResultado("#ECFDF5", "#A7F3D0", 54);
                TextBlock txtA = new TextBlock
                {
                    Text = "0",
                    FontSize = 13,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                bdrA.Child = txtA;
                Grid.SetRow(bdrA, r + 1);
                Grid.SetColumn(bdrA, 12);
                gridTablaCompleta.Children.Add(bdrA);
                lblArregloA[r] = txtA;

                // Celda B (Promedio Fila r)
                Border bdrB = CrearBadgeResultado("#ECFDF5", "#A7F3D0", 54);
                TextBlock txtB = new TextBlock
                {
                    Text = "0.0",
                    FontSize = 13,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                bdrB.Child = txtB;
                Grid.SetRow(bdrB, r + 1);
                Grid.SetColumn(bdrB, 13);
                gridTablaCompleta.Children.Add(bdrB);
                lblArregloB[r] = txtB;
            }

            // Espaciador vertical
            Border sep = new Border { Height = 14 };
            Grid.SetRow(sep, 6);
            gridTablaCompleta.Children.Add(sep);

            // Etiquetas C y D a la izquierda
            TextBlock lblLetraC = new TextBlock
            {
                Text = "C (Suma):",
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46")),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 8, 0)
            };
            Grid.SetRow(lblLetraC, 7);
            Grid.SetColumn(lblLetraC, 0);
            gridTablaCompleta.Children.Add(lblLetraC);

            TextBlock lblLetraD = new TextBlock
            {
                Text = "D (Prom):",
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46")),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 8, 0)
            };
            Grid.SetRow(lblLetraD, 8);
            Grid.SetColumn(lblLetraD, 0);
            gridTablaCompleta.Children.Add(lblLetraD);

            // Columnas C y D (debajo de cada columna 1..10)
            for (int c = 0; c < COLUMNAS; c++)
            {
                Border bdrC = CrearBadgeResultado("#ECFDF5", "#A7F3D0", 42);
                TextBlock txtC = new TextBlock
                {
                    Text = "0",
                    FontSize = 13,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                bdrC.Child = txtC;
                Grid.SetRow(bdrC, 7);
                Grid.SetColumn(bdrC, c + 1);
                gridTablaCompleta.Children.Add(bdrC);
                lblArregloC[c] = txtC;

                Border bdrD = CrearBadgeResultado("#ECFDF5", "#A7F3D0", 42);
                TextBlock txtD = new TextBlock
                {
                    Text = "0.0",
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                bdrD.Child = txtD;
                Grid.SetRow(bdrD, 8);
                Grid.SetColumn(bdrD, c + 1);
                gridTablaCompleta.Children.Add(bdrD);
                lblArregloD[c] = txtD;
            }
        }

        private Border CrearBadgeResultado(string fondoHex, string bordeHex, double ancho)
        {
            return new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(fondoHex)),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(bordeHex)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Width = ancho,
                Height = 36,
                Margin = new Thickness(2)
            };
        }

        private void CargarMatriz(int[,] datos)
        {
            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    celdas[r, c].Text = datos[r, c].ToString();
                }
            }
        }

        private int[,] ObtenerMatriz()
        {
            int[,] matriz = new int[FILAS, COLUMNAS];

            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    string texto = celdas[r, c].Text.Trim();
                    if (string.IsNullOrEmpty(texto))
                    {
                        throw new FormatException($"Hay datos faltantes en fila {r + 1}, columna {c + 1}. Completa la celda.");
                    }

                    if (!int.TryParse(texto, out int val))
                    {
                        throw new FormatException($"El valor '{texto}' en celda [{r + 1},{c + 1}] es inválido. Introducir sólo números.");
                    }

                    matriz[r, c] = val;
                }
            }

            return matriz;
        }

        private void EjecutarCalculos()
        {
            try
            {
                int[,] matriz = ObtenerMatriz();

                // LLAMADA AL SERVICIO SEPARADO
                ReporteEstadisticasMatriz rep = EstadisticasMatrizService.CalcularEstadisticas(matriz);

                int sumaTotal = 0;

                // Actualizar Arreglos A y B
                for (int r = 0; r < FILAS; r++)
                {
                    lblArregloA[r].Text = rep.ArregloA_SumaFilas[r].ToString();
                    lblArregloB[r].Text = rep.ArregloB_PromedioFilas[r].ToString("0.#", CultureInfo.InvariantCulture);
                    sumaTotal += rep.ArregloA_SumaFilas[r];
                }

                // Actualizar Arreglos C y D
                for (int c = 0; c < COLUMNAS; c++)
                {
                    lblArregloC[c].Text = rep.ArregloC_SumaColumnas[c].ToString();
                    lblArregloD[c].Text = rep.ArregloD_PromedioColumnas[c].ToString("0.#", CultureInfo.InvariantCulture);
                }

                lblSumaTotal.Text = sumaTotal.ToString();
                double promedioGeneral = Math.Round((double)sumaTotal / (FILAS * COLUMNAS), 2);
                lblPromedioGeneral.Text = promedioGeneral.ToString("0.##", CultureInfo.InvariantCulture);

                lblStatus.Text = "Arreglos A, B, C y D calculados y actualizados con éxito.";
            }
            catch (FormatException fEx)
            {
                MessageBox.Show(fEx.Message, "Validación de Datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                lblStatus.Text = "Aviso: " + fEx.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        private void btnCargarPDF_Click(object sender, RoutedEventArgs e)
        {
            CargarMatriz(EstadisticasMatrizService.ObtenerMatrizEjemploPDF());
            EjecutarCalculos();
        }

        private void btnAleatorio_Click(object sender, RoutedEventArgs e)
        {
            CargarMatriz(EstadisticasMatrizService.GenerarMatrizAleatoria());
            EjecutarCalculos();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    celdas[r, c].Text = "0";
                }
            }
            EjecutarCalculos();
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            EjecutarCalculos();
        }
    }
}
