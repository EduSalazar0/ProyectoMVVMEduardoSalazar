using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProyectoMVVMEduardoSalazar.ViewModels
{
    internal class NoteViewModel: ObservableObject, IQueryAttributable
    {
        private Models.Note _note;

        public string Text
        {
            get => _note.Text;
            set
            {
                if (_note.Text != value)
                {
                    _note.Text = value;
                    OnPropertyChanged();
                }
            }
        }
        //Recuperan valores correspondientes del modelo.
        public DateTime Date => _note.Date;

        public string Identifier => _note.Filename;
        //Comandos
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        //Los dos constructores se usan para crear el modelo de vista con un nuevo modelo de respaldp
        // que es una nota vacia, o para crear un modelo de vista que usa la instancia de modelo especificada
        public NoteViewModel()
        {
            _note = new Models.Note();
            SaveCommand = new AsyncRelayCommand(Save);
            DeleteCommand = new AsyncRelayCommand(Delete);
        }
        public NoteViewModel(Models.Note note)
        {
            _note = note;
            SaveCommand = new AsyncRelayCommand(Save);
            DeleteCommand = new AsyncRelayCommand(Delete);
        }

        private async Task Save()
        {
            _note.Date = DateTime.Now;
            _note.Save();
            await Shell.Current.GoToAsync($"..?saved={_note.Filename}");
        }
        private async Task Delete()
        {
            _note.Delete();
            await Shell.Current.GoToAsync($"..?deleted={_note.Filename}");
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("load"))
            {
                _note = Models.Note.Load(query["load"].ToString());
                RefreshProperties();
            }
        }
        //Metodo auxiliar que actualiza el objeto del modelo de respaldo y lo vuelve a cargar desde el almacenamiento del dispositivo
        public void Reload()
        {
            _note = Models.Note.Load(_note.Filename);
            RefreshProperties();
        }
        //Es otro auxiliar para asegurar que los suscriptores enlazados a este objeto reciben una notifiación de que las propiedades han cambiado
        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(Date));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
