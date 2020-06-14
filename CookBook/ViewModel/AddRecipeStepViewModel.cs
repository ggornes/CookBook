using CookBook.UIModels;
using CookBook.ViewModel.Interfaces;
using CookBookData;
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
    public class AddRecipeStepViewModel: INotifyPropertyChanged
    {

        public ICommand AddRecipeStepToDBCommand { get; set; }
        private DbActions dbActions;
        private ObservableCollection<CookBookData.Model.RecipeStep> _recipeStepItems;
        private int _recipeId;
        private string _stepInstructions;
        private int _stepNumber;
        
        public string stepInstructions
        {
            get { return _stepInstructions; }
            set
            {
                _stepInstructions = value;
                OnPropertyChanged();
            }
        }

        public int stepNumber
        {
            get { return _stepNumber; }
            set
            {
                _stepNumber = value;
                OnPropertyChanged();
            }
        }

        public int recipeId
        {
            get { return _recipeId; }
            set
            {
                _recipeId = value;
                OnPropertyChanged();
            }
        }

        public AddRecipeStepViewModel(DbActions dbActions, ObservableCollection<CookBookData.Model.RecipeStep> recipeStepItems, int recipeId)
        {
            this.dbActions = dbActions;

            _recipeStepItems = recipeStepItems;

            _recipeId = recipeId;

            AddRecipeStepToDBCommand = new RelayCommand(AddRecipeStep);
        }

        public void AddRecipeStep(object obj)
        {
            if (string.IsNullOrEmpty(stepInstructions) || string.IsNullOrWhiteSpace(stepInstructions))
            {
                if (MessageBox.Show("Enter step instructions", "Invalid step instructions", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    stepInstructions = "";
                }
            }
            else if (_recipeStepItems.Any(c => c.stepInstructions.Trim().ToLower().Equals(stepInstructions.Trim().ToLower())))
            {
                if (MessageBox.Show("Step already exists", "Invalid step instructions", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.OK)
                {
                    stepInstructions = "";
                }
                return;
            }
            else
            {
                if (this.dbActions.AddRecipeStep(new CookBookData.Model.RecipeStep { recipeId = recipeId, stepNumber = stepNumber, stepInstructions = stepInstructions }))
                {
                    Console.WriteLine("Recipe Step added");

                    if (MessageBox.Show("Recipe step added", "Recipe Step added", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK) == MessageBoxResult.OK)
                    {
                        // We added a new Measure, but we dont know the id so
                        // to update the view, we must read the recently created 
                        // Measure and add it to the listview source

                        var readRecipeStep = this.dbActions.ReadRecipeStep(new CookBookData.Model.RecipeStep { stepInstructions = stepInstructions });

                        //// update collection in the view model
                        var recipeStepItem = new CookBookData.Model.RecipeStep
                        {
                            Id = ((CookBookData.Model.RecipeStep)readRecipeStep).Id,
                            stepNumber = ((CookBookData.Model.RecipeStep)readRecipeStep).stepNumber,
                            stepInstructions = ((CookBookData.Model.RecipeStep)readRecipeStep).stepInstructions
                        };

                        this._recipeStepItems.Add(recipeStepItem);

                        //stepNumber = "";
                        stepInstructions = "";
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
