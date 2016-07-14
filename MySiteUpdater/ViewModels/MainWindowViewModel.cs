using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MySiteUpdater.Helpers;
using MySiteUpdater.Models;
using Newtonsoft.Json;

namespace MySiteUpdater.ViewModels
{
    
    class MainWindowViewModel 
    {
        private ObservableCollection<object> _children;
        public ObservableCollection<object> Children { get { return _children; } }
        public MainWindowViewModel()
        {
            GetAccessLevel();
            _children = new ObservableCollection<object>();
            _children.Add(new BusinessViewModel());
            _children.Add(new EmployeeViewModel());

        }

        private int _retries = 0;
        public async void GetAccessLevel()
        {
            try
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://justmysiteapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                CredentialHelper.Credential = Convert.ToBase64String(Encoding.UTF8.GetBytes("######" + ":" + "######"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);



                var response = await client.GetAsync("api/login/getaccesslevel");

                if (!response.IsSuccessStatusCode)
                {
                    _retries++;
                    if (_retries <= 5)
                    {
                        GetAccessLevel();
                    }
                    else
                    {
                        
                    }
                }
            }
            catch (Exception)
            {
               
            }
            



        }
      
    }
}

