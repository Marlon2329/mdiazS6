using System.Net;

namespace mdiazS6.Views;

public partial class vAgregarEstudiante : ContentPage
{
    public vAgregarEstudiante()
    {
        InitializeComponent();
    }

    public void btnAgregar_Clicked(object sender, EventArgs e)
    {
        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("nombre", txtNombre.Text);
            parametros.Add("apellido", txtApellido.Text);
            parametros.Add("edad", txtEdad.Text);

            cliente.UploadValues("http://172.24.80.1/ws_estudiante/ws.php", "POST", parametros);
            DisplayActionSheet("Alerta", "Estudiante Ingresado", "Cerrar");
            Navigation.PushAsync(new vEstudiante());
        }
        catch (Exception ex)
        {
            DisplayActionSheet("ERROR", ex.Message, "CERRAR");
        }
    }
}