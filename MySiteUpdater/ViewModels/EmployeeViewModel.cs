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
using DocumentFormat.OpenXml.Drawing.Charts;
using MySiteUpdater.Annotations;
using MySiteUpdater.Commands;
using MySiteUpdater.Models;
using Newtonsoft.Json;

namespace MySiteUpdater.ViewModels
{
    class EmployeeViewModel : INotifyPropertyChanged
    {
        private string _title;
        private string _description;
        private string _pictureUrl;
        private DateTime _startDate;
        private DateTime _endDate;
        private List<EmployerModel> _employers;
        private Guid _pictureId;
        private EmployerModel _employer;
        private ObservableCollection<List<EmployerModel>> _employerModels;

        public EmployeeViewModel()
        {
            StartDate = new DateTime(2010,01,01);
            EndDate = DateTime.Now;
            GetEmployers();
            Add = new DelegateCommand(x => AddEmployee());
            Delete = new DelegateCommand(x => DeleteEmployer());
        }

       

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string PictureUrl
        {
            get { return _pictureUrl; }
            set
            {
                _pictureUrl = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        public EmployerModel SelectedEmployer
        {
            get { return _employer; }
            set
            {
                _employer = value;
                Description = SelectedEmployer.Description;
                Title = SelectedEmployer.EmployerName;
                PictureUrl = SelectedEmployer.PictureUrl;
                StartDate = SelectedEmployer.StartDate;
                EndDate = SelectedEmployer.EndDate;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<List<EmployerModel>> EmployerModels
        {
            get { return _employerModels; }
            set
            {
                _employerModels = value;
                OnPropertyChanged();
            }
        }

        public List<EmployerModel> Employers
        {
            get { return _employers; }
            set
            {
                _employers = value;
               OnPropertyChanged();
            }
        }

       

        public Guid PictureID
        {
            get { return _pictureId; }
            set
            {
                _pictureId = value;
                OnPropertyChanged();
            }
        }


        private void AddEmployee()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://justmysiteapi.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                var employer = new EmployerModel()
                {
                    ID = Guid.NewGuid(),
                    Description = Description,
                    EmployerName = Title,
                    StartDate = StartDate,
                    EndDate = EndDate
                };
                if (!string.IsNullOrWhiteSpace(PictureUrl))
                {
                    employer.PictureUrl = PictureUrl;
                }
                else
                {
                    employer.PictureUrl = null;
                }

                var addEmployer = $"api/employer/add/";

                var response = client.PostAsJsonAsync(addEmployer, employer).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetEmployers();
                }
            }
            catch (Exception)
            {
                
                throw;
            }

           
        }
        private void DeleteEmployer()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50050/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

            var response = client.PostAsJsonAsync("api/employer/delete/", SelectedEmployer).Result;

            if (response.IsSuccessStatusCode)
            {
                GetEmployers();
            }
        }

        public void GetEmployers()
        {
            try
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:50050/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Helpers.CredentialHelper.Credential);

                var jsonstring = client.GetAsync("api/employer/getall").Result.Content.ReadAsStringAsync().Result;
                var employers = JsonConvert.DeserializeObject<List<EmployerModel>>(jsonstring);

                Employers = Helpers.SortListHelper.SortEmployerListDescending(employers);

            }
            catch (Exception)
            {
                throw;
            }   
        }
           

        public DelegateCommand Add { get; set; }
        public DelegateCommand Delete { get; set; }
    
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
