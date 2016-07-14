using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MySiteUpdater.Annotations;
using MySiteUpdater.Commands;
using MySiteUpdater.Helpers;
using MySiteUpdater.Models;
using Newtonsoft.Json;

namespace MySiteUpdater.ViewModels
{
    class BusinessViewModel : INotifyPropertyChanged
    {
        private List<UserModel> _userList;
        private UserModel _selectedUser;
        private string _backGroundUrl;
        private string _updateUrl;
        private BackGroundModel _backGroundModel;
        private string _backgroundUrl;
        private string _username;
        private string _password;

        public BusinessViewModel()
        {
           
            Get = new DelegateCommand(x => GetBackground());
            Add = new DelegateCommand(x => AddBackground());
            Update = new DelegateCommand(x => UpdateBackground());
            NewUser = new DelegateCommand(x => AddUser());

            GetUsers();
        }

        public BackGroundModel BackGroundModel
        {
            get { return _backGroundModel; }
            set
            {
                _backGroundModel = value;
                OnPropertyChanged();
            }
        }

        private void AddBackground()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://justmysiteapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                var bm = new BackGroundModel()
                {
                    ID = BackGroundModel.ID,
                    UserID = SelectedUser.ID,
                    ImageAdress = UpdateUrl
                };

                var result = client.PutAsJsonAsync("api/background/addbackground", bm).Result;

                if (result.IsSuccessStatusCode)
                {

                }
                else
                {

                }
            }
            catch (Exception)
            {
                
                throw;
            }
          
        }

        public DelegateCommand Update { get; set; }
        public DelegateCommand Add { get; set; }
        public DelegateCommand Get { get; set; }
        public DelegateCommand NewUser { get; set; }
        public async void GetUsers()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://justmysiteapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                var jsonString = client.GetAsync("api/user/getall").Result.Content.ReadAsStringAsync().Result;

                UserList = JsonConvert.DeserializeObject<List<UserModel>>(jsonString);
            }
            catch (Exception)
            {
                
                throw;
            }
           

        }

      

       

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (SelectedUser != null)
                {
                    GetBackground();
                }
            }
        }


        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private void AddUser()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50050/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

            var loginModel = new LoginModel()
            {
                Username = _username,
                Password = _password
            };
            var result = client.PutAsJsonAsync("api/user/adduser", loginModel).Result;

            if (result.IsSuccessStatusCode)
            {
                GetUsers();
            }


        }

        private void GetBackground()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://justmysiteapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                var userid = SelectedUser.ID.ToString();
                var getBackground = $"api/background/getbackground/{userid}";

                var jsonString = client.GetAsync(getBackground).Result.Content.ReadAsStringAsync().Result;
                var background = JsonConvert.DeserializeObject<BackGroundModel>(jsonString);

                if (background != null)
                {
                    BackGroundModel = background;
                    BackgroundUrl = background.ImageAdress;
                    UpdateUrl = background.ImageAdress;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
         
           
        }
        private void UpdateBackground()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://justmysiteapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                var getBackground = $"api/background/updatebackground";

                var background = new BackGroundModel()
                {
                    ImageAdress = UpdateUrl,
                    ID = BackGroundModel.ID,
                    UserID = SelectedUser.ID
                };

                var response = client.PostAsJsonAsync(getBackground, background);

            }
            catch (Exception)
            {
                
                throw;
            }
            
           
           
        }

        public List<UserModel> UserList
        {
            get { return _userList; }
            set
            {
                _userList = value;
                
            }
        }

        public string BackgroundUrl
        {
            get { return _backgroundUrl; }
            set
            {
                _backgroundUrl = value;
                OnPropertyChanged();
            }
        }

        public string UpdateUrl
        {
            get { return _updateUrl; }
            set
            {
                _updateUrl = value;
                BackgroundUrl = _updateUrl;
                OnPropertyChanged();
            }
        }

       

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
