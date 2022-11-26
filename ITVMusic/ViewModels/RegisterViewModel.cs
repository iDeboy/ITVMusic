using ITVMusic.Models;
using ITVMusic.Views;
using ITVMusic.Util;
using System;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ITVMusic.ViewModels {
    public class RegisterViewModel : ViewModelBase {

        // Fields

        private ImageSource? m_Icon;
        private string? m_LastNamePat;
        private string? m_LastNameMat;
        private string? m_Phone;
        private SecureString? m_Password;
        private DateOnly? m_Birthday;
        private string? m_NoControl;
        private string? m_Nickname;
        private string? m_Name;
        private string? m_Gender;
        private string? m_Email;
        private SuscriptionModel? m_Suscription;

        private string? m_ErrorMessage;
        private bool m_IsErrorMessageVisible;
        private bool m_IsRegisterSuccessful;

        public ObservableCollection<SuscriptionModel> Suscriptions { get; }
        public ImageSource? Icon {
            get => m_Icon;
            set {
                m_Icon = value;
                OnPropertyChanged();
            }
        }
        public string? LastNamePat {
            get => m_LastNamePat;
            set {
                m_LastNamePat = value;
                OnPropertyChanged();
            }
        }
        public string? LastNameMat {
            get => m_LastNameMat;
            set {
                m_LastNameMat = value;
                OnPropertyChanged();
            }
        }
        public string? Phone {
            get => m_Phone;
            set {
                m_Phone = value;
                OnPropertyChanged();
            }
        }
        public SecureString? Password {
            get => m_Password;
            set {
                m_Password = value;
                OnPropertyChanged();
            }
        }
        public DateOnly? Birthday {
            get => m_Birthday;
            set {
                m_Birthday = value;
                OnPropertyChanged();
            }
        }
        public string? NoControl {
            get => m_NoControl;
            set {
                m_NoControl = value;
                OnPropertyChanged();
            }
        }
        public string? Nickname {
            get => m_Nickname;
            set {
                m_Nickname = value;
                OnPropertyChanged();
            }
        }
        public string? Name {
            get => m_Name;
            set {
                m_Name = value;
                OnPropertyChanged();
            }
        }
        public string? Gender {
            get => m_Gender;
            set {
                m_Gender = value;
                OnPropertyChanged();
            }
        }
        public string? Email {
            get => m_Email;
            set {
                m_Email = value;
                OnPropertyChanged();
            }
        }
        public SuscriptionModel? Suscription {
            get => m_Suscription;
            set {
                m_Suscription = value;
                OnPropertyChanged();
            }
        }

        public string? ErrorMessage {
            get => m_ErrorMessage;
            set {
                m_ErrorMessage = value;
                OnPropertyChanged();
            }
        }
        public bool IsErrorMessageVisible {
            get => m_IsErrorMessageVisible;
            set {
                m_IsErrorMessageVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsRegisterSuccessful {
            get => m_IsRegisterSuccessful;
            set {
                m_IsRegisterSuccessful = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand CloseCommand { get; }
        public ICommand RegisterCommand { get; }

        public RegisterViewModel() {

            Suscriptions = new();

            FillSuscriptions();

            CloseCommand = new ViewModelCommand(ExecuteCloseCommand, CanExecuteCloseCommand);
            RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
        }

        private bool CanExecuteRegisterCommand(object? obj) {
            return !IsRegisterSuccessful;
        }

        private bool CanExecuteCloseCommand(object? obj) {
            return !IsRegisterSuccessful;
        }

        private void FillSuscriptions() {
            Suscriptions.Add(new() {
                Id = 1,
                PaymentMethod = "Deposito Oxxo",
                Type = "Anual"
            });

            Suscriptions.Add(new() {
                Id = 2,
                PaymentMethod = "Deposito Oxxo",
                Type = "Mensual"
            });

            Suscriptions.Add(new() {
                Id = 3,
                PaymentMethod = "Tarjeta",
                Type = "Anual"
            });

            Suscriptions.Add(new() {
                Id = 4,
                PaymentMethod = "Tarjeta",
                Type = "Mensual"
            });
        }

        private static bool ValidarIcon(ImageSource? imageSource, out string errors) {
            errors = "";

            if (imageSource is not BitmapImage image || image.ToByteArray().Length is 0) {
                errors = "Debe de elegir un icono válido.";
                return false;
            }

            if (image.UriSource is null) {
                errors = "No se pudo cargar la imagen.";
                return false;
            }

            return true;
        }
        private static bool ValidarFechaNacimiento(DateOnly? birthday, out string errors) {

            errors = "";

            if (!birthday.HasValue || birthday.Value.IsDefault()) {
                errors = "Debe ingresar su fecha de nacimiento.";
                return false;
            }

            if (birthday.Value.IsFuture()) {
                errors = "Fecha de nacimiento no válida.";
                return false;
            }

            return true;
        }
        private static bool ValidarNoControl(string? noControl, out string errors) {
            errors = "";

            // Validar si el campo está vacio
            if (string.IsNullOrWhiteSpace(noControl)) {
                errors = "Debe ingresar su No. Control.";
                return false;
            }

            if (noControl.Length > 9) {
                errors = "No. Control muy grande.";
                return false;
            }

            if (noControl.Length < 9) {
                errors = "No. Control muy pequeño.";
                return false;
            }

            // Si no es un número de control
            if (!Regex.IsMatch(noControl, @"[A-z]\d{8}")) {
                errors = "No. Control con formato incorrecto.";
                return false;
            }

            // Validar si no existe

            return true;
        }
        private static bool ValidarNickname(string? nickname, out string errors) {
            errors = "";

            // Validar si el campo está vacio
            if (string.IsNullOrWhiteSpace(nickname)) {
                errors = "Debe ingresar un nickname.";
                return false;
            }

            if (nickname.Length > 15) {
                errors = "Nickname muy largo.";
                return false;
            }

            // Validar si no existe

            return true;
        }
        private static bool ValidarNombre(string? nombre, out string errors) {

            errors = "";

            if (string.IsNullOrWhiteSpace(nombre)) {
                errors = "Debe ingresar su nombre.";
                return false;
            }

            if (Regex.IsMatch(nombre, @"\d")) {
                errors = "Nombre no válido.";
                return false;
            }

            if (nombre.Length < 3) {
                errors = "Nombre no válido.";
                return false;
            }

            return true;
        }
        private static bool ValidarApellidos(string? apellidoPat, string? apellidoMat, out string errors) {

            errors = "";

            if (string.IsNullOrWhiteSpace(apellidoPat)) {
                errors = "Debe ingresar su apellido paterno.";
                return false;
            }

            if (Regex.IsMatch(apellidoPat, @"\d")) {
                errors = "Apellido no válido.";
                return false;
            }

            if (apellidoPat.Length < 3) {
                errors = "Apellido no válido.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(apellidoMat)) {

                if (Regex.IsMatch(apellidoMat, @"\d")) {
                    errors = "Apellido no válido.";
                    return false;
                }

                if (apellidoMat.Length < 3) {
                    errors = "Apellido no válido.";
                    return false;
                }

            }

            return true;
        }
        private static bool ValidarTelefono(string? phone, out string errors) {

            errors = "";

            if (string.IsNullOrWhiteSpace(phone)) {
                errors = "Debe ingresar su número de télefono.";
                return false;
            }

            if (!Regex.IsMatch(phone, @"\d{10}")) {
                errors = "Número de télefono no válido.";
                return false;
            }

            if (phone.Length != 10) {
                errors = "Número de télefono no válido.";
                return false;
            }

            return true;
        }
        private static bool ValidarGenero(string? gender, out string errors) {

            errors = "";

            if (string.IsNullOrWhiteSpace(gender)) {
                errors = "Debe seleccionar su genero";
                return false;
            }

            return true;
        }
        private static bool ValidarCorreo(string? email, out string errors) {

            errors = "";

            if (string.IsNullOrWhiteSpace(email)) {
                errors = "Debe ingresar su correo institucional.";
                return false;
            }

            if (!Regex.IsMatch(email, @"[A-z]\d{8}@veracruz.tecnm.mx", RegexOptions.IgnoreCase)) {
                errors = "Correo no válido";
                return false;
            }

            // Validar de que ya no este registrada

            return true;
        }
        private static bool ValidarPassword(SecureString? password, out string errors) {

            errors = "";
            if (password is null || password.Length is 0) {
                errors = "Cree un contraseña.";
                return false;
            }

            if (password.Length < 10) {
                errors = "Contraseña muy corta.";
                return false;
            }

            if (password.Length > 10) {
                errors = "Contraseña muy larga.";
                return false;
            }

            return true;
        }
        private static bool ValidarSuscripcion(SuscriptionModel? suscription, out string errors) {

            errors = "";

            if (suscription is null) {
                errors = "Seleccione una suscripción.";
                return false;
            }

            return true;
        }

        private async void ExecuteRegisterCommand(object? obj) {

            if (IsErrorMessageVisible = !ValidarIcon(Icon, out string iconErrors)) {
                ErrorMessage = iconErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarFechaNacimiento(Birthday, out string birthdayErrors)) {
                ErrorMessage = birthdayErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarNoControl(NoControl, out string noControlErrors)) {
                ErrorMessage = noControlErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarNickname(Nickname, out string nicknameErrors)) {
                ErrorMessage = nicknameErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarNombre(Name, out string nameErrors)) {
                ErrorMessage = nameErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarApellidos(LastNamePat, LastNameMat, out string lastNameErrors)) {
                ErrorMessage = lastNameErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarTelefono(Phone, out string phoneErrors)) {
                ErrorMessage = phoneErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarGenero(Gender, out string genderErrors)) {
                ErrorMessage = genderErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarCorreo(Email, out string emailErrors)) {
                ErrorMessage = emailErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarPassword(Password, out string passwordErrors)) {
                ErrorMessage = passwordErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarSuscripcion(Suscription, out string suscriptionErrors)) {
                ErrorMessage = suscriptionErrors;
                return;
            }

            IsRegisterSuccessful = true;

            await Task.Delay(2000);

            ExecuteCloseCommand(obj);
        }

        private void ExecuteCloseCommand(object? obj) {
            NextWindow = new LoginView();
            IsViewVisible = false;
        }

        
    }
}
