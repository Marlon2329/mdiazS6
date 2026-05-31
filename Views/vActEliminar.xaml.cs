
using mdiazS6.Models;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace mdiazS6.Views;

public partial class vActEliminar : ContentPage
{
    private const string URL = "http://172.24.80.1/ws_estudiante/ws.php";
    public vActEliminar(Estudiante datos)
    {
        InitializeComponent();
        txtCodigo.Text = datos.codigo.ToString();
        txtNombre.Text = datos.nombre.ToString();
        txtApellido.Text = datos.apellido.ToString();
        txtEdad.Text = datos.edad.ToString();
    }

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtEdad.Text))
            {
                await DisplayAlert("ERROR", "Completa todos los campos", "OK");
                return;
            }

            if (!int.TryParse(txtEdad.Text, out _))
            {
                await DisplayAlert("ERROR", "Edad inválida", "OK");
                return;
            }

            using (var cliente = new WebClient())
            {
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("accion", "actualizar");
                parametros.Add("codigo", txtCodigo.Text.Trim());
                parametros.Add("nombre", txtNombre.Text.Trim());
                parametros.Add("apellido", txtApellido.Text.Trim());
                parametros.Add("edad", txtEdad.Text.Trim());

                var responseBytes = await cliente.UploadValuesTaskAsync(URL, "POST", parametros);
                var response = Encoding.UTF8.GetString(responseBytes);

                string mensaje = response;
                try
                {
                    var j = JToken.Parse(response);
                    if (j.Type == JTokenType.Object && j["mensaje"] != null)
                        mensaje = j["mensaje"].ToString();
                }
                catch { }

                if (response.ToLower().Contains("error") || response.ToLower().Contains("fatal"))
                {
                    await DisplayAlert("ERROR WS", mensaje, "OK");
                    return;
                }

                await DisplayAlert("ÉXITO", mensaje, "OK");
                await Navigation.PopAsync();  // ← Vuelve a la lista
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", ex.Message, "CERRAR");
        }
    }

    private async void btnEliminar_Clicked(object sender, EventArgs e)
    {
        try
        {
            
            bool confirmar = await DisplayAlert("CONFIRMAR",
                $"¿Eliminar al estudiante {txtNombre.Text} {txtApellido.Text}?",
                "Sí", "No");

            if (!confirmar) return;

            using (var cliente = new WebClient())
            {
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("accion", "eliminar");
                parametros.Add("codigo", txtCodigo.Text);

                var responseBytes = await cliente.UploadValuesTaskAsync(URL, "POST", parametros);
                var response = Encoding.UTF8.GetString(responseBytes);

                string mensaje = response;
                try
                {
                    var j = JToken.Parse(response);
                    if (j.Type == JTokenType.Object && j["mensaje"] != null)
                        mensaje = j["mensaje"].ToString();
                }
                catch { }

                await DisplayAlert("Respuesta", mensaje, "OK");

                if (response.ToLower().Contains("ok") || mensaje.ToLower().Contains("elim") || mensaje.ToLower().Contains("correct"))
                {
                    await Navigation.PopAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", ex.Message, "CERRAR");
        }
    }
}