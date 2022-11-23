using ITVMusic.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ITVMusic.ViewModels {
    public class LoginViewModel : ViewModelBase {

        // Fields
        private string? m_Username;
        private SecureString? m_Password;
        private string? m_ErrorMessage;
        private bool m_IsErrorMessageVisible;
        private bool m_IsUserSuscriptionExpired;

        public string? Username {
            get => m_Username;
            set {
                m_Username = value;
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
        public bool IsUserSuscriptionExpired {
            get => m_IsUserSuscriptionExpired;
            set {
                m_IsUserSuscriptionExpired = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand CloseCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public LoginViewModel() {

            IsErrorMessageVisible = false;
            IsUserSuscriptionExpired = false;

            CloseCommand = new ViewModelCommand(ExecuteCloseCommand);
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand);
            GoToRegisterCommand = new ViewModelCommand(ExecuteGoToRegisterCommand);
        }

        private void ExecuteCloseCommand(object? obj) {
            IsViewVisible = false;
        }

        private void ExecuteGoToRegisterCommand(object? obj) {

            NextWindow = new RegisterView();
            ExecuteCloseCommand(obj);
        }

        private static bool ValidarUsername(string? username, out string errors) {
            errors = "";

            // Validar si el campo está vacio
            if (string.IsNullOrWhiteSpace(username)) {
                errors = "Debe ingresar su nickname o su No. Control.";
                return false;
            }

            // Si no es un número de control
            if (!Regex.IsMatch(username, @"\[A-z]\d{8}")) {

                if (username.Length > 15) {
                    errors = "Nickname muy largo.";
                    return false;
                }

            }

            return true;
        }

        private static bool ValidarPassword(SecureString? password, out string errors) {

            errors = "";
            if (password is null || password.Length is 0) {
                errors = "Debe ingresar su contraseña.";
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

        private void ExecuteLoginCommand(object? obj) {

            IsUserSuscriptionExpired = false;

            // Comprobar el usuario y contraseña

            if (IsErrorMessageVisible = !ValidarUsername(Username, out string usernameErrors)) {
                ErrorMessage = usernameErrors;
                return;
            }

            if (IsErrorMessageVisible = !ValidarPassword(Password, out string passwordErrors)) {
                ErrorMessage = passwordErrors;
                return;
            }

            // Validar usuario
            bool isValidUser = false; // <- Cambiar por consulta a la base de datos
            bool isExpired = true;

            // Hacer con excepciones
            // Si la suscripcion ya no es valida, mostrar el mensaje de renovarla
            if (IsErrorMessageVisible = !isValidUser) {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return;
            }

            if (IsErrorMessageVisible = IsUserSuscriptionExpired = isExpired) {
                ErrorMessage = "Suscripción expirada.";
                return;
            }

            // NextWindow = new MainView();
            ExecuteCloseCommand(obj);
        }
    }
}
