using FormularioAPI.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FormularioAPI
{
    /// <summary>
    /// Interaction logic for AtualizaFormulario.xaml
    /// </summary>
    public partial class AtualizaFormulario : Window
    {
        public AtualizaFormulario(Formulario formulario)
        {
            InitializeComponent();
            PreencheCampos(formulario);
        }      
        
        private void PreencheCampos(Formulario formulario)
        {
            txtID.Text = formulario.id.ToString();
            txtID.IsEnabled = false;

            txtNome.Text = formulario.nome;
            txtSobrenome.Text = formulario.sobrenome;
            txtTelefone.Text = formulario.telefone;
        }

        private void Atualiza_onClick(object sender, RoutedEventArgs e)
        {
            var id = txtID.Text;
            var nome = txtNome.Text;
            var sobrenome = txtSobrenome.Text;
            var telefone = txtTelefone.Text;

            var body = "{\r\n    \"id\": " + id + ",\r\n    \"nome\": \"" + nome + "\",\r\n    \"sobrenome\": \"" + sobrenome + "\",\r\n    \"telefone\": \"" + telefone + "\"\r\n}";
            var client = new RestClient("https://apiwebappformulario.azurewebsites.net/api/APIFormularios/" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=4300f504668eb50daedc35f96cd0392e3a685710075b8621969d65e4f5dc63bf");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            MessageBox.Show("Formulário alterado com sucesso!");
        }
    }
}
