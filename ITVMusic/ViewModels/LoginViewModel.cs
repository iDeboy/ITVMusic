using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ITVMusic.ViewModels {
    public class LoginViewModel : ViewModelBase {

        // Fields
        private string? m_Username;
        private SecureString? m_Password;
        private string? m_ErrorMessage;
        private bool m_IsErrorMessageVisible;
        private bool m_IsUserSuscriptionExpired;
        private bool m_IsViewVisible;

        public string? Username {
            get => m_Username;
            set {
                m_Username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public SecureString? Password {
            get => m_Password;
            set {
                m_Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string? ErrorMessage {
            get => m_ErrorMessage;
            set {
                m_ErrorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible {
            get => m_IsViewVisible;
            set {
                m_IsViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }
        public bool IsErrorMessageVisible {
            get => m_IsErrorMessageVisible;
            set {
                m_IsErrorMessageVisible = value;
                OnPropertyChanged(nameof(IsErrorMessageVisible));
            }
        }
        public bool IsUserSuscriptionExpired {
            get => m_IsUserSuscriptionExpired;
            set {
                m_IsUserSuscriptionExpired = value;
                OnPropertyChanged(nameof(IsUserSuscriptionExpired));
            }
        }

        // Commands
        public ICommand LoginCommand { get; }

        public LoginViewModel() {

            IsViewVisible = true;
            IsErrorMessageVisible = false;
            IsUserSuscriptionExpired = false;

            LoginCommand = new ViewModelCommand(ExecuteLoginCommand);

        }

        private static bool ValidarUsername(string? username, out string errors) {
            errors = "";

            // Validar si el campo está vacio
            if (string.IsNullOrWhiteSpace(username)) {
                errors = "Campo de No. Control vácio.";
                return false;
            }

            // Si no es un número de control
            if (!Regex.IsMatch(username, @"\w\d{1,8}")) {

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
                errors = "Campo de contraseña vácia.";
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

            // Hacer con excepciones
            // Si la suscripcion ya no es valida, mostrar el mensaje de renovarla
            if (IsErrorMessageVisible = !isValidUser) {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return;
            }

            IsViewVisible = false;
        }
    }
}
