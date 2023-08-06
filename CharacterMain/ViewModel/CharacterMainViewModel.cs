using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using MakCraft.ViewModels;

namespace CharacterMain.ViewModel
{
    public class CharacterMainViewModel : ViewModelBase
    {
        private List<string> _characterList;
        private bool _enableButtons;

        public List<string> CharacterList 
        {
            get => _characterList;
            
            set
            {
                SetProperty(ref _characterList, value, "CharacterList");
                RaisePropertyChanged("CharacterList");
            }
        }

        public bool EnableButtons
        {
            get => _enableButtons;

            set
            {
                SetProperty(ref _enableButtons, value, "EnableButtons");
                RaisePropertyChanged("EnableButtons");
            }
        }

        public CharacterMainViewModel()
        {
            _enableButtons = false;
        }

        public async Task LoadCharacterList()
        {
            EnableButtons = false;

            _characterList = new List<string>
            {
                "Loading..."
            };
            CharacterList = _characterList;

            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:8000/getCharacterIds");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(responseBody))
            {
                CharacterList = JsonSerializer.Deserialize<List<string>>(responseBody);
            }

            EnableButtons = true;
        }
    }
}
