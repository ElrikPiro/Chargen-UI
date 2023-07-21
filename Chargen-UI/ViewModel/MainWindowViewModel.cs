﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MakCraft.ViewModels;

namespace Chargen_UI.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Attributes

        ObservableCollection<string> _sectionNames;
        UserControl _selectedSection;
        Collection<UserControl> _sectionList;

        #endregion

        public MainWindowViewModel()
        {
            _sectionNames = new ObservableCollection<string> {
                "Character",
                "Culture"
            };

            _selectedSection = new UserControl();

            _sectionList = new Collection<UserControl>
            {
                new UserControl(),
                new UserControl()
            };

        }

        #region Properties
        public ObservableCollection<string> SectionListNames
        {
            get { return _sectionNames; }
            set { SetProperty(ref _sectionNames, value); }
        }

        public UserControl SelectedSection
        {
            get { return _selectedSection; }
            set { SetProperty(ref _selectedSection, value); }
        }

        #endregion

        #region Methods

        public void setSection(int index)
        {
            SelectedSection = _sectionList[index];
        }

        #endregion

    }
}
