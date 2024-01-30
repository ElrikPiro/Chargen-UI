using CharacterMain.ViewModel;
using Domain.Character.Interfaces;
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

        CharacterMainViewModel viewModel_;
        
        public CharacterMainView(ICharacterDataProvider characterDataProvider)
        {
            InitializeComponent();
            viewModel_ = new CharacterMainViewModel(characterDataProvider);
            DataContext = viewModel_;
        }

        async private void CharacterList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await viewModel_.LoadCharacterList();
            }
            catch(Exception ex)
            {
                viewModel_.CharacterList = new List<string> { "Error!" };
                MessageBox.Show(ex.Message);
            }
        }
    }
}
