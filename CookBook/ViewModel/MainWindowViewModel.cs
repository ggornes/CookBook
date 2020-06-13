using CookBook.ViewModel.Interfaces;
using CookBookData.Model;
using CookBookData.Model.DbActions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookBook.ViewModel
{
    class MainWindowViewModel : IViewModel, INotifyPropertyChanged
    {
        private IViewModel _currentViewModel;
        public IViewModel CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }
        private List<IViewModel> _viewModels;
        public List<IViewModel> ViewModels
        {
            get
            {
                if (_viewModels == null)
                {
                    _viewModels = new List<IViewModel>();
                }
                return _viewModels;
            }
        }

        private void ChangeViewModel(IViewModel viewModel)
        {
            if (!ViewModels.Contains(viewModel))
            {
                ViewModels.Add(viewModel);
            }
            CurrentViewModel = ViewModels.FirstOrDefault(vm => vm == viewModel);
        }


        public ICommand RecipesCommand { get; set; }
        public ICommand IngredientsCommand { get; set; }
        public ICommand MeasuresCommand { get; set; }
        private DbActions dbActions {get; set;}

        //public IAsyncCommand RecipesCommand { get; set; }


        public MainWindowViewModel(DbActions dbActions)
        {
            this.dbActions = dbActions;

            MeasuresCommand = new RelayCommand(OpenMeasures);
            IngredientsCommand = new RelayCommand(OpenIngredients);

            ViewModels.Add(new IngredientsViewModel());
            // add all



            CurrentViewModel = ViewModels[0]; // select one default (the first one)

            // Mediator.Suscribe();



        }

        public void OpenMeasures(object obj)
        {
            CurrentViewModel = new MeasuresViewModel();
        }

        public void OpenIngredients(object obj)
        {
            CurrentViewModel = new IngredientsViewModel();
        }








        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        //public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
