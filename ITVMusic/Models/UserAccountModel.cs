using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ITVMusic.Models {
    public class UserAccountModel : UserModel {

        private UserModel? UserData { get; }

        public UserAccountModel(UserModel? userData = null) {
            UserData = userData;

            if (UserData is null) return;

            NoControl = UserData.NoControl;
            SuscriptionId = UserData.SuscriptionId;
            Icon = UserData.Icon;
            Name = UserData.Name;
            LastNamePat = UserData.LastNamePat;
            LastNameMat = UserData.LastNameMat;
            Nickname = UserData.Nickname;
            Gender = UserData.Gender;
            Email = UserData.Email;
            Birthday = UserData.Birthday;
            PhoneNumber = UserData.PhoneNumber;
            Password = UserData.Password;
            ContratationDate = UserData.ContratationDate;

        }

        public UserAccountModel(UserAccountModel? userAccount = null)
            : this(userAccount?.UserData) { }

    }
}
