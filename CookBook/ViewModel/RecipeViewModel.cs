using CookBookData.Model.DbActions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.ViewModel
{
    public class RecipeViewModel: INotifyPropertyChanged
    {
        private DbActions dbActions;

        public RecipeViewModel()
        {
            dbActions = new DbActions();

            //var allRecipes = this.dbActions.BrowseRecipes();
        }















        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
