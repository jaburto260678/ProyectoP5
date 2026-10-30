
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProyectoP5.Entidades;
using ProyectoP5.Negocio;

namespace ProyectoP5
{
    /// <summary>
    /// Formulario principal del módulo de Ventas.
    /// Permite seleccionar clientes, agregar productos
    /// y registrar ventas en MySQL.
    /// </summary>
    public partial class FrmVentas : Form
    {
        private readonly VentaServicio servicio;
        private readonly List<DetalleVenta> carrito;

        private bool registrandoVenta = false;

        private ComboBox cboClientes = null!;
        private ComboBox cboProductos = null!;
        private NumericUpDown nudCantidad = null!;
        private DataGridView dgvDetalles = null!;

        private Label lblPrecio = null!;
        private Label lblStock = null!;
        private Label lblTotal = null!;
        private Label lblEstado = null!;

        private Button btnAgregar = null!;
        private Button btnEliminar = null!;
        private Button btnLimpiar = null!;
        private Button btnRegistrar = null!;
        private Button btnActualizar = null!;

        public FrmVentas()
        {
            InitializeComponent();

            servicio = new VentaServicio();
            carrito = new List<DetalleVenta>();

            CrearInterfaz();

            Shown += (s, e) => CargarDatos();
        }

        /// <summary>
        /// Crea los controles de la interfaz gráfica.
        /// </summary>
        private void CrearInterfaz()
        {
            Text = "ProyectoP5 - Módulo de Ventas";

            StartPosition = FormStartPosition.CenterScreen;

            ClientSize = new Size(1000, 735);
            MinimumSize = new Size(1000, 770);

            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);

