# UNIVERSIDAD TECNOLÓGICA METROPOLITANA (UTM)
## División de Tecnologías de la Información y Comunicación (TIC)
### Asignatura: Estructura de Datos (Cuatrimestre 4, Grupos 4A-4D)
**Docente:** Ing. Mirian Magaly Canché Caamal  
**Instrumento de Evaluación:** F-SGC-033 | Revisión 00  
**Tema:** Conceptos básicos de estructuras de datos orientadas a objetos — Práctica: Arreglos y Matrices  

---

## 📌 Resumen General de la Solución Desarrollada

Se ha implementado una solución integral bajo el paradigma de **Programación Orientada a Objetos (POO)** en **C# (.NET)**, separando estrictamente la capa de presentación gráfica de la capa de lógica algorítmica.

- **Diseño Visual:** Interfaz gráfica basada en principios **Cupertino** (tarjetas blancas redondeadas, sombras sutiles, botones tipo píldora) combinada con la paleta de colores oficial **Blanco / Verde Esmeralda (`#10B981`, `#059669`, `#ECFDF5`)**, tipografía Segoe UI y vectoriales nítidos de **Material Design**.
- **Nomenclatura Estándar:** Todos los componentes siguen las directrices de nomenclatura requeridas: botones (`btn...`), etiquetas (`lbl...`), cajas de texto (`txt...`), formularios (`frm...`).
- **Validación de Entradas:** Manejo robusto de excepciones ante campos vacíos (*"Hay datos faltantes"*) o formatos inválidos (*"Introducir sólo números"*), además de protección contra divisiones por cero.
- **Separación de Métodos Específicos:** Cada ejercicio cuenta con su clase de servicio independiente en la carpeta `Servicios/`, con comentarios pedagógicos que documentan su funcionamiento paso a paso y su complejidad algorítmica.
- **Ejecutables Standalone:** Cada ejercicio cuenta con su propio ejecutable `.exe` independiente en la carpeta `Ejecutables_EXE/` y en su correspondiente carpeta `bin/Debug/net10.0-windows/`. Además, se incluye el lanzador global `MenuPrincipal_Suite.exe` para navegar entre todos ellos sin cerrar la aplicación.
- **Ejercicios para Calificación 10/10:** Se agregaron los Ejercicios 8 y 9 con nivel de dificultad alto aplicando arreglos bidimensionales (Pathfinding con BFS en cuadrícula y Álgebra Matricial Avanzada con multiplicación arbitraria, transpuesta y determinantes).

---

## 📂 Directorio de Ejecutables Independientes (`Ejecutables_EXE/`)

| Ejercicio | Archivo Ejecutable (.EXE) | Descripción Principal |
| :--- | :--- | :--- |
| **0. Menú Suite** | `MenuPrincipal_Suite.exe` | Lanzador visual de la suite completa con navegación entre ejercicios |
| **1. Ejercicio 1** | `Ejercicio1_ContadorCeros.exe` | Conteo de ceros por renglón en matriz 5x5 |
| **2. Ejercicio 2** | `Ejercicio2_CuadroMagico.exe` | Validación de sumas y constante mágica con generador |
| **3. Ejercicio 3** | `Ejercicio3_OperacionesMatrices2x2.exe` | Suma, resta, producto simple y división simple de matrices 2x2 |
| **4. Ejercicio 4** | `Ejercicio4_MatrizIdentidad.exe` | Generador dinámico de matriz identidad NxN (diagonal 1s, resto 0s) |
| **5. Ejercicio 5** | `Ejercicio5_EstadisticasMatriz5x10.exe` | Matriz 5x10 con arreglos vectoriales A, B (filas) y C, D (columnas) |
| **6. Ejercicio 6** | `Ejercicio6_ResumenVentas.exe` | Resumen anual de ventas 12x5: menor, mayor, total y por día |
| **7. Ejercicio 7** | `Ejercicio7_ResumenCalificaciones.exe` | Análisis escolar: promedios, récord, reprobados y tabla de frecuencias |
| **8. Ejercicio 8** | `Ejercicio8_LaberintoPathfinding.exe` | **Nivel Alto:** Búsqueda de camino óptimo en matriz 2D con BFS y obstáculos |
| **9. Ejercicio 9** | `Ejercicio9_MultiplicacionAvanzada.exe` | **Nivel Alto:** Producto matricial O(n³), transpuesta y determinante por cofactores |

