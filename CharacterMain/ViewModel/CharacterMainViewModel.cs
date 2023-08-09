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
        private int _characterIndex;

        public List<string> CharacterList 
        {
            get => _characterList;
            
            set
            {
                SetProperty(ref _characterList, value, "CharacterList");
                RaisePropertyChanged("CharacterList");
            }
        }

        public int SelectedCharacterIndex { 
            get => _characterIndex; 
            set
            {
                SetProperty(ref _characterIndex, value, nameof(SelectedCharacterIndex));
                RaisePropertyChanged(nameof(SelectedCharacterIndex));
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
            _characterIndex = -1;
        }

        public async Task LoadCharacterList()
        {
            EnableButtons = false;

            var indexAfterLoading = _characterIndex != -1 ? _characterIndex : -1;

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
                SelectedCharacterIndex = CharacterList.Count >= indexAfterLoading ? indexAfterLoading : -1;
            }

            EnableButtons = true;
        }
    }
}
