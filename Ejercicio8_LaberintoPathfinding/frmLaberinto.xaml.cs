using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ejercicio8_LaberintoPathfinding.Servicios;

namespace Ejercicio8_LaberintoPathfinding
{
    public partial class frmLaberinto : Window
    {
        private const int FILAS = 8;
        private const int COLUMNAS = 8;

        private int[,] laberinto = new int[FILAS, COLUMNAS];
        private Button[,] botones = new Button[FILAS, COLUMNAS];

        private readonly (int r, int c) inicio = (0, 0);
        private readonly (int r, int c) destino = (FILAS - 1, COLUMNAS - 1);

        public frmLaberinto()
        {
            InitializeComponent();
            ConstruirTablero();
            GenerarLaberinto();
            ResolverRuta();
        }

        private void ConstruirTablero()
        {
            gridLaberinto.Children.Clear();
            gridLaberinto.RowDefinitions.Clear();
            gridLaberinto.ColumnDefinitions.Clear();

            for (int r = 0; r < FILAS; r++)
            {
                gridLaberinto.RowDefinitions.Add(new RowDefinition { Height = new GridLength(46) });
            }
            for (int c = 0; c < COLUMNAS; c++)
            {
                gridLaberinto.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(46) });
            }

            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    int fila = r;
                    int col = c;

                    Button btn = new Button
                    {
                        Margin = new Thickness(2),
                        BorderThickness = new Thickness(1),
                        Cursor = System.Windows.Input.Cursors.Hand
                    };

                    btn.Click += (s, e) =>
                    {
                        AlternarCelda(fila, col);
                    };

                    Grid.SetRow(btn, r);
                    Grid.SetColumn(btn, c);
                    gridLaberinto.Children.Add(btn);
                    botones[r, c] = btn;
                }
            }
        }

        private void AlternarCelda(int r, int c)
        {
            // No permitir bloquear inicio ni destino
            if ((r == inicio.r && c == inicio.c) || (r == destino.r && c == destino.c))
            {
                return;
            }

            laberinto[r, c] = (laberinto[r, c] == LaberintoService.ESTADO_MURO) ? LaberintoService.ESTADO_LIBRE : LaberintoService.ESTADO_MURO;
            ResolverRuta();
        }

        private void GenerarLaberinto()
        {
            laberinto = LaberintoService.GenerarLaberintoAleatorio(FILAS, COLUMNAS, 0.28);
        }

        private void ResolverRuta()
        {
            // LLAMADA AL SERVICIO SEPARADO
            ResultadoRuta res = LaberintoService.EncontrarRutaOptima(laberinto, inicio, destino);

            HashSet<(int, int)> caminoSet = new HashSet<(int, int)>(res.CaminoOptimo);
            Dictionary<(int, int), int> pasoNumero = new Dictionary<(int, int), int>();

            for (int i = 0; i < res.CaminoOptimo.Count; i++)
            {
                pasoNumero[res.CaminoOptimo[i]] = i + 1;
            }

            // Actualizar visualmente todas las celdas
            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    Button btn = botones[r, c];

                    if (r == inicio.r && c == inicio.c)
                    {
                        btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981"));
                        btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669"));
                        btn.Content = new TextBlock { Text = "A", FontWeight = FontWeights.Bold, Foreground = Brushes.White, FontSize = 14 };
                    }
                    else if (r == destino.r && c == destino.c)
                    {
                        btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669"));
                        btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#047857"));
                        btn.Content = new TextBlock { Text = "B", FontWeight = FontWeights.Bold, Foreground = Brushes.White, FontSize = 14 };
                    }
                    else if (laberinto[r, c] == LaberintoService.ESTADO_MURO)
                    {
                        btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155"));
                        btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));
                        btn.Content = null;
                    }
                    else if (caminoSet.Contains((r, c)))
                    {
                        btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D1FAE5"));
                        btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981"));
                        btn.Content = new TextBlock
                        {
                            Text = pasoNumero[(r, c)].ToString(),
                            FontSize = 11,
                            FontWeight = FontWeights.Bold,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"))
                        };
                    }
                    else
                    {
                        btn.Background = Brushes.White;
                        btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBD5E1"));
                        btn.Content = null;
                    }
                }
            }

            // Actualizar panel lateral
            lblNodosExplorados.Text = $"{res.NodosExplorados} celdas";
            lblCoordenadas.Text = res.DetalleCoordenadas;

            if (res.HayCamino)
            {
                cardEstadoRuta.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECFDF5"));
                cardEstadoRuta.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A7F3D0"));
                lblEstadoHeader.Text = "ESTADO DE LA BÚSQUEDA";
                lblEstadoHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065F46"));
                lblEstadoRuta.Text = "¡Ruta Encontrada!";
                lblEstadoRuta.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#047857"));
                lblLongitudRuta.Text = $"{res.LongitudCamino} pasos";
                lblStatus.Text = $"Ruta óptima trazada con éxito en {res.LongitudCamino} pasos.";
            }
            else
            {
                cardEstadoRuta.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF2F2"));
                cardEstadoRuta.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FECACA"));
                lblEstadoHeader.Text = "ESTADO DE LA BÚSQUEDA";
                lblEstadoHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#991B1B"));
                lblEstadoRuta.Text = "Sin Camino Disponible";
                lblEstadoRuta.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                lblLongitudRuta.Text = "0 pasos";
                lblStatus.Text = "No existe un camino continuo hacia la meta. Despeja algunos muros.";
            }
        }

        private void btnGenerarAleatorio_Click(object sender, RoutedEventArgs e)
        {
            GenerarLaberinto();
            ResolverRuta();
        }

        private void btnLimpiarMuros_Click(object sender, RoutedEventArgs e)
        {
            for (int r = 0; r < FILAS; r++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    laberinto[r, c] = LaberintoService.ESTADO_LIBRE;
                }
            }
            ResolverRuta();
        }

        private void btnResolver_Click(object sender, RoutedEventArgs e)
        {
            ResolverRuta();
        }
    }
}
