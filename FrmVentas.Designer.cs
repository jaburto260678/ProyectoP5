
namespace ProyectoP5
{
    partial class FrmVentas
    {
        /// <summary>
        /// Contenedor de componentes del formulario.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        /// <summary>
        /// Libera los recursos utilizados.
        /// </summary>
        /// <param name="disposing">
        /// Indica si se deben liberar los recursos administrados.
        /// </param>
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
        /// Los controles visuales se crean en CrearInterfaz().
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // FrmVentas
            // 
            ClientSize = new Size(459, 459);
            Name = "FrmVentas";
            Text = "Registro de Ventas";
            ResumeLayout(false);
        }
    }
}
