
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
    /// Formulario para registrar ventas de productos.
    /// </summary>
    public partial class FrmVentas : Form
    {
        private readonly VentaServicio servicio;
        private readonly List<DetalleVenta> carrito;

        private ComboBox cboClientes = null!;
        private ComboBox cboProductos = null!;
        private NumericUpDown nudCantidad = null!;
        private DataGridView dgvDetalles = null!;

        private Label lblStock = null!;
        private Label lblPrecio = null!;
        private Label lblTotal = null!;

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
        /// Construye los controles de la pantalla.
        /// </summary>
        private void CrearInterfaz()
        {
            Text = "ProyectoP5 - Módulo de Ventas";

            StartPosition = FormStartPosition.CenterScreen;

            ClientSize = new Size(1000, 720);
            MinimumSize = new Size(900, 700);

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

            // Fecha de la operación.
            Label lblFecha = new Label
            {
                Text = "Fecha: " +
                       DateTime.Now.ToString("dd/MM/yyyy"),
                Location = new Point(760, 35),
                Size = new Size(210, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Selección de cliente.
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

            // Recargar información.
            btnActualizar = new Button
            {
                Text = "Actualizar datos",
                Location = new Point(770, 120),
                Size = new Size(190, 40),
                BackColor = Color.FromArgb(70, 85, 105),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnActualizar.Click += (s, e) => CargarDatos();

            // Selección de producto.
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

            // Botón agregar.
            btnAgregar = new Button
            {
                Text = "Agregar producto",
                Location = new Point(745, 220),
                Size = new Size(215, 40),
                BackColor = Color.FromArgb(30, 110, 205),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnAgregar.Click += (s, e) => AgregarProducto();

            // Precio del producto seleccionado.
            lblPrecio = new Label
            {
                Text = "Precio: ₡0.00",
                Location = new Point(30, 275),
                Size = new Size(250, 30),
                ForeColor = Color.FromArgb(50, 70, 90)
            };

            // Stock disponible.
            lblStock = new Label
            {
                Text = "Stock disponible: 0",
                Location = new Point(310, 275),
                Size = new Size(260, 30),
                ForeColor = Color.FromArgb(50, 70, 90)
            };

            // Título de la tabla.
            Label lblDetalle = new Label
            {
                Text = "PRODUCTOS DE LA VENTA",
                Location = new Point(30, 325),
                Size = new Size(450, 30),
                Font = new Font(
                    "Segoe UI", 12, FontStyle.Bold)
            };

            // Tabla del carrito.
            dgvDetalles = new DataGridView
            {
                Location = new Point(30, 365),
                Size = new Size(930, 205),
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

            // Eliminar producto.
            btnEliminar = new Button
            {
                Text = "Quitar seleccionado",
                Location = new Point(30, 590),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(175, 65, 65),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnEliminar.Click += (s, e) => QuitarProducto();

            // Limpiar venta.
            btnLimpiar = new Button
            {
                Text = "Limpiar venta",
                Location = new Point(245, 590),
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(110, 120, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnLimpiar.Click += (s, e) => LimpiarVenta();

            // Total.
            lblTotal = new Label
            {
                Text = "TOTAL: ₡0.00",
                Location = new Point(605, 590),
                Size = new Size(355, 45),
                Font = new Font(
                    "Segoe UI", 17, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                ForeColor = Color.FromArgb(25, 90, 55),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Guardar venta.
            btnRegistrar = new Button
            {
                Text = "REGISTRAR VENTA",
                Location = new Point(700, 650),
                Size = new Size(260, 45),
                BackColor = Color.FromArgb(30, 135, 75),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI", 11, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnRegistrar.Click += (s, e) => RegistrarVenta();

            // Agregar los controles al formulario.
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
                btnRegistrar
            });

            ActualizarTabla();
        }

        /// <summary>
        /// Carga clientes y productos desde MySQL.
        /// </summary>
        private void CargarDatos()
        {
            try
            {
                int clienteAnterior =
                    (cboClientes.SelectedItem
                     as VentaClienteOpcion)?.Id ?? 0;

                int productoAnterior =
                    (cboProductos.SelectedItem
                     as VentaProductoOpcion)?.Id ?? 0;

                var clientes = servicio.ObtenerClientes();
                var productos = servicio.ObtenerProductos();

                cboClientes.DataSource = null;
                cboProductos.DataSource = null;

                cboClientes.DataSource = clientes;
                cboProductos.DataSource = productos;

                var cliente = clientes.FirstOrDefault(
                    c => c.Id == clienteAnterior);

                if (cliente != null)
                    cboClientes.SelectedItem = cliente;

                var producto = productos.FirstOrDefault(
                    p => p.Id == productoAnterior);

                if (producto != null)
                    cboProductos.SelectedItem = producto;

                ActualizarInformacionProducto();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los datos:\n" +
                    ex.Message,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Actualiza precio y existencias mostradas.
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
            if (cboProductos.SelectedItem
                is not VentaProductoOpcion producto)
            {
                MessageBox.Show(
                    "Seleccione un producto.");
                return;
            }

            int cantidad = (int)nudCantidad.Value;

            var existente = carrito.FirstOrDefault(
                p => p.ProductoId == producto.Id);

            int agregado = existente?.Cantidad ?? 0;

            if (cantidad > producto.Stock - agregado)
            {
                MessageBox.Show(
                    "No hay suficientes unidades disponibles.",
                    "Stock insuficiente",
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
        }

        /// <summary>
        /// Muestra el carrito y calcula el total.
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
                dgvDetalles.Columns["ProductoId"]!.Visible = false;
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
            if (dgvDetalles.CurrentRow == null)
                return;

            object? valor =
                dgvDetalles.CurrentRow.Cells["ProductoId"].Value;

            if (valor == null)
                return;

            int productoId = Convert.ToInt32(valor);

            var detalle = carrito.FirstOrDefault(
                p => p.ProductoId == productoId);

            if (detalle == null)
                return;

            carrito.Remove(detalle);

            ActualizarTabla();
        }

        /// <summary>
        /// Limpia los artículos agregados.
        /// </summary>
        private void LimpiarVenta()
        {
            carrito.Clear();

            ActualizarTabla();
        }

        /// <summary>
        /// Guarda la venta mediante la capa Negocio.
        /// </summary>
        private void RegistrarVenta()
        {
            if (cboClientes.SelectedItem
                is not VentaClienteOpcion cliente)
            {
                MessageBox.Show(
                    "Debe seleccionar un cliente.");
                return;
            }

            if (carrito.Count == 0)
            {
                MessageBox.Show(
                    "Agregue al menos un producto.");
                return;
            }

            decimal total = carrito.Sum(p => p.Subtotal);

            DialogResult respuesta = MessageBox.Show(
                $"¿Desea registrar la venta por ₡{total:N2}?\n" +
                "El importe definitivo se calculará con " +
                "los precios actuales de MySQL.",
                "Confirmar venta",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            btnRegistrar.Enabled = false;

            try
            {
                var venta = new VentaRegistro
                {
                    ClienteId = cliente.Id,
                    Fecha = DateTime.Now,
                    Detalles = carrito.Select(p => new DetalleVenta
                    {
                        ProductoId = p.ProductoId,
                        NombreProducto = p.NombreProducto,
                        Cantidad = p.Cantidad,
                        PrecioUnitario = p.PrecioUnitario
                    }).ToList()
                };

                int ventaId = servicio.RegistrarVenta(venta);

                MessageBox.Show(
                    $"La venta #{ventaId} se registró correctamente.",
                    "Venta registrada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LimpiarVenta();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo registrar la venta:\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnRegistrar.Enabled = true;
            }
        }
    }
}