            // Título principal.
            Label lblTitulo = new Label
            {
                Text = "REGISTRO DE VENTAS",
                Location = new Point(30, 20),
                Size = new Size(650, 45),
                Font = new Font(
                    "Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 85)
            };

            // Fecha actual.
            Label lblFecha = new Label
            {
                Text = "Fecha: " +
                       DateTime.Now.ToString("dd/MM/yyyy"),
                Location = new Point(745, 35),
                Size = new Size(220, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Cliente.
            Label lblCliente = new Label
            {
                Text = "Cliente:",
                Location = new Point(30, 95),
                AutoSize = true
            };

            cboClientes = new ComboBox
            {
                Location = new Point(30, 125),
                Size = new Size(500, 32),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Actualizar información.
            btnActualizar = new Button
            {
                Text = "Actualizar productos",
                Location = new Point(750, 120),
                Size = new Size(215, 42),
                BackColor = Color.FromArgb(70, 85, 105),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnActualizar.Click +=
                (s, e) => ActualizarDatosManual();

            // Producto.
            Label lblProducto = new Label
            {
                Text = "Producto:",
                Location = new Point(30, 195),
                AutoSize = true
            };

            cboProductos = new ComboBox
            {
                Location = new Point(30, 225),
                Size = new Size(550, 32),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cboProductos.SelectedIndexChanged +=
                (s, e) => ActualizarInformacionProducto();

            // Cantidad.
            Label lblCantidad = new Label
            {
                Text = "Cantidad:",
                Location = new Point(610, 195),
                AutoSize = true
            };

            nudCantidad = new NumericUpDown
            {
                Location = new Point(610, 225),
                Size = new Size(110, 32),
                Minimum = 1,
                Maximum = 100000,
                Value = 1
            };

            // Agregar al carrito.
            btnAgregar = new Button
            {
                Text = "Agregar producto",
                Location = new Point(745, 220),
                Size = new Size(220, 42),
                BackColor = Color.FromArgb(30, 110, 205),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnAgregar.Click +=
                (s, e) => AgregarProducto();

            // Precio.
            lblPrecio = new Label
            {
                Text = "Precio: ₡0.00",
                Location = new Point(30, 275),
                Size = new Size(260, 30),
                ForeColor = Color.FromArgb(50, 70, 90)
            };

            // Inventario.
            lblStock = new Label
            {
                Text = "Stock disponible: 0",
                Location = new Point(310, 275),
                Size = new Size(300, 30),
                ForeColor = Color.FromArgb(50, 70, 90)
            };

            // Título del carrito.
            Label lblDetalle = new Label
            {
                Text = "PRODUCTOS DE LA VENTA",
                Location = new Point(30, 325),
                Size = new Size(450, 30),
                Font = new Font(
                    "Segoe UI", 12, FontStyle.Bold)
            };

            // Tabla de artículos.
            dgvDetalles = new DataGridView
            {
                Location = new Point(30, 365),
                Size = new Size(935, 205),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Left |
                         AnchorStyles.Right
            };

            // Quitar un producto.
            btnEliminar = new Button
            {
                Text = "Quitar seleccionado",
                Location = new Point(30, 590),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(175, 65, 65),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnEliminar.Click +=
                (s, e) => QuitarProducto();

            // Limpiar el carrito.
            btnLimpiar = new Button
            {
                Text = "Limpiar venta",
                Location = new Point(245, 590),
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(110, 120, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnLimpiar.Click +=
                (s, e) => LimpiarVenta();

            // Total.
            lblTotal = new Label
            {
                Text = "TOTAL: ₡0.00",
                Location = new Point(600, 590),
                Size = new Size(365, 45),
                Font = new Font(
                    "Segoe UI", 17, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                ForeColor = Color.FromArgb(25, 90, 55),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Estado del formulario.
            lblEstado = new Label
            {
                Text = "Listo para registrar ventas.",
                Location = new Point(30, 660),
                Size = new Size(500, 30),
                ForeColor = Color.DimGray
            };

            // Guardar la venta.
            btnRegistrar = new Button
            {
                Text = "REGISTRAR VENTA",
                Location = new Point(700, 650),
                Size = new Size(265, 45),
                BackColor = Color.FromArgb(30, 135, 75),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI", 11, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnRegistrar.Click +=
                (s, e) => RegistrarVenta();

            Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblFecha,
                lblCliente,
                cboClientes,
                btnActualizar,
                lblProducto,
                cboProductos,
                lblCantidad,
                nudCantidad,
                btnAgregar,
                lblPrecio,
                lblStock,
                lblDetalle,
                dgvDetalles,
                btnEliminar,
                btnLimpiar,
                lblTotal,
                lblEstado,
                btnRegistrar
            });

            ActualizarTabla();
        }

        /// <summary>
        /// Carga clientes y productos desde MySQL.
        /// </summary>
        private bool CargarDatos()
        {
            try
            {
                int clienteAnterior =
                    (cboClientes.SelectedItem
                    as VentaClienteOpcion)?.Id ?? 0;

                int productoAnterior =
                    (cboProductos.SelectedItem
                    as VentaProductoOpcion)?.Id ?? 0;

                // Consultar primero. Si falla MySQL,
                // conservamos las opciones anteriores.
                var clientes = servicio.ObtenerClientes();
                var productos = servicio.ObtenerProductos();

                cboClientes.DataSource = null;
                cboProductos.DataSource = null;

                cboClientes.DataSource = clientes;
                cboProductos.DataSource = productos;

                var cliente = clientes.FirstOrDefault(
                    c => c.Id == clienteAnterior);

                if (cliente != null)
                {
                    cboClientes.SelectedItem = cliente;
                }

                var producto = productos.FirstOrDefault(
                    p => p.Id == productoAnterior);

                if (producto != null)
                {
                    cboProductos.SelectedItem = producto;
                }

                ActualizarInformacionProducto();

                lblEstado.Text =
                    "Información actualizada correctamente.";

                return true;
            }
            catch (Exception ex)
            {
                lblEstado.Text =
                    "No se pudo actualizar la información.";

                MessageBox.Show(
                    "Error al consultar MySQL:\n" +
                    ex.Message,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        /// <summary>
        /// Solicita confirmación antes de actualizar
        /// si hay productos en el carrito.
        /// </summary>
        private void ActualizarDatosManual()
        {
            if (registrandoVenta)
                return;

            if (carrito.Count > 0)
            {
                var respuesta = MessageBox.Show(
                    "Al actualizar los productos se limpiará " +
                    "la venta que está preparando.\n" +
                    "¿Desea continuar?",
                    "Confirmar actualización",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes)
                    return;
            }

            if (CargarDatos())
            {
                carrito.Clear();
                ActualizarTabla();
            }
        }

        /// <summary>
        /// Muestra precio y stock disponible.
        /// </summary>
        private void ActualizarInformacionProducto()
        {
            if (cboProductos.SelectedItem
                is VentaProductoOpcion producto)
            {
                int agregado = carrito
                    .Where(p => p.ProductoId == producto.Id)
                    .Sum(p => p.Cantidad);

                int disponible = Math.Max(
                    0, producto.Stock - agregado);

                lblPrecio.Text =
                    $"Precio: ₡{producto.Precio:N2}";

                lblStock.Text =
                    $"Stock disponible: {disponible}";
            }
            else
            {
                lblPrecio.Text = "Precio: ₡0.00";
                lblStock.Text = "Stock disponible: 0";
            }
        }

        /// <summary>
        /// Agrega un producto al carrito.
        /// </summary>
        private void AgregarProducto()
        {
            if (registrandoVenta)
                return;

            if (cboProductos.SelectedItem
                is not VentaProductoOpcion producto)
            {
                MessageBox.Show(
                    "Seleccione un producto válido.");
                return;
            }

            int cantidad = (int)nudCantidad.Value;

            var existente = carrito.FirstOrDefault(
                p => p.ProductoId == producto.Id);

            int agregado = existente?.Cantidad ?? 0;

            int disponible = Math.Max(
                0, producto.Stock - agregado);

            if (cantidad > disponible)
            {
                MessageBox.Show(
                    $"No hay suficientes unidades de " +
                    $"{producto.Nombre}.\n" +
                    $"Disponibles: {disponible}",
                    "Inventario insuficiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (existente != null)
            {
                existente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new DetalleVenta
                {
                    ProductoId = producto.Id,
                    NombreProducto = producto.Nombre,
                    Cantidad = cantidad,
                    PrecioUnitario = producto.Precio
                });
            }

            ActualizarTabla();

            nudCantidad.Value = 1;

            lblEstado.Text =
                "Producto agregado correctamente.";
        }

        /// <summary>
        /// Actualiza el listado y el total de la venta.
        /// </summary>
        private void ActualizarTabla()
        {
            dgvDetalles.DataSource = null;

            dgvDetalles.DataSource = carrito.Select(p => new
            {
                p.ProductoId,
                Producto = p.NombreProducto,
                p.Cantidad,
                Precio = p.PrecioUnitario,
                p.Subtotal
            }).ToList();

            if (dgvDetalles.Columns["ProductoId"] != null)
            {
                dgvDetalles.Columns["ProductoId"]!
                    .Visible = false;
            }

            if (dgvDetalles.Columns["Precio"] != null)
            {
                dgvDetalles.Columns["Precio"]!
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvDetalles.Columns["Subtotal"] != null)
            {
                dgvDetalles.Columns["Subtotal"]!
                    .DefaultCellStyle.Format = "N2";
            }

            decimal total = carrito.Sum(p => p.Subtotal);

            lblTotal.Text = $"TOTAL: ₡{total:N2}";

            ActualizarInformacionProducto();
        }

        /// <summary>
        /// Quita el producto seleccionado del carrito.
        /// </summary>
        private void QuitarProducto()
        {
            if (registrandoVenta)
                return;

            if (dgvDetalles.CurrentRow == null)
                return;

            object? valor =
                dgvDetalles.CurrentRow.Cells["ProductoId"].Value;

            if (valor == null)
                return;

            int productoId = Convert.ToInt32(valor);

            var detalle = carrito.FirstOrDefault(
                p => p.ProductoId == productoId);

            if (detalle != null)
            {
                carrito.Remove(detalle);

                ActualizarTabla();

                lblEstado.Text =
                    "Producto eliminado del carrito.";
            }
        }

        /// <summary>
        /// Vacía el carrito sin modificar MySQL.
        /// </summary>
        private void LimpiarVenta()
        {
            if (registrandoVenta)
                return;

            carrito.Clear();

            ActualizarTabla();

            lblEstado.Text = "Carrito vacío.";
        }

        /// <summary>
        /// Habilita o deshabilita los controles
        /// durante el registro de la venta.
        /// </summary>
        private void CambiarEstadoControles(bool habilitar)
        {
            cboClientes.Enabled = habilitar;
            cboProductos.Enabled = habilitar;
            nudCantidad.Enabled = habilitar;

            btnAgregar.Enabled = habilitar;
            btnEliminar.Enabled = habilitar;
            btnLimpiar.Enabled = habilitar;
            btnActualizar.Enabled = habilitar;
            btnRegistrar.Enabled = habilitar;
        }

        /// <summary>
        /// Valida y registra la venta en MySQL.
        /// </summary>
        private void RegistrarVenta()
        {
            // Evita iniciar otra operación mientras
            // ya se está procesando una venta.
            if (registrandoVenta)
                return;

            if (cboClientes.SelectedItem
                is not VentaClienteOpcion cliente)
            {
                MessageBox.Show(
                    "Seleccione un cliente válido.");
                return;
            }

            if (carrito.Count == 0)
            {
                MessageBox.Show(
                    "Debe agregar al menos un producto.");
                return;
            }

            decimal total = carrito.Sum(p => p.Subtotal);

            DialogResult confirmacion = MessageBox.Show(
                $"¿Desea registrar esta venta por ₡{total:N2}?",
                "Confirmar venta",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            registrandoVenta = true;
            CambiarEstadoControles(false);

            lblEstado.Text = "Registrando venta...";

            try
            {
                // Crear una copia del carrito.
                var detalles = carrito.Select(
                    p => new DetalleVenta
                    {
                        ProductoId = p.ProductoId,
                        NombreProducto = p.NombreProducto,
                        Cantidad = p.Cantidad,
                        PrecioUnitario = p.PrecioUnitario
                    }).ToList();

                var venta = new VentaRegistro
                {
                    ClienteId = cliente.Id,
                    Fecha = DateTime.Now,
                    Detalles = detalles
                };

                // Enviar la operación a la capa Negocio.
                int ventaId = servicio.RegistrarVenta(venta);

                // La venta ya quedó confirmada en MySQL.
                carrito.Clear();
                ActualizarTabla();

                lblEstado.Text =
                    $"Venta #{ventaId} registrada correctamente.";

                MessageBox.Show(
                    $"Venta #{ventaId} registrada correctamente.",
                    "Venta guardada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Actualizar el catálogo y las existencias.
                CargarDatos();
            }
            catch (Exception ex)
            {
                lblEstado.Text =
                    "Ocurrió un problema al registrar la venta.";

                MessageBox.Show(
                    "No se pudo completar la operación:\n" +
                    ex.Message +
                    "\n\nSi la conexión se interrumpió, " +
                    "verifique el historial antes de intentarlo " +
                    "nuevamente.",
                    "Error al registrar venta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                registrandoVenta = false;
                CambiarEstadoControles(true);
            }
        }
    }
}
