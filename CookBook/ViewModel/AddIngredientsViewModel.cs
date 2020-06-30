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
    public class AddIngredientsViewModel: INotifyPropertyChanged
    {
        public ICommand AddIngredientToDBCommand { get; set; }
        private IDbActions dbActions;
        private ObservableCollection<Ingredient> _ingredientItems;
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

        public AddIngredientsViewModel(DbActions dbActions, ObservableCollection<Ingredient> ingredientItems)
        {
            this.dbActions = dbActions;

            _ingredientItems = ingredientItems;

            AddIngredientToDBCommand = new RelayCommand(AddIngredient);
        }


        public void AddIngredient(object obj)
        {
            //Console.WriteLine("click");
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                if (MessageBox.Show("Enter ingredient name", "Invalid Ingredient name", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    name = "";
                }
            }
            else if (_ingredientItems.Any(c => c.name.Trim().ToLower().Equals(name.Trim().ToLower())))
            {
                if (MessageBox.Show("Ingredient already exists", "Invalid ingredient name", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    name = "";
                }
                return;
            }
            else
            {
                if (this.dbActions.AddIngredient(new CookBookData.Model.Ingredient { name = name }))
                {
                    Console.WriteLine("Ingredient added");

                    if (MessageBox.Show("Ingredient added", "Ingredient added", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK) == MessageBoxResult.OK)
                    {
                        // We added a new ingredient, but we dont know the id so
                        // to update the view, we must read the recently created 
                        // ingredient and add it to the listview source

                        var readIngredient = this.dbActions.ReadIngredient(new CookBookData.Model.Ingredient { name = name });

                        // update collection in the view model
                        var ingredientItem = new CookBookData.Model.Ingredient
                        {
                            Id = ((CookBookData.Model.Ingredient)readIngredient).Id,
                            name = ((CookBookData.Model.Ingredient)readIngredient).name,
                        };

                        this._ingredientItems.Add(ingredientItem);

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
