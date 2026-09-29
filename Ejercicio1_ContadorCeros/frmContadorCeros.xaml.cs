using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ejercicio1_ContadorCeros.Servicios;

namespace Ejercicio1_ContadorCeros
{
    /// <summary>
    /// Formulario gráfico para el Ejercicio 1: Contador de Ceros por Renglón.
    /// Implementa diseño Cupertino/Material con paleta blanco/verde esmeralda.
    /// </summary>
    public partial class frmContadorCeros : Window
    {
        private const int FILAS = 5;
        private const int COLUMNAS = 5;
        private TextBox[,] celdas = new TextBox[FILAS, COLUMNAS];

        public frmContadorCeros()
        {
            InitializeComponent();
            ConstruirCuadriculaMatriz();
            CargarMatriz(MatrizContadorCerosService.ObtenerMatrizOriginalDelProblema());
            EjecutarCalculo();
        }

        /// <summary>
        /// Construye dinámicamente la cuadrícula de 5x5 controles TextBox estilizados.
        /// </summary>
        private void ConstruirCuadriculaMatriz()
        {
            gridMatriz.Children.Clear();
            gridMatriz.RowDefinitions.Clear();
            gridMatriz.ColumnDefinitions.Clear();

            for (int r = 0; r < FILAS; r++)
            {
                gridMatriz.RowDefinitions.Add(new RowDefinition { Height = new GridLength(56) });
            }
            for (int c = 0; c < COLUMNAS; c++)
            {
                gridMatriz.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(56) });
            }

            Style tbStyle = (Style)FindResource("MatrizTextBoxStyle");

            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    TextBox txt = new TextBox
                    {
                        Name = $"txtCelda_{r}_{c}",
                        Style = tbStyle,
                        Margin = new Thickness(4),
                        MaxLength = 3,
                        Text = "0"
                    };

                    txt.TextChanged += (s, e) =>
                    {
                        ResaltarCeros(txt);
                    };

                    Grid.SetRow(txt, r);
                    Grid.SetColumn(txt, c);
                    gridMatriz.Children.Add(txt);
                    celdas[r, c] = txt;
                }
            }
        }

        private void ResaltarCeros(TextBox txt)
        {
            if (txt.Text.Trim() == "0")
            {
                txt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECFDF5"));
                txt.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981"));
            }
            else
            {
                txt.Background = Brushes.White;
                txt.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBD5E1"));
            }
        }

        private void CargarMatriz(int[,] datos)
        {
            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    celdas[r, c].Text = datos[r, c].ToString();
                    ResaltarCeros(celdas[r, c]);
                }
            }
        }

        private int[,] ObtenerMatrizDesdeUI()
        {
            int[,] matriz = new int[FILAS, COLUMNAS];

            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    string valor = celdas[r, c].Text.Trim();
                    if (string.IsNullOrEmpty(valor))
                    {
                        throw new FormatException($"Hay datos faltantes en la fila {r + 1}, columna {c + 1}. Por favor completa todos los campos.");
                    }

                    if (!int.TryParse(valor, out int numero))
                    {
                        throw new FormatException($"El valor '{valor}' en el renglón {r + 1}, columna {c + 1} no es válido. Introducir sólo números enteros.");
                    }

                    matriz[r, c] = numero;
                }
            }

            return matriz;
        }

        private void EjecutarCalculo()
        {
            try
            {
                int[,] matriz = ObtenerMatrizDesdeUI();

                // LLAMADA AL MÉTODO SEPARADO
                int[] cerosPorRenglon = MatrizContadorCerosService.ContarCerosPorRenglon(matriz);
                int totalCeros = MatrizContadorCerosService.ContarTotalCeros(cerosPorRenglon);

                lblTotalCeros.Text = $"{totalCeros} {(totalCeros == 1 ? "cero" : "ceros")}";
                ActualizarPanelResultados(cerosPorRenglon);

                lblStatus.Text = "Cálculo realizado con éxito. Los ceros en la matriz han sido resaltados.";
            }
            catch (FormatException fEx)
            {
                MessageBox.Show(fEx.Message, "Validación de Entrada", MessageBoxButton.OK, MessageBoxImage.Warning);
                lblStatus.Text = "Error de validación: " + fEx.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        private void ActualizarPanelResultados(int[] cerosPorRenglon)
        {
            panelRenglones.Children.Clear();

            for (int i = 0; i < cerosPorRenglon.Length; i++)
            {
                int cantidad = cerosPorRenglon[i];

                Border itemCard = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(cantidad > 0 ? "#F0FDF4" : "#F8FAFC")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(cantidad > 0 ? "#BBF7D0" : "#E2E8F0")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(12, 10, 12, 10),
                    Margin = new Thickness(0, 0, 0, 8)
                };

                Grid rowGrid = new Grid();
                rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                StackPanel leftStack = new StackPanel { Orientation = Orientation.Horizontal };
                Border numBadge = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981")),
                    CornerRadius = new CornerRadius(10),
                    Width = 24,
                    Height = 24,
                    Margin = new Thickness(0, 0, 8, 0)
                };
                TextBlock numTxt = new TextBlock
                {
                    Text = (i + 1).ToString(),
                    Foreground = Brushes.White,
                    FontSize = 11,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                numBadge.Child = numTxt;

                TextBlock lblRenglon = new TextBlock
                {
                    Text = $"Renglón {i + 1}",
                    FontSize = 13,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B")),
                    VerticalAlignment = VerticalAlignment.Center
                };

                leftStack.Children.Add(numBadge);
                leftStack.Children.Add(lblRenglon);

                Border tagBorder = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(cantidad > 0 ? "#DCFCE7" : "#F1F5F9")),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(10, 3, 10, 3)
                };
                TextBlock tagText = new TextBlock
                {
                    Text = $"{cantidad} {(cantidad == 1 ? "cero" : "ceros")}",
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(cantidad > 0 ? "#15803D" : "#64748B"))
                };
                tagBorder.Child = tagText;

                Grid.SetColumn(leftStack, 0);
                Grid.SetColumn(tagBorder, 1);
                rowGrid.Children.Add(leftStack);
                rowGrid.Children.Add(tagBorder);

                itemCard.Child = rowGrid;
                panelRenglones.Children.Add(itemCard);
            }
        }

        private void btnCargarDefecto_Click(object sender, RoutedEventArgs e)
        {
            CargarMatriz(MatrizContadorCerosService.ObtenerMatrizOriginalDelProblema());
            EjecutarCalculo();
        }

        private void btnAleatorio_Click(object sender, RoutedEventArgs e)
        {
            CargarMatriz(MatrizContadorCerosService.GenerarMatrizAleatoria());
            EjecutarCalculo();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    celdas[r, c].Text = "0";
                    ResaltarCeros(celdas[r, c]);
                }
            }
            EjecutarCalculo();
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            EjecutarCalculo();
        }
    }
}
