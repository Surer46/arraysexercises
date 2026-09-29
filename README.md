# Práctica de Arreglos y Matrices — Estructura de Datos
**Universidad Tecnológica Metropolitana (UTM)**  
**División TIC — Ingeniería en Tecnologías de la Información e Innovación Digital (DSM)**  
**Asignatura:** Estructura de Datos (Cuatrimestre 4, Grupos 4A-4D)  
**Docente:** Ing. Mirian Magaly Canché Caamal  
**Instrumento de Evaluación:** F-SGC-033 | Revisión 00  

---

## 🌟 Descripción del Proyecto

Este repositorio contiene la solución completa de la práctica de arreglos y matrices desarrollada en **C# (.NET / WPF)**. Cumple con los criterios de evaluación solicitados:

* **Arquitectura Orientada a Objetos (POO):** Separación estricta de lógica algorítmica y presentación visual.
* **Ejecutables Independientes:** Cada ejercicio genera su propio archivo binario `.exe` standalone.
* **Diseño Cupertino / Material:** Tarjetas redondeadas, sombras sutiles, botones interactivos estilo píldora y paleta oficial **Blanco y Verde Esmeralda (`#10B981`, `#059669`, `#ECFDF5`)**.
* **Nomenclatura Estandarizada:** Formularios (`frm...`), botones (`btn...`), etiquetas (`lbl...`), cajas de texto (`txt...`).
* **Validación de Entradas:** Manejo de excepciones para entradas inválidas, campos faltantes y divisiones por cero.
* **Nivel de Dificultad Alto (10/10):** Inclusión de los Ejercicios 8 y 9 requeridos en el instrumento para alcanzar la calificación máxima.

---

## 📂 Directorio de Ejecutables (`Ejecutables_EXE/`)

Los ejecutables listos para usar se encuentran en la carpeta [`Ejecutables_EXE/`](./Ejecutables_EXE):

| # | Ejercicio | Archivo Ejecutable | Descripción |
| :---: | :--- | :--- | :--- |
| 🎛️ | **Menú Suite Global** | `MenuPrincipal_Suite.exe` | Lanzador visual de la suite completa que permite abrir cualquier ejercicio sin salir. |
| 1️⃣ | **Ejercicio 1** | `Ejercicio1_ContadorCeros.exe` | Conteo y clasificación de ceros por renglón en matriz de 5x5. |
| 2️⃣ | **Ejercicio 2** | `Ejercicio2_CuadroMagico.exe` | Verificación de cuadrado mágico, constante mágica y generador siamés. |
| 3️⃣ | **Ejercicio 3** | `Ejercicio3_OperacionesMatrices2x2.exe` | Suma, resta, producto simple y división simple término a término. |
| 4️⃣ | **Ejercicio 4** | `Ejercicio4_MatrizIdentidad.exe` | Generador interactivo de matriz identidad NxN (diagonal 1s, resto 0s). |
| 5️⃣ | **Ejercicio 5** | `Ejercicio5_EstadisticasMatriz5x10.exe` | Matriz 5x10 con vectores de suma y promedio: A, B (filas) y C, D (columnas). |
| 6️⃣ | **Ejercicio 6** | `Ejercicio6_ResumenVentas.exe` | Resumen anual de ventas 12x5: venta menor/mayor con fecha, total y ventas por día. |
| 7️⃣ | **Ejercicio 7** | `Ejercicio7_ResumenCalificaciones.exe` | Análisis escolar (12 alumnos x 3 parciales): promedios, récord y tabla de frecuencias. |
| 8️⃣ | **Ejercicio 8 (Nivel Alto)** | `Ejercicio8_LaberintoPathfinding.exe` | Búsqueda de rutas óptimas en laberintos interactivos mediante BFS y matrices de visitados. |
| 9️⃣ | **Ejercicio 9 (Nivel Alto)** | `Ejercicio9_MultiplicacionAvanzada.exe` | Producto matricial $O(n^3)$ de dimensiones arbitrarias, transpuesta y determinante por cofactores. |

---

## 🛠️ Requisitos e Instalación

Para ejecutar los archivos `.exe` o compilar el código fuente desde el repositorio:

1. **Windows 10 / 11 (x64)**.
2. **.NET Desktop Runtime 10** o superior (incluido por defecto si tienes Visual Studio o .NET SDK).
3. Para compilar desde la terminal:
   ```bash
   dotnet build EjerciciosArrays.sln
   ```

---

## 📑 Documentación Completa

El análisis formal de **Entradas, Proceso y Salida**, descripción detallada del problema y justificación de cada ejercicio se encuentra redactado en el archivo:
* [`DOCUMENTACION_PRACTICA.md`](./DOCUMENTACION_PRACTICA.md)
