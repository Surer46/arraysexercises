using System;
using System.Collections.Generic;

namespace Ejercicio8_LaberintoPathfinding.Servicios
{
    public class ResultadoRuta
    {
        public bool HayCamino { get; set; }
        public List<(int r, int c)> CaminoOptimo { get; set; } = new List<(int, int)>();
        public int NodosExplorados { get; set; }
        public int LongitudCamino => CaminoOptimo?.Count ?? 0;
        public string DetalleCoordenadas { get; set; }
    }

    /// <summary>
    /// Servicio de dificultad alta que resuelve la búsqueda del camino más corto en un 
    /// arreglo bidimensional (laberinto 2D) mediante el algoritmo BFS (Breadth-First Search).
    /// Demuestra el uso avanzado de matrices de estados, distancias y predecesores.
    /// </summary>
    public static class LaberintoService
    {
        public const int ESTADO_LIBRE = 0;
        public const int ESTADO_MURO = 1;

        // Desplazamientos ortogonales (Arriba, Abajo, Izquierda, Derecha)
        private static readonly int[] dFila = { -1, 1, 0, 0 };
        private static readonly int[] dCol = { 0, 0, -1, 1 };

        /// <summary>
        /// MÉTODO ESPECÍFICO: EncontrarRutaOptima
        /// -------------------------------------------------------------
        /// FUNCIONAMIENTO PASO A PASO:
        /// 1. Instancia una matriz booleana 'visitado[filas, columnas]' para no repetir celdas.
        /// 2. Instancia una matriz de tuplas 'padre[filas, columnas]' para reconstruir la ruta hacia atrás.
        /// 3. Utiliza una Cola (Queue) de coordenadas (r, c) para explorar en anchura nivel por nivel (garantiza la ruta más corta).
        /// 4. Marca el inicio como visitado y lo encola.
        /// 5. Mientras la cola no esté vacía:
        ///    - Desencola la casilla actual (r, c).
        ///    - Si (r, c) es el destino, se detiene la búsqueda y se reconstruye la ruta siguiendo los predecesores en 'padre[,]'.
        ///    - De lo contrario, para cada una de las 4 direcciones adyacentes:
        ///      * Comprueba que la celda vecina esté dentro de los límites del arreglo.
        ///      * Comprueba que no sea un muro (matriz[nr, nc] != ESTADO_MURO).
        ///      * Comprueba que no haya sido visitada previamente.
        ///      * Si cumple, la marca como visitada, guarda a (r, c) como su padre, y la encola.
        /// 6. Complejidad temporal: O(Filas * Columnas).
        /// </summary>
        public static ResultadoRuta EncontrarRutaOptima(int[,] laberinto, (int r, int c) inicio, (int r, int c) destino)
        {
            if (laberinto == null) throw new ArgumentNullException(nameof(laberinto));
            int filas = laberinto.GetLength(0);
            int columnas = laberinto.GetLength(1);

            bool[,] visitado = new bool[filas, columnas];
            (int r, int c)[,] padre = new (int, int)[filas, columnas];

            Queue<(int r, int c)> cola = new Queue<(int, int)>();
            cola.Enqueue(inicio);
            visitado[inicio.r, inicio.c] = true;

            int nodosExplorados = 0;
            bool destinoAlcanzado = false;

            while (cola.Count > 0)
            {
                var actual = cola.Dequeue();
                nodosExplorados++;

                if (actual.r == destino.r && actual.c == destino.c)
                {
                    destinoAlcanzado = true;
                    break;
                }

                for (int d = 0; d < 4; d++)
                {
                    int nr = actual.r + dFila[d];
                    int nc = actual.c + dCol[d];

                    // Validar límites del arreglo bidimensional
                    if (nr >= 0 && nr < filas && nc >= 0 && nc < columnas)
                    {
                        // Validar que sea transitable y no visitado
                        if (laberinto[nr, nc] != ESTADO_MURO && !visitado[nr, nc])
                        {
                            visitado[nr, nc] = true;
                            padre[nr, nc] = actual;
                            cola.Enqueue((nr, nc));
                        }
                    }
                }
            }

            ResultadoRuta resultado = new ResultadoRuta
            {
                HayCamino = destinoAlcanzado,
                NodosExplorados = nodosExplorados
            };

            if (destinoAlcanzado)
            {
                // Reconstrucción del camino desde el destino hasta el inicio
                List<(int r, int c)> rutaReversa = new List<(int, int)>();
                var paso = destino;

                while (!(paso.r == inicio.r && paso.c == inicio.c))
                {
                    rutaReversa.Add(paso);
                    paso = padre[paso.r, paso.c];
                }
                rutaReversa.Add(inicio);
                rutaReversa.Reverse();

                resultado.CaminoOptimo = rutaReversa;

                // Generar texto descriptivo de coordenadas
                List<string> coords = new List<string>();
                foreach (var p in rutaReversa)
                {
                    coords.Add($"[{p.r + 1},{p.c + 1}]");
                }
                resultado.DetalleCoordenadas = string.Join(" → ", coords);
            }
            else
            {
                resultado.DetalleCoordenadas = "No existe una ruta transitable hacia la meta.";
            }

            return resultado;
        }

        /// <summary>
        /// Genera un laberinto aleatorio de tamaño filas x columnas con densidad de muros controlada.
        /// Garantiza que el inicio [0,0] y el destino [filas-1, cols-1] permanezcan transitables.
        /// </summary>
        public static int[,] GenerarLaberintoAleatorio(int filas, int cols, double densidadMuros = 0.28)
        {
            Random rng = new Random();
            int[,] lab = new int[filas, cols];

            for (int r = 0; r < filas; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if ((r == 0 && c == 0) || (r == filas - 1 && c == cols - 1))
                    {
                        lab[r, c] = ESTADO_LIBRE;
                    }
                    else
                    {
                        lab[r, c] = (rng.NextDouble() < densidadMuros) ? ESTADO_MURO : ESTADO_LIBRE;
                    }
                }
            }

            return lab;
        }
    }
}
