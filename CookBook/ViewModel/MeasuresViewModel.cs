using CookBook.View;
using CookBook.ViewModel.Interfaces;
using CookBookData.Model;
using CookBookData.Model.DbActions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookBook.ViewModel
{
    class MeasuresViewModel: INotifyPropertyChanged
    {
        private DbActions dbActions;

        public ObservableCollection<Measure> measureItems { get; set; }

        private Measure _selectedMeasure;

        public Measure selectedMeasure
        {
            get
            {
                return _selectedMeasure;
            }
            set
            {
                _selectedMeasure = value;
                OnPropertyChanged();
            }
        }

        public MeasuresViewModel()
        {
            dbActions = new DbActions();

            var allMeasures = this.dbActions.BrowseMeasures();

            measureItems = new ObservableCollection<Measure>(
                allMeasures.Select(obj =>
                {
                    var measure = (CookBookData.Model.Measure)obj;
                    return new Measure { Id = measure.Id, name = measure.name };
                })
                );
            //Console.WriteLine(measureItems[0].name);

            DeleteMeasureCommand = new RelayCommand(DeleteMeasure);
            EditMeasureCommand = new RelayCommand(EditMeasure);
            AddMeasureCommand = new RelayCommand(OpenAddMeasureWindow);

        }

        private void OpenAddMeasureWindow(object obj)
        {
            var AddMeasureVM = new AddMeasureViewModel(dbActions, measureItems);
            var AddMeasureV = new AddMeasureView(AddMeasureVM);

            AddMeasureV.Show();
        }

        private void DeleteMeasure(Object measure)
        {
            dbActions = new DbActions();

            if (selectedMeasure != null)
            {
                if (MessageBox.Show("Do you want to delete the selected measure?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    if (dbActions.DeleteMeasure(new CookBookData.Model.Measure { Id = selectedMeasure.Id }))
                    {
                        measureItems.Remove(selectedMeasure);
                    }
                }
            }
        }

        private void EditMeasure(object measure)
        {
            dbActions = new DbActions();

            if (selectedMeasure != null && !string.IsNullOrEmpty(selectedMeasure.name) && !string.IsNullOrWhiteSpace(selectedMeasure.name))
            {
                if (dbActions.EditMeasure(new CookBookData.Model.Measure { Id = selectedMeasure.Id, name = selectedMeasure.name }))
                {
                    MessageBox.Show("Measure updated", "Measure updated", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }
        }



        public ICommand DeleteMeasureCommand { get; set; }
        public ICommand EditMeasureCommand { get; set; }
        public ICommand AddMeasureCommand { get; set; }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
