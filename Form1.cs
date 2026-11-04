using ProyectoP5.Datos;
using ProyectoP5.Presentacion;

namespace ProyectoP5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Gestiondeusuarios form3 = new Gestiondeusuarios();
            form3.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ConexionBD.CrearConexion();
            MessageBox.Show("Conexión exitosa");
        }
    }
}
