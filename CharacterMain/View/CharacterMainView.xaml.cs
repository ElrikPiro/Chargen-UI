using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

namespace CharacterMain
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CharacterMainView : UserControl
    {
        public CharacterMainView()
        {
            InitializeComponent();
        }

        async private Task<List<string>> getCharacterList() //TODO: esto al viewmodel
        {
            var retval = new List<string>();

            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:8000/getCharacterIds");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(responseBody))
            {
                // Convert the JSON response to a list of strings
                var responseList = JsonSerializer.Deserialize<List<string>>(responseBody);
                retval = responseList;
            }

            return retval;
        }

        async private void CharacterList_Loaded(object sender, RoutedEventArgs e)
        {
            await getCharacterList();
        }
    }
}
