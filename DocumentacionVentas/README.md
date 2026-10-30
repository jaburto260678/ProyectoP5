
# Módulo de Ventas - ProyectoP5

## 1. Descripción

El módulo de Ventas permite registrar las compras de
productos realizadas por los clientes del sistema.

Se desarrolló en C# utilizando .NET 8.0, Windows Forms
y una base de datos MySQL.

## 2. Funcionalidades

- Consultar clientes activos.
- Consultar productos disponibles.
- Mostrar precios y existencias.
- Agregar productos a una venta.
- Calcular subtotales y total.
- Validar cantidades y precios.
- Registrar ventas en MySQL.
- Guardar el detalle de cada venta.
- Descontar automáticamente el inventario.
- Consultar el historial de ventas.
- Filtrar ventas por fechas.
- Consultar los productos de una venta registrada.

## 3. Arquitectura

El módulo utiliza tres capas principales:

### Presentación

Contiene los formularios:

- FrmVentas.cs
- FrmHistorialVentas.cs

Permite la interacción con el usuario.

### Negocio

Contiene:

- VentaServicio.cs
- VentaHistorialServicio.cs

Aplica las validaciones y las reglas del sistema.

### Datos

Contiene:

- VentaConexion.cs
- VentaDAO.cs
- VentaHistorialDAO.cs

Realiza las operaciones de consulta y registro en MySQL.

## 4. Entidades

- VentaRegistro
- DetalleVenta
- VentaClienteOpcion
- VentaProductoOpcion
- VentaHistorialItem
- VentaDetalleHistorialItem

Estas clases representan los datos utilizados
por el módulo.

## 5. Base de datos

Base de datos: sistema_ventas

Tablas utilizadas:

- clientes
- productos
- ventas
- detalle_venta

Una venta pertenece a un cliente y puede contener
varios productos.

## 6. Proceso para registrar una venta

1. El usuario selecciona un cliente.
2. Selecciona uno o varios productos.
3. Especifica las cantidades.
4. El formulario calcula el total preliminar.
5. El usuario confirma la venta.
6. La capa de negocio valida los datos.
7. La capa de datos verifica precios y stock.
8. Se registra la cabecera de la venta.
9. Se registran los detalles.
10. Se actualiza el inventario.
11. La transacción confirma los cambios.
12. El formulario informa el resultado.

Si ocurre un error antes de confirmar la transacción,
se revierten las operaciones de la venta.

## 7. Control de inventario

El registro de ventas utiliza transacciones MySQL
y consultas de bloqueo FOR UPDATE para proteger
las existencias durante operaciones simultáneas.

Se comprueba que haya suficientes unidades antes
de confirmar cada venta.

## 8. Validaciones

Se comprueba que:

- Exista un cliente válido.
- La venta tenga al menos un producto.
- Los identificadores de productos sean válidos.
- Las cantidades sean mayores que cero.
- Los precios no sean negativos.
- Exista suficiente inventario.
- Los precios no hayan cambiado durante la compra.

## 9. Pruebas

Las pruebas unitarias utilizan MSTest y están
implementadas en VentaServicioTests.cs.

Se comprueban los cálculos y las principales
validaciones del módulo.

También se realizan pruebas manuales con MySQL
para verificar el registro y el inventario.

## 10. Configuración

La conexión de Ventas utiliza la variable de
entorno SISTEMA_VENTAS_CONNECTION.

Las credenciales de MySQL no deben guardarse
directamente en el repositorio.

## 11. Limitaciones actuales

- No se han implementado anulaciones de ventas.
- Los formularios se encuentran pendientes
  de integración con el menú principal del equipo.
- La exportación de reportes corresponde al
  alcance general del sistema.
- Las pruebas unitarias no sustituyen las pruebas
  de integración con una base de datos real.

## 12. Tecnologías

- C#
- .NET 8.0
- Windows Forms
- MySQL
- MySqlConnector
- MSTest
- Git y GitHub
