
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
    /// Formulario para consultar el historial de ventas
    /// y los productos vendidos.
    /// </summary>
    public partial class FrmHistorialVentas : Form
    {
        private readonly VentaHistorialServicio servicio;

        private DateTimePicker dtpInicio = null!;
        private DateTimePicker dtpFin = null!;

        private DataGridView dgvVentas = null!;
        private DataGridView dgvDetalle = null!;

        private Label lblCantidad = null!;
        private Label lblTotalVentas = null!;
        private Label lblVentaSeleccionada = null!;
        private Label lblTotalDetalle = null!;

        private Button btnBuscar = null!;

        public FrmHistorialVentas()
        {
            InitializeComponent();

            servicio = new VentaHistorialServicio();

            CrearInterfaz();

            Shown += (s, e) => CargarHistorial();
        }

        /// <summary>
        /// Construye los controles visuales.
        /// </summary>
        private void CrearInterfaz()
        {
            Text = "ProyectoP5 - Historial de Ventas";

            StartPosition = FormStartPosition.CenterScreen;

            ClientSize = new Size(1050, 760);
            MinimumSize = new Size(950, 740);

            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);

            var lblTitulo = new Label
            {
                Text = "HISTORIAL DE VENTAS",
                Font = new Font(
                    "Segoe UI", 21, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 85),
                Location = new Point(30, 20),
                Size = new Size(680, 45)
            };

            var lblSubtitulo = new Label
            {
                Text = "Consulta de ventas registradas en MySQL",
                Location = new Point(30, 66),
                Size = new Size(650, 28),
                ForeColor = Color.DimGray
            };

            // Filtro de fecha inicial.
            var lblDesde = new Label
            {
                Text = "Fecha desde:",
                Location = new Point(30, 115),
                AutoSize = true
            };

            dtpInicio = new DateTimePicker
            {
                Location = new Point(30, 145),
                Size = new Size(240, 32),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddMonths(-1)
            };

            // Filtro de fecha final.
            var lblHasta = new Label
            {
                Text = "Fecha hasta:",
                Location = new Point(300, 115),
                AutoSize = true
            };

            dtpFin = new DateTimePicker
            {
                Location = new Point(300, 145),
                Size = new Size(240, 32),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };

            // Botón buscar.
            btnBuscar = new Button
            {
                Text = "BUSCAR VENTAS",
                Location = new Point(575, 140),
                Size = new Size(200, 42),
                BackColor = Color.FromArgb(30, 110, 205),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnBuscar.Click += (s, e) => CargarHistorial();

            // Título de resultados.
            var lblListado = new Label
            {
                Text = "VENTAS REGISTRADAS",
                Location = new Point(30, 215),
                Size = new Size(450, 30),
                Font = new Font(
                    "Segoe UI", 12, FontStyle.Bold)
            };

            // Tabla del historial.
            dgvVentas = new DataGridView
            {
                Location = new Point(30, 255),
                Size = new Size(980, 180),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Left |
                         AnchorStyles.Right
            };

            dgvVentas.SelectionChanged +=
                (s, e) => CargarDetalleSeleccionado();

            lblCantidad = new Label
            {
                Text = "Ventas encontradas: 0",
                Location = new Point(30, 450),
                Size = new Size(300, 30)
            };

            lblTotalVentas = new Label
            {
                Text = "Total del período: ₡0.00",
                Location = new Point(640, 450),
                Size = new Size(370, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font(
                    "Segoe UI", 11, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Título del detalle.
            lblVentaSeleccionada = new Label
            {
                Text = "DETALLE DE VENTA",
                Location = new Point(30, 495),
                Size = new Size(700, 30),
                Font = new Font(
                    "Segoe UI", 12, FontStyle.Bold)
            };

            // Tabla de productos vendidos.
            dgvDetalle = new DataGridView
            {
                Location = new Point(30, 535),
                Size = new Size(980, 150),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Left |
                         AnchorStyles.Right
            };

            lblTotalDetalle = new Label
            {
                Text = "TOTAL: ₡0.00",
                Location = new Point(640, 700),
                Size = new Size(370, 35),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font(
                    "Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 90, 55),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblSubtitulo,
                lblDesde,
                dtpInicio,
                lblHasta,
                dtpFin,
                btnBuscar,
                lblListado,
                dgvVentas,
                lblCantidad,
                lblTotalVentas,
                lblVentaSeleccionada,
                dgvDetalle,
                lblTotalDetalle
            });
        }

        /// <summary>
        /// Busca ventas dentro del período indicado.
        /// </summary>
        private void CargarHistorial()
        {
            try
            {
                btnBuscar.Enabled = false;

                var ventas = servicio.ConsultarVentas(
                    dtpInicio.Value,
                    dtpFin.Value);

                // Evita ejecutar una consulta de detalle
                // mientras se reemplazan las filas.
                dgvVentas.SelectionChanged -=
                    DgvVentas_SelectionChanged;

                dgvVentas.DataSource = null;

                dgvDetalle.DataSource = null;

                dgvVentas.DataSource = ventas;

                ConfigurarTablaVentas();

                lblCantidad.Text =
                    $"Ventas encontradas: {ventas.Count}";

                lblTotalVentas.Text =
                    $"Total del período: ₡{ventas.Sum(v => v.Total):N2}";

                if (ventas.Count > 0)
                {
                    dgvVentas.Rows[0].Selected = true;
                    dgvVentas.CurrentCell =
                        dgvVentas.Rows[0]
                        .Cells["Id"];

                    CargarDetalleSeleccionado();
                }
                else
                {
                    lblVentaSeleccionada.Text =
                        "DETALLE DE VENTA";

                    lblTotalDetalle.Text = "TOTAL: ₡0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo consultar el historial:\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                dgvVentas.SelectionChanged -=
                    DgvVentas_SelectionChanged;

                dgvVentas.SelectionChanged +=
                    DgvVentas_SelectionChanged;

                btnBuscar.Enabled = true;
            }
        }

        /// <summary>
        /// Evento de selección de una venta.
        /// </summary>
        private void DgvVentas_SelectionChanged(
            object? sender,
            EventArgs e)
        {
            CargarDetalleSeleccionado();
        }

        /// <summary>
        /// Configura las columnas del historial.
        /// </summary>
        private void ConfigurarTablaVentas()
        {
            if (dgvVentas.Columns["Id"] != null)
                dgvVentas.Columns["Id"]!.HeaderText =
                    "N.º Venta";

            if (dgvVentas.Columns["Fecha"] != null)
            {
                dgvVentas.Columns["Fecha"]!.HeaderText =
                    "Fecha";

                dgvVentas.Columns["Fecha"]!
                    .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }

            if (dgvVentas.Columns["ClienteId"] != null)
                dgvVentas.Columns["ClienteId"]!.Visible =
                    false;

            if (dgvVentas.Columns["Cliente"] != null)
                dgvVentas.Columns["Cliente"]!.HeaderText =
                    "Cliente";

            if (dgvVentas.Columns["Total"] != null)
            {
                dgvVentas.Columns["Total"]!.HeaderText =
                    "Total (₡)";

                dgvVentas.Columns["Total"]!
                    .DefaultCellStyle.Format = "N2";
            }
        }

        /// <summary>
        /// Muestra los productos de la venta seleccionada.
        /// </summary>
        private void CargarDetalleSeleccionado()
        {
            if (dgvVentas.CurrentRow?.DataBoundItem
                is not VentaHistorialItem venta)
            {
                dgvDetalle.DataSource = null;

                lblVentaSeleccionada.Text =
                    "DETALLE DE VENTA";

                lblTotalDetalle.Text =
                    "TOTAL: ₡0.00";

                return;
            }

            try
            {
                var detalles =
                    servicio.ConsultarDetalle(venta.Id);

                dgvDetalle.DataSource = null;
                dgvDetalle.DataSource = detalles;

                ConfigurarTablaDetalle();

                lblVentaSeleccionada.Text =
                    $"DETALLE DE VENTA #{venta.Id} - " +
                    venta.Cliente;

                lblTotalDetalle.Text =
                    $"TOTAL: ₡{venta.Total:N2}";
            }
            catch (Exception ex)
            {
                dgvDetalle.DataSource = null;

                lblVentaSeleccionada.Text =
                    "DETALLE DE VENTA";

                lblTotalDetalle.Text =
                    "TOTAL: ₡0.00";

                MessageBox.Show(
                    "No se pudo consultar el detalle:\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configura las columnas de productos vendidos.
        /// </summary>
        private void ConfigurarTablaDetalle()
        {
            string[] columnasOcultas =
            {
                "Id", "VentaId", "ProductoId"
            };

            foreach (string nombre in columnasOcultas)
            {
                if (dgvDetalle.Columns[nombre] != null)
                    dgvDetalle.Columns[nombre]!.Visible =
                        false;
            }

            if (dgvDetalle.Columns["Codigo"] != null)
                dgvDetalle.Columns["Codigo"]!.HeaderText =
                    "Código";

            if (dgvDetalle.Columns["Producto"] != null)
                dgvDetalle.Columns["Producto"]!.HeaderText =
                    "Producto";

            if (dgvDetalle.Columns["Cantidad"] != null)
                dgvDetalle.Columns["Cantidad"]!.HeaderText =
                    "Cantidad";

            if (dgvDetalle.Columns["PrecioUnitario"] != null)
            {
                dgvDetalle.Columns["PrecioUnitario"]!
                    .HeaderText = "Precio Unitario (₡)";

                dgvDetalle.Columns["PrecioUnitario"]!
                    .DefaultCellStyle.Format = "N2";
            }

            if (dgvDetalle.Columns["Subtotal"] != null)
            {
                dgvDetalle.Columns["Subtotal"]!
                    .HeaderText = "Subtotal (₡)";

                dgvDetalle.Columns["Subtotal"]!
                    .DefaultCellStyle.Format = "N2";
            }
        }
    }
}