---

## 📝 Documentación Individual por Ejercicio (Entradas, Proceso, Salida)

### 📌 Ejercicio 1: Contador de Ceros por Renglón
- **Descripción:** Dado un arreglo bidimensional de 5x5 números enteros, calcular e informar cuántos ceros aparecen en cada renglón de la matriz y el total general.
- **Entradas:** 
  - Matriz bidimensional de $5 \times 5$ (`int[5, 5]`), editable o cargada con el arreglo de la práctica:
    ```
    0 2 5 7 6
    0 0 0 3 8
    2 9 6 3 4
    1 5 6 1 4
    0 9 2 5 0
    ```
- **Proceso:**
  - Método separado: `MatrizContadorCerosService.ContarCerosPorRenglon(int[,] matriz)`
  - Se recorre la matriz con dos ciclos anidados: para cada renglón $i \in [0, 4]$, se itera cada columna $j \in [0, 4]$. Si `matriz[i, j] == 0`, se incrementa el contador de la fila actual `conteo[i]`.
  - Complejidad: $O(F \times C) = O(25)$ operaciones.
- **Salida:**
  - Renglón 1: 1 cero
  - Renglón 2: 3 ceros
  - Renglón 3: 0 ceros
  - Renglón 4: 0 ceros
  - Renglón 5: 2 ceros
  - Total General: 6 ceros. Celdas con ceros resaltadas con fondo verde menta.

---

### 📌 Ejercicio 2: Cuadrado Mágico y Constante Mágica
- **Descripción:** Determina si una matriz cuadrada de tamaño $N \times N$ es un cuadrado mágico (la suma de cada fila, cada columna y ambas diagonales es idéntica). Devuelve la constante mágica calculada.
- **Entradas:**
  - Dimensión $N$ ($3 \times 3, 4 \times 4, 5 \times 5$).
  - Elementos enteros de la matriz (ejemplo del PDF: `[4,9,2; 3,5,7; 8,1,6]`).
- **Proceso:**
  - Método separado: `CuadroMagicoService.AnalizarCuadroMagico(int[,] matriz)`
  - 1. Suma de cada fila $i$: $\sum_{j=0}^{n-1} M[i,j]$.
  - 2. Suma de cada columna $j$: $\sum_{i=0}^{n-1} M[i,j]$.
  - 3. Suma de diagonal principal: $\sum_{i=0}^{n-1} M[i,i]$.
  - 4. Suma de diagonal secundaria: $\sum_{i=0}^{n-1} M[i, n-1-i]$.
  - 5. Comprobación de igualdad de todas las sumas respecto a la constante objetivo $K$.
  - 6. Verificación de números del $1$ al $N^2$ sin repetición.
- **Salida:**
  - Diagnóstico: "¡Es Cuadro Mágico!" / "No es Cuadro Mágico".
  - Constante Mágica: 15 (para el ejemplo del PDF).
  - Sumas laterales `SumFils` (15, 15, 15) y sumas inferiores `SumCols` (15, 15, 15).
  - Sumas de diagonales: Principal = 15, Secundaria = 15.

---

### 📌 Ejercicio 3: Operaciones con Matrices 2x2
- **Descripción:** Lee dos matrices cuadradas de tamaño $2 \times 2$ y calcula suma, resta, producto simple y división simple elemento a elemento.
- **Entradas:**
  - Matriz 1 ($A$): `[10, 5; 8, 2]`
  - Matriz 2 ($B$): `[2, 4; 6, 8]`
- **Proceso:**
  - Métodos en `Matrices2x2Service`:
    - a) Suma: $C_{i,j} = A_{i,j} + B_{i,j}$
    - b) Resta: $C_{i,j} = A_{i,j} - B_{i,j}$
    - c) Producto Simple: $C_{i,j} = A_{i,j} \cdot B_{i,j}$
    - d) División Simple: $C_{i,j} = A_{i,j} / B_{i,j}$ con validación de división entre cero ($B_{i,j} \neq 0$).
