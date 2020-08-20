using FormularioAPI.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FormularioAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RequestToAPI();
        }

        private void Adicionar_onClick(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtNome.Text) &&
               !string.IsNullOrWhiteSpace(txtSobrenome.Text) &&
               !string.IsNullOrWhiteSpace(txtTelefone.Text))
            {
                try
                {                    

                    var body = "{\"nome\": \""+txtNome.Text+"\",\"sobrenome\": \""+ txtSobrenome.Text + "\",\"telefone\": \""+txtTelefone.Text+"\"}";
                    var client = new RestClient("https://apiwebappformulario.azurewebsites.net/api/APIFormularios");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Cookie", "ARRAffinity=4300f504668eb50daedc35f96cd0392e3a685710075b8621969d65e4f5dc63bf");
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    MessageBox.Show("Cadastro concluído com sucesso");
                    RequestToAPI();
                    txtNome.Text = string.Empty;
                    txtSobrenome.Text = string.Empty;
                    txtTelefone.Text = string.Empty;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Um erro ocorreu: \n" + ex.StackTrace);
                }
            }
        }
        private void Excluir_onClick(object sender, RoutedEventArgs e)
        {
            var formulario = (Formulario)dataGridClientes.CurrentCell.Item;

            var id = formulario.id;

            var client = new RestClient("https://apiwebappformulario.azurewebsites.net/api/APIFormularios/" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Cookie", "ARRAffinity=4300f504668eb50daedc35f96cd0392e3a685710075b8621969d65e4f5dc63bf");
            IRestResponse response = client.Execute(request);
            RequestToAPI();
        }
        private void Atualizar_onClick(object sender, RoutedEventArgs e)
        {
            var formulario = (Formulario)dataGridClientes.CurrentCell.Item;
            AtualizaFormulario att = new AtualizaFormulario(formulario);
            att.ShowDialog();
            RequestToAPI();
        }

        private void RequestToAPI()
        {
            var client = new RestClient("https://apiwebappformulario.azurewebsites.net/api/APIFormularios");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "ARRAffinity=4300f504668eb50daedc35f96cd0392e3a685710075b8621969d65e4f5dc63bf");
            IRestResponse response = client.Execute(request);
            var value = response.Content;

            //Json Deserialize            
            var formularios = JsonConvert.DeserializeObject<List<Formulario>>(value);
            AtualizaGrid(formularios);
        }

        private void AtualizaGrid(List<Formulario> formularios)
        {
            dataGridClientes.ItemsSource = formularios;
        }        
    }
}
