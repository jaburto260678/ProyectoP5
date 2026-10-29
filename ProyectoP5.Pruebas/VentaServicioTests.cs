
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProyectoP5.Entidades;
using ProyectoP5.Negocio;

namespace ProyectoP5.Pruebas
{
    /// <summary>
    /// Pruebas unitarias para comprobar las validaciones
    /// y los cálculos del módulo de Ventas.
    /// </summary>
    [TestClass]
    public class VentaServicioTests
    {
        private VentaServicio servicio = null!;

        /// <summary>
        /// Prepara el servicio antes de cada prueba.
        /// </summary>
        [TestInitialize]
        public void PrepararPrueba()
        {
            servicio = new VentaServicio();
        }

        /// <summary>
        /// Comprueba el subtotal y el total
        /// de un producto vendido.
        /// </summary>
        [TestMethod]
        public void CalcularTotal_UnProducto_TotalCorrecto()
        {
            var detalles = new List<DetalleVenta>
            {
                new DetalleVenta
                {
                    ProductoId = 1,
                    NombreProducto = "Mouse inalámbrico",
                    Cantidad = 2,
                    PrecioUnitario = 6500m
                }
            };

            decimal resultado = servicio.CalcularTotal(detalles);

            Assert.AreEqual(13000m, resultado);
        }

        /// <summary>
        /// Comprueba el total con varios productos.
        /// </summary>
        [TestMethod]
        public void CalcularTotal_VariosProductos_TotalCorrecto()
        {
            var detalles = new List<DetalleVenta>
            {
                new DetalleVenta
                {
                    ProductoId = 1,
                    NombreProducto = "Mouse inalámbrico",
                    Cantidad = 2,
                    PrecioUnitario = 6500m
                },
                new DetalleVenta
                {
                    ProductoId = 4,
                    NombreProducto = "Cable USB",
                    Cantidad = 3,
                    PrecioUnitario = 2500m
                }
            };

            decimal resultado = servicio.CalcularTotal(detalles);

            Assert.AreEqual(20500m, resultado);
        }

        /// <summary>
        /// Comprueba que una venta sin cliente
        /// no sea considerada válida.
        /// </summary>
        [TestMethod]
        public void ValidarVenta_SinCliente_LanzaExcepcion()
        {
            var venta = new VentaRegistro
            {
                ClienteId = 0,
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = 1,
                        Cantidad = 1,
                        PrecioUnitario = 6500m
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(
                () => servicio.ValidarVenta(venta));
        }

        /// <summary>
        /// Comprueba que no pueda registrarse
        /// una venta sin productos.
        /// </summary>
        [TestMethod]
        public void ValidarVenta_SinProductos_LanzaExcepcion()
        {
            var venta = new VentaRegistro
            {
                ClienteId = 1,
                Detalles = new List<DetalleVenta>()
            };

            Assert.ThrowsException<ArgumentException>(
                () => servicio.ValidarVenta(venta));
        }

        /// <summary>
        /// Comprueba que una cantidad de cero
        /// no esté permitida.
        /// </summary>
        [TestMethod]
        public void ValidarVenta_CantidadCero_LanzaExcepcion()
        {
            var venta = new VentaRegistro
            {
                ClienteId = 1,
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = 1,
                        Cantidad = 0,
                        PrecioUnitario = 6500m
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(
                () => servicio.ValidarVenta(venta));
        }

        /// <summary>
        /// Comprueba que las cantidades negativas
        /// sean rechazadas.
        /// </summary>
        [TestMethod]
        public void ValidarVenta_CantidadNegativa_LanzaExcepcion()
        {
            var venta = new VentaRegistro
            {
                ClienteId = 1,
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = 1,
                        Cantidad = -2,
                        PrecioUnitario = 6500m
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(
                () => servicio.ValidarVenta(venta));
        }

        /// <summary>
        /// Comprueba que el precio no pueda
        /// ser menor que cero.
        /// </summary>
        [TestMethod]
        public void ValidarVenta_PrecioNegativo_LanzaExcepcion()
        {
            var venta = new VentaRegistro
            {
                ClienteId = 1,
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = 1,
                        Cantidad = 1,
                        PrecioUnitario = -100m
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(
                () => servicio.ValidarVenta(venta));
        }

        /// <summary>
        /// Comprueba que un producto sin ID válido
        /// sea rechazado.
        /// </summary>
        [TestMethod]
        public void ValidarVenta_ProductoInvalido_LanzaExcepcion()
        {
            var venta = new VentaRegistro
            {
                ClienteId = 1,
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = 0,
                        Cantidad = 1,
                        PrecioUnitario = 6500m
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(
                () => servicio.ValidarVenta(venta));
        }

        /// <summary>
        /// Comprueba que un mismo producto
        /// no tenga dos precios diferentes.
        /// </summary>
        [TestMethod]
        public void ValidarVenta_MismoProductoPreciosDiferentes_LanzaExcepcion()
        {
            var venta = new VentaRegistro
            {
                ClienteId = 1,
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = 1,
                        Cantidad = 1,
                        PrecioUnitario = 6500m
                    },
                    new DetalleVenta
                    {
                        ProductoId = 1,
                        Cantidad = 1,
                        PrecioUnitario = 7000m
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(
                () => servicio.ValidarVenta(venta));
        }

        /// <summary>
        /// Comprueba que una venta correcta
        /// pase las validaciones sin errores.
        /// </summary>
        [TestMethod]
        public void ValidarVenta_DatosCorrectos_NoLanzaExcepcion()
        {
            var venta = new VentaRegistro
            {
                ClienteId = 1,
                Fecha = DateTime.Now,
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = 1,
                        NombreProducto = "Mouse inalámbrico",
                        Cantidad = 2,
                        PrecioUnitario = 6500m
                    },
                    new DetalleVenta
                    {
                        ProductoId = 4,
                        NombreProducto = "Cable USB",
                        Cantidad = 3,
                        PrecioUnitario = 2500m
                    }
                }
            };

            servicio.ValidarVenta(venta);

            decimal total = servicio.CalcularTotal(
                venta.Detalles);

            Assert.AreEqual(20500m, total);
        }

        /// <summary>
        /// Comprueba que el subtotal se calcule
        /// multiplicando cantidad por precio.
        /// </summary>
        [TestMethod]
        public void DetalleVenta_CalcularSubtotal_Correcto()
        {
            var detalle = new DetalleVenta
            {
                ProductoId = 3,
                NombreProducto = "Audífonos",
                Cantidad = 3,
                PrecioUnitario = 12000m
            };

            Assert.AreEqual(36000m, detalle.Subtotal);
        }

        /// <summary>
        /// Comprueba que el total calculado
        /// por la entidad sea correcto.
        /// </summary>
        [TestMethod]
        public void VentaRegistro_CalcularTotal_Correcto()
        {
            var venta = new VentaRegistro
            {
                ClienteId = 1,
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = 1,
                        Cantidad = 2,
                        PrecioUnitario = 6500m
                    },
                    new DetalleVenta
                    {
                        ProductoId = 4,
                        Cantidad = 3,
                        PrecioUnitario = 2500m
                    }
                }
            };

            Assert.AreEqual(20500m, venta.Total);
        }
    }
}