- **Salida:**
  - Suma: `[12, 9; 14, 10]`
  - Resta: `[8, 1; 2, -6]`
  - Producto: `[20, 20; 48, 16]`
  - División: `[5, 1.25; 1.33, 0.25]`

---

### 📌 Ejercicio 4: Generador de Matriz Identidad
- **Descripción:** Llena una matriz cuadrada de números enteros de orden $N \times N$ de tal forma que la diagonal principal contenga 1s y las demás posiciones 0s.
- **Entradas:**
  - Orden $N$ de la matriz (seleccionable dinámicamente de 2 a 10).
- **Proceso:**
  - Método separado: `MatrizIdentidadService.GenerarMatrizIdentidad(int n)`
  - Recorrido con doble ciclo for $(i, j)$: si $i == j \Rightarrow M[i,j] = 1$, si $i \neq j \Rightarrow M[i,j] = 0$.
- **Salida:**
  - Tablero gráfico interactivo con celdas de la diagonal resaltadas en verde esmeralda con valor 1.
  - Conteo de Unos en diagonal ($N$), ceros fuera de diagonal ($N^2 - N$) y propiedades de álgebra lineal.

---

### 📌 Ejercicio 5: Matriz 5x10 con Sumas y Promedios (Arreglos A, B, C, D)
- **Descripción:** Llena una matriz de $5 \times 10$ con números y calcula la suma y promedio por fila y columna almacenándolos en 4 arreglos unidimensionales con el formato visual exacto del PDF.
- **Entradas:**
  - Matriz de 5 renglones por 10 columnas (`int[5, 10]`).
- **Proceso:**
  - Método separado: `EstadisticasMatrizService.CalcularEstadisticas(int[,] matriz)`
  - Arreglo A: $A[i] = \sum_{j=0}^{9} M[i,j]$
  - Arreglo B: $B[i] = A[i] / 10.0$
  - Arreglo C: $C[j] = \sum_{i=0}^{4} M[i,j]$
  - Arreglo D: $D[j] = C[j] / 5.0$
- **Salida:**
  - Tabla 5x10 en el centro.
  - A la derecha: Columna A (Sumas de filas: ej. Fila 1 = 26) y Columna B (Promedios: ej. Fila 1 = 2.6).
  - En la parte inferior: Fila C (Sumas de columnas: ej. Col 1 = 27) y Fila D (Promedios: ej. Col 1 = 5.4).
  - Suma total general y promedio global de la matriz.

---

### 📌 Ejercicio 6: Resumen Anual de Ventas (12 Meses x 5 Días)
- **Descripción:** Gestiona un arreglo bidimensional con el resumen de ventas de 12 meses (filas) y 5 días laborales (columnas: Lunes a Viernes).
- **Entradas:**
  - Arreglo bidimensional de $12 \times 5$ con la tabla de ventas oficial del PDF.
- **Proceso:**
  - Método separado: `ResumenVentasService.AnalizarVentas(double[,] matriz)`
  - a) Inicialización del arreglo 12x5.
  - b) Búsqueda del menor valor con registro de su índice de fila (mes) y columna (día).
  - c) Búsqueda del mayor valor con registro de su índice de fila (mes) y columna (día).
  - d) Sumatoria acumulativa de todas las celdas para la venta total general.
  - e) Sumatoria por cada una de las 5 columnas a lo largo de los 12 meses para obtener la venta por día de la semana.
- **Salida:**
  - Menor venta: `$ 3.00` realizada en **Noviembre, Martes** (resaltada en amarillo en la tabla).
  - Mayor venta: `$ 82.00` realizada en **Noviembre, Viernes** (resaltada en verde en la tabla).
  - Venta total general: `$ 2,056.00`.
  - Ventas por día: Lunes: `$ 420.00`, Martes: `$ 377.00`, Miércoles: `$ 472.00`, Jueves: `$ 219.00`, Viernes: `$ 568.00`.

---

### 📌 Ejercicio 7: Resumen de Calificaciones Escolares
- **Descripción:** Analiza una matriz de 12 alumnos (filas) por 3 parciales (columnas).
- **Entradas:**
  - Arreglo bidimensional de $12 \times 3$ con las calificaciones del PDF.
