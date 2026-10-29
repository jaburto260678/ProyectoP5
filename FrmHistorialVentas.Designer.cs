
namespace ProyectoP5
{
    partial class FrmHistorialVentas
    {
        /// <summary>
        /// Contenedor de los componentes del formulario.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        /// <summary>
        /// Libera los recursos utilizados por el formulario.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Inicializa los componentes del formulario.
        /// Los controles se crean en CrearInterfaz().
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();

            Name = "FrmHistorialVentas";
            Text = "Historial de Ventas";

            ResumeLayout(false);
        }
    }
}
