using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ejercicio6_ResumenVentas.Servicios;

namespace Ejercicio6_ResumenVentas
{
    public partial class frmResumenVentas : Window
    {
        private const int MESES = 12;
        private const int DIAS = 5;

        private TextBox[,] celdas = new TextBox[MESES, DIAS];
        private TextBlock[] lblSubtotalMes = new TextBlock[MESES];

        public frmResumenVentas()
        {
            InitializeComponent();
            ConstruirGridVentas();
            CargarMatriz(ResumenVentasService.ObtenerVentasIniciales());
            EjecutarAnalisis();
        }

        private void ConstruirGridVentas()
        {
            gridVentas.Children.Clear();
            gridVentas.RowDefinitions.Clear();
            gridVentas.ColumnDefinitions.Clear();

            // 1 fila de encabezado + 12 filas de meses
            for (int r = 0; r <= MESES; r++)
            {
                gridVentas.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // 1 col de mes + 5 cols de días + 1 col de total mes
            gridVentas.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            for (int c = 0; c < DIAS; c++)
            {
                gridVentas.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(72) });
            }
            gridVentas.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(85) });

            // Encabezados
            TextBlock thMes = CrearHeader("Mes", HorizontalAlignment.Left);
            Grid.SetRow(thMes, 0);
            Grid.SetColumn(thMes, 0);
            gridVentas.Children.Add(thMes);

            for (int c = 0; c < DIAS; c++)
            {
                TextBlock thDia = CrearHeader(ResumenVentasService.NombresDias[c], HorizontalAlignment.Center);
                Grid.SetRow(thDia, 0);
                Grid.SetColumn(thDia, c + 1);
                gridVentas.Children.Add(thDia);
            }

            TextBlock thTotal = CrearHeader("Subtotal", HorizontalAlignment.Center);
            Grid.SetRow(thTotal, 0);
            Grid.SetColumn(thTotal, DIAS + 1);
            gridVentas.Children.Add(thTotal);

            // Filas de Meses
            for (int r = 0; r < MESES; r++)
            {
                TextBlock lblMes = new TextBlock
                {
                    Text = ResumenVentasService.NombresMeses[r],
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 12,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(4, 0, 4, 0)
                };
                Grid.SetRow(lblMes, r + 1);
                Grid.SetColumn(lblMes, 0);
                gridVentas.Children.Add(lblMes);

                for (int c = 0; c < DIAS; c++)
                {
                    TextBox txt = new TextBox
                    {
                        Width = 62,
                        Height = 32,
                        FontSize = 13,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B")),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(2),
                        Text = "0"
                    };
                    Grid.SetRow(txt, r + 1);
                    Grid.SetColumn(txt, c + 1);
                    gridVentas.Children.Add(txt);
                    celdas[r, c] = txt;
                }

                TextBlock lblSub = new TextBlock
                {
                    Text = "$ 0.00",
                    FontWeight = FontWeights.Bold,
                    FontSize = 12,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(lblSub, r + 1);
                Grid.SetColumn(lblSub, DIAS + 1);
                gridVentas.Children.Add(lblSub);
                lblSubtotalMes[r] = lblSub;
            }
        }

        private TextBlock CrearHeader(string texto, HorizontalAlignment align)
        {
            return new TextBlock
            {
                Text = texto,
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46")),
                HorizontalAlignment = align,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4, 0, 4, 8)
            };
        }

        private void CargarMatriz(double[,] datos)
        {
            for (int r = 0; r < MESES; r++)
            {
                for (int c = 0; c < DIAS; c++)
                {
                    celdas[r, c].Text = datos[r, c].ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private double[,] ObtenerMatriz()
        {
            double[,] matriz = new double[MESES, DIAS];

            for (int r = 0; r < MESES; r++)
            {
                for (int c = 0; c < DIAS; c++)
                {
                    string texto = celdas[r, c].Text.Trim();
                    if (string.IsNullOrEmpty(texto))
                    {
                        throw new FormatException($"Hay datos faltantes en {ResumenVentasService.NombresMeses[r]}, día {ResumenVentasService.NombresDias[c]}.");
                    }

                    if (!double.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out double val) &&
                        !double.TryParse(texto, out val))
                    {
                        throw new FormatException($"El valor '{texto}' en {ResumenVentasService.NombresMeses[r]}, {ResumenVentasService.NombresDias[c]} es inválido. Introducir sólo números.");
                    }

                    matriz[r, c] = val;
                }
            }

            return matriz;
        }

        private void EjecutarAnalisis()
        {
            try
            {
                double[,] matriz = ObtenerMatriz();

                // LLAMADA AL SERVICIO SEPARADO
                ReporteVentas rep = ResumenVentasService.AnalizarVentas(matriz);

                // Inciso d: Venta total
                lblVentaTotal.Text = $"$ {rep.VentaTotal:N2}";

                // Inciso b: Menor venta y fecha
                lblMenorMonto.Text = $"$ {rep.VentaMenor:N2}";
                lblMenorFecha.Text = $"{rep.MesMenor}, {rep.DiaMenor}";

                // Inciso c: Mayor venta y fecha
                lblMayorMonto.Text = $"$ {rep.VentaMayor:N2}";
                lblMayorFecha.Text = $"{rep.MesMayor}, {rep.DiaMayor}";

                // Subtotales por mes
                for (int r = 0; r < MESES; r++)
                {
                    lblSubtotalMes[r].Text = $"$ {rep.VentasPorMes[r]:N2}";
                }

                // Inciso e: Ventas por día de la semana
                ActualizarVentasPorDia(rep.VentasPorDia);

                // Resaltar en la tabla la celda menor y mayor
                ResaltarMinMax(rep.FilaMenor, rep.ColumnaMenor, rep.FilaMayor, rep.ColumnaMayor);

                lblStatus.Text = "Análisis comercial generado exitosamente. Celdas clave resaltadas.";
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

        private void ActualizarVentasPorDia(double[] ventasPorDia)
        {
            panelVentasPorDia.Children.Clear();

            for (int c = 0; c < DIAS; c++)
            {
                Border cardDia = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8FAFC")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(12, 6, 12, 6),
                    Margin = new Thickness(0, 0, 0, 6)
                };

                Grid g = new Grid();
                g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                g.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                TextBlock txtDia = new TextBlock
                {
                    Text = ResumenVentasService.NombresDias[c],
                    FontSize = 13,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155")),
                    VerticalAlignment = VerticalAlignment.Center
                };

                TextBlock txtMonto = new TextBlock
                {
                    Text = $"$ {ventasPorDia[c]:N2}",
                    FontSize = 13,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669")),
                    VerticalAlignment = VerticalAlignment.Center
                };

                Grid.SetColumn(txtDia, 0);
                Grid.SetColumn(txtMonto, 1);
                g.Children.Add(txtDia);
                g.Children.Add(txtMonto);

                cardDia.Child = g;
                panelVentasPorDia.Children.Add(cardDia);
            }
        }

        private void ResaltarMinMax(int rMin, int cMin, int rMax, int cMax)
        {
            for (int r = 0; r < MESES; r++)
            {
                for (int c = 0; c < DIAS; c++)
                {
                    if (r == rMin && c == cMin)
                    {
                        celdas[r, c].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF3C7")); // Amarillo/dorado para min
                        celdas[r, c].BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B"));
                    }
                    else if (r == rMax && c == cMax)
                    {
                        celdas[r, c].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D1FAE5")); // Verde destacado para max
                        celdas[r, c].BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981"));
                    }
                    else
                    {
                        celdas[r, c].Background = Brushes.White;
                        celdas[r, c].BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBD5E1"));
                    }
                }
            }
        }

        private void btnRestaurar_Click(object sender, RoutedEventArgs e)
        {
            CargarMatriz(ResumenVentasService.ObtenerVentasIniciales());
            EjecutarAnalisis();
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            EjecutarAnalisis();
        }
    }
}
