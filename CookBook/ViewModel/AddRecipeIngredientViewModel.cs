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
using System.Windows.Input;

namespace CookBook.ViewModel
{
    public class AddRecipeIngredientViewModel: INotifyPropertyChanged
    {
        public ICommand AddRecipeIngredientToDBCommand { get; set; }
        private DbActions dbActions;

        // Ingredients Combo Box
        private ObservableCollection<Ingredient> _allIngredientItems { get; set; }
        public ObservableCollection<Ingredient> allIngredientItems
        {
            get { return _allIngredientItems; }
            set
            {
                if (_allIngredientItems != value)
                {
                    _allIngredientItems = value;
                    OnPropertyChanged();
                }
            }
        }

        // Measures combo box
        private ObservableCollection<Measure> _allMeasureItems { get; set; }
        public ObservableCollection<Measure> allMeasureItems
        {
            get { return _allMeasureItems; }
            set
            {
                if (_allMeasureItems != value)
                {
                    _allMeasureItems = value;
                    OnPropertyChanged();
                }
            }
        }


        public AddRecipeIngredientViewModel(DbActions dbActions, ObservableCollection<CookBookData.Model.Ingredient> ingredientItems, ObservableCollection<CookBookData.Model.Measure> measureItems, int recipeId)
        {
            this.dbActions = dbActions;
            _allIngredientItems = ingredientItems;
            _allMeasureItems = measureItems;
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