- **Proceso:**
  - Método separado: `ResumenCalificacionesService.AnalizarCalificaciones(double[,] matriz)`
  - a) Promedio de cada alumno: $P[i] = (\sum_{j=0}^{2} M[i,j]) / 3.0$.
  - b) Búsqueda del promedio más alto y su alumno.
  - c) Búsqueda del promedio más bajo y su alumno.
  - d) Conteo de parciales con nota $< 7.0$ (reprobados).
  - e) Clasificación de promedios finales en 6 intervalos: `[0, 4.9]`, `[5.0, 5.9]`, `[6.0, 6.9]`, `[7.0, 7.9]`, `[8.0, 8.9]`, `[9.0, 10.0]`.
- **Salida:**
  - Promedio de cada uno de los 12 alumnos con etiqueta "Aprobado" o "Reprobado".
  - Promedio más alto: **9.40** (Alumno 10).
  - Promedio más bajo: **5.00** (Alumno 8).
  - Total de parciales reprobados: **13 parciales**.
  - Distribución de calificaciones en formato exacto del PDF:
    - 0 – 4.9: 0 Alumnos
    - 5.0 – 5.9: 2 Alumnos
    - 6.0 – 6.9: 2 Alumnos
    - 7.0 – 7.9: 4 Alumnos
    - 8.0 – 8.9: 3 Alumnos
    - 9.0 – 10.0: 1 Alumnos

---

### 📌 Ejercicio 8 (Nivel de Dificultad Alto - Calificación 10/10): Pathfinding en Matriz 2D
- **Descripción:** Búsqueda interactiva del camino óptimo en una cuadrícula con obstáculos (laberinto 2D) aplicando el algoritmo Breadth-First Search (BFS) y matrices de visitados y predecesores.
- **Entradas:**
  - Cuadrícula de $8 \times 8$ interactiva donde el usuario puede activar/desactivar muros haciendo clic en las celdas, o generar laberintos aleatorios. Punto de inicio en $[0, 0]$ y meta en $[7, 7]$.
- **Proceso:**
  - Método separado: `LaberintoService.EncontrarRutaOptima(...)`
  - Utiliza una matriz booleana `visitado[8,8]` y una matriz de tuplas `padre[8,8]`.
  - Explora mediante una cola las 4 direcciones ortogonales respetando los límites del arreglo bidimensional.
  - Reconstruye la ruta óptima mediante backtracking desde el destino hacia el origen.
- **Salida:**
  - Animación y resaltado visual de las casillas del camino con su número de paso correlativo.
  - Contador de nodos explorados y longitud del camino.
  - Detalle textual de coordenadas del trayecto: `[1,1] → [1,2] → [2,2] → ...`.

---

### 📌 Ejercicio 9 (Nivel de Dificultad Alto - Calificación 10/10): Multiplicación Matricial Avanzada, Transpuesta y Determinante
- **Descripción:** Operaciones avanzadas de álgebra lineal sobre arreglos bidimensionales con dimensiones dinámicas ($N \times M$ y $M \times P$).
- **Entradas:**
  - Matriz A ($N \times M$) y Matriz B ($M \times P$) con presets ($2 \times 2$, $2 \times 3 \cdot 3 \times 2$, $3 \times 3$).
- **Proceso:**
  - Métodos en `AlgebraMatricialService`:
    - 1. Multiplicación Matricial $C = A \times B$ mediante el algoritmo clásico de triple bucle $O(N \cdot M \cdot P)$:
      $$C_{i,j} = \sum_{k=0}^{M-1} A_{i,k} \cdot B_{k,j}$$
    - 2. Matriz Transpuesta $A^T$: intercambio de subíndices $A^T[j, i] = A[i, j]$.
    - 3. Determinante $\det(A)$ mediante expansión por cofactores (Teorema de Laplace) con submatrices recursivas.
- **Salida:**
  - Matriz producto $C$ resultante.
  - Matriz transpuesta $A^T$.
  - Determinante $\det(A)$.
  - Desglose detallado paso a paso con las fórmulas de productos escalares para cada celda de la matriz.
