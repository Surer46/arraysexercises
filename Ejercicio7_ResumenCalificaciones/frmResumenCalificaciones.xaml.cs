using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ejercicio7_ResumenCalificaciones.Servicios;

namespace Ejercicio7_ResumenCalificaciones
{
    public partial class frmResumenCalificaciones : Window
    {
        private const int ALUMNOS = 12;
        private const int PARCIALES = 3;

        private TextBox[,] celdas = new TextBox[ALUMNOS, PARCIALES];
        private TextBlock[] lblPromedios = new TextBlock[ALUMNOS];
        private Border[] bdrEstados = new Border[ALUMNOS];
        private TextBlock[] lblTextosEstados = new TextBlock[ALUMNOS];

        public frmResumenCalificaciones()
        {
            InitializeComponent();
            ConstruirGrid();
            CargarMatriz(ResumenCalificacionesService.ObtenerCalificacionesIniciales());
            EjecutarAnalisis();
        }

        private void ConstruirGrid()
        {
            gridCalificaciones.Children.Clear();
            gridCalificaciones.RowDefinitions.Clear();
            gridCalificaciones.ColumnDefinitions.Clear();

            // 1 header + 12 alumnos
            for (int r = 0; r <= ALUMNOS; r++)
            {
                gridCalificaciones.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Alumno, P1, P2, P3, Promedio, Estado
            gridCalificaciones.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(85) });
            gridCalificaciones.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(58) });
            gridCalificaciones.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(58) });
            gridCalificaciones.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(58) });
            gridCalificaciones.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
            gridCalificaciones.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });

            // Encabezados
            gridCalificaciones.Children.Add(CrearHeader("Alumno", 0, 0, HorizontalAlignment.Left));
            gridCalificaciones.Children.Add(CrearHeader("P1", 0, 1, HorizontalAlignment.Center));
            gridCalificaciones.Children.Add(CrearHeader("P2", 0, 2, HorizontalAlignment.Center));
            gridCalificaciones.Children.Add(CrearHeader("P3", 0, 3, HorizontalAlignment.Center));
            gridCalificaciones.Children.Add(CrearHeader("Promedio", 0, 4, HorizontalAlignment.Center));
            gridCalificaciones.Children.Add(CrearHeader("Estado", 0, 5, HorizontalAlignment.Center));

            for (int r = 0; r < ALUMNOS; r++)
            {
                TextBlock lblAlm = new TextBlock
                {
                    Text = $"Alumno {r + 1}",
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 12,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(4, 0, 4, 0)
                };
                Grid.SetRow(lblAlm, r + 1);
                Grid.SetColumn(lblAlm, 0);
                gridCalificaciones.Children.Add(lblAlm);

                for (int c = 0; c < PARCIALES; c++)
                {
                    TextBox txt = new TextBox
                    {
                        Width = 50,
                        Height = 30,
                        FontSize = 12,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B")),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(2),
                        Text = "0"
                    };
                    Grid.SetRow(txt, r + 1);
                    Grid.SetColumn(txt, c + 1);
                    gridCalificaciones.Children.Add(txt);
                    celdas[r, c] = txt;
                }

                // Promedio
                TextBlock lblP = new TextBlock
                {
                    Text = "0.0",
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(lblP, r + 1);
                Grid.SetColumn(lblP, 4);
                gridCalificaciones.Children.Add(lblP);
                lblPromedios[r] = lblP;

                // Estado (Aprobado / Reprobado)
                Border bdrEst = new Border
                {
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(6, 2, 6, 2),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                TextBlock txtEst = new TextBlock
                {
                    FontSize = 11,
                    FontWeight = FontWeights.Bold
                };
                bdrEst.Child = txtEst;
                Grid.SetRow(bdrEst, r + 1);
                Grid.SetColumn(bdrEst, 5);
                gridCalificaciones.Children.Add(bdrEst);
                bdrEstados[r] = bdrEst;
                lblTextosEstados[r] = txtEst;
            }
        }

        private TextBlock CrearHeader(string texto, int fila, int col, HorizontalAlignment align)
        {
            TextBlock tb = new TextBlock
            {
                Text = texto,
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46")),
                HorizontalAlignment = align,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4, 0, 4, 8)
            };
            Grid.SetRow(tb, fila);
            Grid.SetColumn(tb, col);
            return tb;
        }

        private void CargarMatriz(double[,] datos)
        {
            for (int r = 0; r < ALUMNOS; r++)
            {
                for (int c = 0; c < PARCIALES; c++)
                {
                    celdas[r, c].Text = datos[r, c].ToString("0.#", CultureInfo.InvariantCulture);
                }
            }
        }

        private double[,] ObtenerMatriz()
        {
            double[,] matriz = new double[ALUMNOS, PARCIALES];

            for (int r = 0; r < ALUMNOS; r++)
            {
                for (int c = 0; c < PARCIALES; c++)
                {
                    string texto = celdas[r, c].Text.Trim();
                    if (string.IsNullOrEmpty(texto))
                    {
                        throw new FormatException($"Hay datos faltantes en Alumno {r + 1}, Parcial {c + 1}.");
                    }

                    if (!double.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out double val) &&
                        !double.TryParse(texto, out val))
                    {
                        throw new FormatException($"El valor '{texto}' en Alumno {r + 1}, Parcial {c + 1} no es numérico. Introducir sólo números.");
                    }

                    if (val < 0 || val > 10.0)
                    {
                        throw new FormatException($"La calificación '{val}' en Alumno {r + 1}, Parcial {c + 1} debe estar en el rango de 0 a 10.");
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
                ReporteCalificaciones rep = ResumenCalificacionesService.AnalizarCalificaciones(matriz);

                // Inciso a: Promedios individuales y estado
                for (int r = 0; r < ALUMNOS; r++)
                {
                    double prom = rep.PromediosAlumnos[r];
                    lblPromedios[r].Text = prom.ToString("0.##", CultureInfo.InvariantCulture);

                    if (prom >= 7.0)
                    {
                        bdrEstados[r].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECFDF5"));
                        lblTextosEstados[r].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669"));
                        lblTextosEstados[r].Text = "Aprobado";
                    }
                    else
                    {
                        bdrEstados[r].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF2F2"));
                        lblTextosEstados[r].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                        lblTextosEstados[r].Text = "Reprobado";
                    }
                }

                // Inciso b: Promedio más alto
                lblPromedioAlto.Text = rep.PromedioMasAlto.ToString("0.##", CultureInfo.InvariantCulture);
                lblAlumnoAlto.Text = $"Alumno {rep.AlumnoPromedioAltoIdx + 1}";

                // Inciso c: Promedio más bajo
                lblPromedioBajo.Text = rep.PromedioMasBajo.ToString("0.##", CultureInfo.InvariantCulture);
                lblAlumnoBajo.Text = $"Alumno {rep.AlumnoPromedioBajoIdx + 1}";

                // Inciso d: Parciales reprobados (< 7.0)
                lblTotalReprobados.Text = $"{rep.TotalParcialesReprobados} {(rep.TotalParcialesReprobados == 1 ? "parcial" : "parciales")}";

                // Inciso e: Distribución de frecuencias
                ActualizarDistribucion(rep.DistribucionFrecuencias);

                lblStatus.Text = "Análisis académico completado correctamente.";
            }
            catch (FormatException fEx)
            {
                MessageBox.Show(fEx.Message, "Validación de Calificaciones", MessageBoxButton.OK, MessageBoxImage.Warning);
                lblStatus.Text = "Aviso: " + fEx.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        private void ActualizarDistribucion(int[] frecuencias)
        {
            panelDistribucion.Children.Clear();

            for (int i = 0; i < frecuencias.Length; i++)
            {
                int cant = frecuencias[i];

                Border itemCard = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(cant > 0 ? "#F0FDF4" : "#F8FAFC")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(cant > 0 ? "#BBF7D0" : "#E2E8F0")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(10, 5, 10, 5),
                    Margin = new Thickness(0, 0, 0, 4)
                };

                Grid g = new Grid();
                g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                g.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                TextBlock txtRango = new TextBlock
                {
                    Text = $"{ResumenCalificacionesService.NombresRangos[i]}:",
                    FontSize = 12,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155")),
                    VerticalAlignment = VerticalAlignment.Center
                };

                Border tagBorder = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(cant > 0 ? "#DCFCE7" : "#F1F5F9")),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(8, 2, 8, 2)
                };
                TextBlock tagText = new TextBlock
                {
                    Text = $"{cant} Alumnos",
                    FontSize = 11,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(cant > 0 ? "#15803D" : "#64748B"))
                };
                tagBorder.Child = tagText;

                Grid.SetColumn(txtRango, 0);
                Grid.SetColumn(tagBorder, 1);
                g.Children.Add(txtRango);
                g.Children.Add(tagBorder);

                itemCard.Child = g;
                panelDistribucion.Children.Add(itemCard);
            }
        }

        private void btnRestaurar_Click(object sender, RoutedEventArgs e)
        {
            CargarMatriz(ResumenCalificacionesService.ObtenerCalificacionesIniciales());
            EjecutarAnalisis();
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            EjecutarAnalisis();
        }
    }
}
