using mdiazS6.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace mdiazS6.Views;

public partial class vEstudiante : ContentPage
{
	private const string URL = "http://172.26.64.1/ws_estudiante/ws.php"; //ws
	private readonly HttpClient client = new HttpClient();
	private ObservableCollection<Estudiante> _estud;

	public async void get()
	{
		var content = await client.GetStringAsync(URL);
		List<Estudiante> objEstudiante = JsonConvert.DeserializeObject<List<Estudiante>>(content);

        _estud= new ObservableCollection<Estudiante>(objEstudiante);
        ListaEstudiantes.ItemsSource = _estud;

    }


    public vEstudiante()
	{
		InitializeComponent();
		get();
	}
}