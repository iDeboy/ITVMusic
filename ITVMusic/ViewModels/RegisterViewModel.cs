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
using ITVMusic.Repositories.Bases;
using ITVMusic.Repositories;
using System.Linq;
using System.Net;
using System.Diagnostics.CodeAnalysis;

namespace ITVMusic.ViewModels {
    public class RegisterViewModel : ViewModelBase {

        // Data
        private readonly IUserRepository userRepository;
        private readonly ISuscriptionRepository suscriptionRepository;


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
        public ObservableCollection<string> Genders { get; }
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
        public string? PhoneNumber {
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

        private ObservableCollection<UserModel> AllUsers { get; }

        // Commands
        public ICommand CloseCommand { get; }
        public ICommand RegisterCommand { get; }

        public RegisterViewModel() {

            Suscriptions = new();
            Genders = new();
            AllUsers = new();

            userRepository = new UserRepository();
            suscriptionRepository = new SuscriptionRepository();

            InitSuscriptions();
            InitGenders();
            InitAllUsers();

            CloseCommand = new ViewModelCommand(ExecuteCloseCommand, CanExecuteCloseCommand);
            RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
        }

        private bool CanExecuteRegisterCommand(object? obj) {
            return !IsRegisterSuccessful;
        }

        private bool CanExecuteCloseCommand(object? obj) {
            return !IsRegisterSuccessful;
        }

        private async void InitSuscriptions() {

            Suscriptions.Clear();

            Suscriptions.AddRange(await suscriptionRepository.GetByAll());

        }

        private void InitGenders() {

            Genders.Clear();

            Genders.Add("Masculino");
            Genders.Add("Femenino");
            Genders.Add("Indefinido");

        }
        private async void InitAllUsers() {

            AllUsers.Clear();

            AllUsers.AddRange(await userRepository.GetByAll());

        }
        private void FillSuscriptionsTest() {
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

        private static bool ValidarIcon([NotNullWhen(true)] ImageSource? imageSource, out string errors) {
            errors = "";

            if (imageSource is not BitmapImage image) {
                errors = "Debe de elegir un icono válido.";
                return false;
            }

            if (image.UriSource is null) {
                errors = "No se pudo cargar la imagen.";
                return false;
            }

            return true;
        }
        private static bool ValidarFechaNacimiento([NotNullWhen(true)] DateOnly? birthday, out string errors) {

            errors = "";

            if (!birthday.HasValue || birthday.Value.IsDefault()) {
                errors = "Debe ingresar su fecha de nacimiento.";
                return false;
            }

            if (!birthday.Value.IsValid()) {
                errors = "Fecha de nacimiento no válida.";
                return false;
            }

            return true;
        }
        private bool ValidarNoControl([NotNullWhen(true)] string? noControl, out string errors) {
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
            if (AllUsers.Any(u => u.NoControl == noControl)) {
                errors = "No. Control ya está registrado.";
                return false;
            }

            return true;
        }
        private bool ValidarNickname([NotNullWhen(true)] string? nickname, out string errors) {
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
            if (AllUsers.Any(u => u.Nickname == nickname)) {
                errors = "Nickname ya está registrado.";
                return false;
            }

            return true;
        }
        private static bool ValidarNombre([NotNullWhen(true)] string? nombre, out string errors) {

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
        private static bool ValidarApellidos([NotNullWhen(true)] string? apellidoPat, string? apellidoMat, out string errors) {

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
        private static bool ValidarTelefono([NotNullWhen(true)] string? phoneNumber, out string errors) {

            errors = "";

            if (string.IsNullOrWhiteSpace(phoneNumber)) {
                //errors = "Debe ingresar su número de télefono.";
                return true;
            }

            if (!Regex.IsMatch(phoneNumber, @"\d{10}")) {
                errors = "Número de télefono no válido.";
                return false;
            }

            return true;
        }
        private static bool ValidarGenero([NotNullWhen(true)] string? gender, out string errors) {

            errors = "";

            if (string.IsNullOrWhiteSpace(gender)) {
                errors = "Debe seleccionar su genero";
                return false;
            }

            return true;
        }
        private bool ValidarCorreo([NotNullWhen(true)] string? email, out string errors) {

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
            if (AllUsers.Any(u => u.Email == email)) {
                errors = "Correo ya está registrado.";
                return false;
            }

            return true;
        }
        private static bool ValidarPassword([NotNullWhen(true)] SecureString? password, out string errors) {

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
        private static bool ValidarSuscripcion([NotNullWhen(true)] SuscriptionModel? suscription, out string errors) {

            errors = "";

            if (suscription is null) {
                errors = "Seleccione una suscripción.";
                return false;
            }

            return true;
        }

        private async void ExecuteRegisterCommand(object? obj) {

            /*if (IsErrorMessageVisible = !ValidarIcon(Icon, out string iconErrors)) {
                ErrorMessage = iconErrors;
                return;
            }*/

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

            if (IsErrorMessageVisible = !ValidarTelefono(PhoneNumber, out string phoneErrors)) {
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

            IsRegisterSuccessful = await userRepository.Add(new() {
                Icon = Icon,
                Birthday = Birthday!.Value,
                NoControl = NoControl,
                Nickname = Nickname,
                Name = Name,
                LastNamePat = LastNamePat,
                LastNameMat = LastNameMat,
                Gender = Gender,
                Email = Email,
                Password = new NetworkCredential(null, Password).Password,
                SuscriptionId = Suscription!.Id,
            });

            await Task.Delay(2000);

            ExecuteCloseCommand(obj);
        }

        private void ExecuteCloseCommand(object? obj) {
            NextWindow = new LoginView();
            IsViewVisible = false;
        }


    }
}
