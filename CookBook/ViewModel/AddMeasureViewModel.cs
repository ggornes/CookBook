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
    public class AddMeasureViewModel: INotifyPropertyChanged
    {
        public ICommand AddMeasureToDBCommand { get; set; }
        private DbActions dbActions;
        private ObservableCollection<Measure> _measureItems;
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public AddMeasureViewModel(DbActions dbActions, ObservableCollection<Measure> measureItems)
        {
            this.dbActions = dbActions;

            _measureItems = measureItems;

            AddMeasureToDBCommand = new RelayCommand(AddMeasure);
        }

        public void AddMeasure(object obj)
        {
            //Console.WriteLine("click");
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                if (MessageBox.Show("Enter Measure name", "Invalid Measure name", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    name = "";
                }
            }
            else if (_measureItems.Any(c => c.name.Trim().ToLower().Equals(name.Trim().ToLower())))
            {
                if (MessageBox.Show("Measure already exists", "Invalid measure name", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    name = "";
                }
                return;
            }
            else
            {
                if (this.dbActions.AddMeasure(new CookBookData.Model.Measure { name = name }))
                {
                    Console.WriteLine("Measure added");

                    if (MessageBox.Show("Measure added", "Measure added", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK) == MessageBoxResult.OK)
                    {
                        // We added a new Measure, but we dont know the id so
                        // to update the view, we must read the recently created 
                        // Measure and add it to the listview source

                        var readMeasure = this.dbActions.ReadMeasure(new CookBookData.Model.Measure { name = name });

                        // update collection in the view model
                        var measureItem = new CookBookData.Model.Measure
                        {
                            Id = ((CookBookData.Model.Measure)readMeasure).Id,
                            name = ((CookBookData.Model.Measure)readMeasure).name,
                        };

                        this._measureItems.Add(measureItem);

                        name = "";
                    }
                }
            }

        }














        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
