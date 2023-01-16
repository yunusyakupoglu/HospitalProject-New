using AutoMapper;
using BL.IServices;
using Common;
using Common.Enums;
using DAL.UnitOfWorks;
using DTOs;
using FluentValidation;
using MaterialSkin;
using MaterialSkin.Controls;
using OL;
using Syncfusion.Windows.Forms;
using System.Windows.Forms;

namespace HospitalProject
{
    public partial class Form1 : MaterialForm
    {
        private readonly IDiseaseService _diseaseService;
        private readonly IPersonService _personService;
        private readonly IDiseaseDoctorService _diseaseDoctorService;
        private readonly IDiseasePatientService _diseasePatientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DiseaseCreateDto> _diseaseCreateValidator;
        private readonly IValidator<DiseaseUpdateDto> _diseaseUpdateValidator;
        private readonly IValidator<PersonCreateDto> _personCreateValidator;
        private readonly IValidator<PersonUpdateDto> _personUpdateValidator;
        private readonly IValidator<DiseaseDoctorCreateDto> _diseaseDoctorCreateValidator;
        private readonly IValidator<DiseaseDoctorUpdateDto> _diseaseDoctorUpdateValidator;
        private readonly IValidator<DiseasePatientCreateDto> _diseasePatientCreateValidator;
        private readonly IValidator<DiseasePatientUpdateDto> _diseasePatientUpdateValidator;
        private readonly IValidator<AppointmentCreateDto> _appointmentCreateValidator;
        private readonly IValidator<AppointmentUpdateDto> _appointmentUpdateValidator;
        private readonly IValidator<AppointmentIsCancelUpdateDto> _appointmentIsCancelUpdateValidator;
        public Form1(IDiseaseService diseaseService, IMapper mapper, IUnitOfWork unitOfWork, IValidator<DiseaseCreateDto> diseaseCreateValidator, IValidator<DiseaseUpdateDto> diseaseUpdateValidator, IPersonService personService, IValidator<PersonCreateDto> personCreateValidator, IValidator<PersonUpdateDto> personUpdateValidator, IValidator<DiseaseDoctorCreateDto> diseaseDoctorCreateValidator, IDiseaseDoctorService diseaseDoctorService, IValidator<DiseaseDoctorUpdateDto> diseaseDoctorUpdateValidator, IValidator<DiseasePatientCreateDto> diseasePatientCreateValidator, IValidator<DiseasePatientUpdateDto> diseasePatientUpdateValidator, IDiseasePatientService diseasePatientService, IAppointmentService appointmentService, IValidator<AppointmentCreateDto> appointmentCreateValidator, IValidator<AppointmentUpdateDto> appointmentUpdateValidator, IValidator<AppointmentIsCancelUpdateDto> appointmentIsCancelUpdateValidator)
        {
            MaterialSkin.MaterialSkinManager skinManager;
            skinManager = MaterialSkinManager.Instance;
            skinManager.EnforceBackcolorOnAllComponents = true;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.Blue500, Primary.Blue700, Primary.Blue300, Accent.Amber400, TextShade.WHITE);
            _diseaseService = diseaseService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _diseaseCreateValidator = diseaseCreateValidator;
            _diseaseUpdateValidator = diseaseUpdateValidator;
            InitializeComponent();
            _personService = personService;
            _personCreateValidator = personCreateValidator;
            _personUpdateValidator = personUpdateValidator;
            _diseaseDoctorCreateValidator = diseaseDoctorCreateValidator;
            _diseaseDoctorService = diseaseDoctorService;
            _diseaseDoctorUpdateValidator = diseaseDoctorUpdateValidator;
            _diseasePatientCreateValidator = diseasePatientCreateValidator;
            _diseasePatientUpdateValidator = diseasePatientUpdateValidator;
            _diseasePatientService = diseasePatientService;
            _appointmentService = appointmentService;
            _appointmentCreateValidator = appointmentCreateValidator;
            _appointmentUpdateValidator = appointmentUpdateValidator;
            _appointmentIsCancelUpdateValidator = appointmentIsCancelUpdateValidator;
        }

        protected async void Load_Graphics()
        {
            var plt = new ScottPlot.Plot(600, 400);

            var diseaseNames = await _diseaseService.GetAllDiseaseNames();
            var diseaseIdArray = await _diseaseService.GetDiseaseIdArrayByDiseaseNameArray(diseaseNames);
            double[] values = await _diseasePatientService.GetPatientCountByDiseaseIdArray(diseaseIdArray);
            if (values.Count() > 0)
            {
                var a = formsPlot1.Plot.AddLollipop(values);
                formsPlot1.Plot.XTicks(diseaseNames);
                a.ShowValuesAboveBars = true;
                formsPlot1.Plot.SetAxisLimits(yMin: 0);

                formsPlot1.Plot.SaveFig("bar_labels.png");
                formsPlot1.Refresh();
            }


            var titleId = await _personService.GetTitleId("Doktor");
            var doctorNames = await _personService.GetAllByTitleAsync(titleId);
            var doctorIds = await _personService.GetAllDoctorIdByNameAsync(doctorNames);
            var patientValues = await _appointmentService.GetPatientCountByDoctorIdAsync(doctorIds);
            if (patientValues.Count() > 0)
            {
                var b = formsPlot2.Plot.AddLollipop(patientValues);
                formsPlot2.Plot.XTicks(doctorNames);
                b.ShowValuesAboveBars = true;
                formsPlot2.Plot.SetAxisLimits(yMin: 0);

                formsPlot2.Plot.SaveFig("bar_labels.png");
                formsPlot2.Refresh();
            }

        }

        #region Form load
        private async void Form1_Load(object sender, EventArgs e)
        {
            Load_Graphics();
            DiseaseCounter();
            CmbPersonelTitleFill();
            DiseaseListViewRefresh();
            PersonListViewRefresh();
            cmbHourAndMinuteFill();
        }
        #endregion

        #region Tema rengi ayarlama
        private void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialSwitch1.Checked is not true) //Light
            {
                materialSwitch1.Text = "LIGHT";
                MaterialSkin.MaterialSkinManager skinManager;
                skinManager = MaterialSkinManager.Instance;
                skinManager.EnforceBackcolorOnAllComponents = true;
                skinManager.AddFormToManage(this);
                skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
                skinManager.ColorScheme = new ColorScheme(Primary.Blue500, Primary.Blue700, Primary.Blue300, Accent.Amber400, TextShade.WHITE);
            }
            else if (materialSwitch1.Checked is true) //Dark
            {
                materialSwitch1.Text = "DARK";
                MaterialSkin.MaterialSkinManager skinManager;
                skinManager = MaterialSkinManager.Instance;
                skinManager.EnforceBackcolorOnAllComponents = true;
                skinManager.AddFormToManage(this);
                skinManager.Theme = MaterialSkinManager.Themes.DARK;
                skinManager.ColorScheme = new ColorScheme(Primary.BlueGrey500, Primary.BlueGrey700, Primary.BlueGrey300, Accent.DeepOrange400, TextShade.WHITE);
            }

        }
        #endregion

        #region Dashboard Counter
        #region Disease Counts
        public async void DiseaseCounter()
        {
            // Disease
            var sumData = await _diseaseService.GetAllAsync();
            var activeData = await _diseaseService.GetActiveAsync();
            var inActiveData = await _diseaseService.GetInActiveAsync();

            lblSumDisease.Text = sumData.Data.Count.ToString();
            lblActiveDisease.Text = activeData.Data.Count.ToString();
            lblInactiveDisease.Text = inActiveData.Data.Count.ToString();

            //Doctor
            var titleDoctorId = await _personService.GetTitleId("Doktor");
            var sumDoctorData = await _personService.GetAllByTitleCountAsync(titleDoctorId);
            var activeDoctorData = await _personService.GetActiveAsync(titleDoctorId);
            var inActiveDoctorData = await _personService.GetInActiveAsync(titleDoctorId);

            lblSumDoctor.Text = sumDoctorData.Data.Count.ToString();
            lblActiveDoctor.Text = activeDoctorData.Data.Count.ToString();
            lblInActiveDoctor.Text = inActiveDoctorData.Data.Count.ToString();

            //Secretary
            var titleSecretaryId = await _personService.GetTitleId("Sekreter");
            var sumSecretaryData = await _personService.GetAllByTitleCountAsync(titleSecretaryId);
            var activeSecretaryData = await _personService.GetActiveAsync(titleSecretaryId);
            var inActiveSecretaryData = await _personService.GetInActiveAsync(titleSecretaryId);

            lblSectretary.Text = sumSecretaryData.Data.Count.ToString();
            lblActiveSecretary.Text = activeSecretaryData.Data.Count.ToString();
            lblInActiveSecretary.Text = inActiveSecretaryData.Data.Count.ToString();

            //Patient
            var titlePatientId = await _personService.GetTitleId("Hasta");
            var sumPatientData = await _personService.GetAllByTitleCountAsync(titlePatientId);
            var activePatientData = await _personService.GetActiveAsync(titlePatientId);
            var inActivePatientData = await _personService.GetInActiveAsync(titlePatientId);

            lblPatient.Text = sumPatientData.Data.Count.ToString();
            lblActivePatient.Text = activePatientData.Data.Count.ToString();
            lblInactivePatient.Text = inActivePatientData.Data.Count.ToString();
        }
        #endregion

        #endregion

        #region Tabcontrol selected tab
        private void materialTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialTabControl1.SelectedTab == materialTabControl1.TabPages["tabAppointment"])
            {
                CmbAppointmentDiseaseFill();
            }
        }
        #endregion

        #region Combobox data set
        #region Personel combobox
        public async void CmbPersonelTitleFill()
        {
            cmbPersonelTitle.Items.Clear();
            var titleData = await _personService.GetPersonTitleAsync();
            foreach (var item in titleData.Data)
            {
                cmbPersonelTitle.Items.Add(item.PersonTitle);
            }
        }
        public async void CmbDiseaseFill()
        {
            cmbDisease.Items.Clear();
            var diseaseData = await _diseaseService.GetAllAsync();
            foreach (var item in diseaseData.Data)
            {
                cmbDisease.Items.Add(item.Name);
            }
        }

        public async void CmbDoctorFill(int getDiseaseId)
        {
            cmbDoctor.Items.Clear();
            var doctorData = await _diseaseDoctorService.GetDoctorIdByDiseaseId(getDiseaseId);
            if (doctorData.Count > 0)
            {
                cmbDoctor.Enabled = true;
                foreach (var item in doctorData)
                {
                    var doctorName = await _personService.GetDoctorNameById(item);
                    cmbDoctor.Items.Add(doctorName);
                }
            }
            else
            {
                cmbDoctor.Enabled = false;
                MessageBox.Show("Seçilen hastalýk kategorisine ait hiçbir doktor bulunmamaktadýr. Lütfen sisteme doktor kaydediniz.");
            }

        }
        public async void cmbHourAndMinuteFill()
        {
            for (int hour = 0; hour < 24; hour++)
            {
                cmbHour.Items.Add(hour.ToString());
            }

            for (int minute = 0; minute < 60; minute += 5)
            {
                cmbMinute.Items.Add(minute.ToString());
            }
        }
        #endregion
        #region Appointment combobox
        public async void CmbAppointmentDiseaseFill()
        {
            materialListView3.Items.Clear();
            cmbAppointmentDisease.Items.Clear();
            var diseaseData = await _diseaseService.GetAllAsync();
            foreach (var item in diseaseData.Data)
            {
                cmbAppointmentDisease.Items.Add(item.Name);
            }
        }
        public async void CmbAppointmentDoctorFill(int getDiseaseId)
        {
            materialListView3.Items.Clear();
            cmbAppointmentDoctor.Items.Clear();
            var doctorData = await _diseaseDoctorService.GetDoctorIdByDiseaseId(getDiseaseId);
            if (doctorData.Count > 0)
            {
                cmbAppointmentDoctor.Enabled = true;
                foreach (var item in doctorData)
                {
                    var doctorName = await _personService.GetDoctorNameById(item);
                    cmbAppointmentDoctor.Items.Add(doctorName);
                }
            }
            else
            {
                cmbAppointmentDoctor.Enabled = false;
                MessageBox.Show("Seçilen hastalýk kategorisine ait hiçbir doktor bulunmamaktadýr. Lütfen sisteme doktor kaydediniz.");
            }

        }
        #endregion
        #endregion

        #region Combobox selectedIndex_Changed
        private async void cmbPersonelTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = cmbPersonelTitle.SelectedItem;
            if (selectedItem is "Doktor")
            {
                CmbDiseaseFill();
                cmbDisease.Visible = true;
                cmbDoctor.Visible = false;
                tableLayoutPanelButtons.ColumnCount = 3;

            }
            else if (selectedItem is "Hasta")
            {
                CmbDiseaseFill();
                cmbDisease.Visible = true;
            }
            else
            {
                cmbDisease.Visible = false;
                cmbDoctor.Visible = false;
                tableLayoutPanelButtons.ColumnCount = 3;
            }

        }

        private async void cmbDisease_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = cmbDisease.SelectedItem;
            var selectedTitleItem = cmbPersonelTitle.SelectedItem;
            var getDiseaseId = await _diseaseService.GetDiseaseId(selectedItem.ToString());
            if (selectedTitleItem is "Hasta")
            {
                CmbDoctorFill(getDiseaseId);
                cmbDoctor.Visible = true;

            }
            else
                cmbDoctor.Visible = false;

        }

        private async void cmbAppointmentDisease_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = cmbAppointmentDisease.SelectedItem;
            var getDiseaseId = await _diseaseService.GetDiseaseId(selectedItem.ToString());
            CmbAppointmentDoctorFill(getDiseaseId);
        }

        private async void cmbDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //tableLayoutPanelButtons.ColumnCount = 4;
            //tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            //tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            //tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            //tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));

            //btnAppointmentCalendar.Click += BtnAppointmentCalendar_Click;
            //tableLayoutPanelButtons.Controls.Add(btnAppointmentCalendar, 4, 0);

        }

        private async void cmbAppointmentDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = cmbAppointmentDoctor.SelectedItem;
            var doctorId = await _personService.GetSelectedDoctorId(selectedItem.ToString());
            AppointmentListViewRefresh(doctorId);
        }
        #endregion

        #region Listview Refresh Ýþlemleri
        #region DiseaseListView refresh
        public async void DiseaseListViewRefresh()
        {
            materialListView1.Items.Clear();
            var dataList = await _diseaseService.GetAllAsync();
            foreach (var data in dataList.Data)
            {
                ListViewItem item = new ListViewItem(data.Id.ToString());
                item.SubItems.Add(data.Name);
                if (data.Status is true)
                {
                    item.SubItems.Add("Aktif");
                }
                else
                {
                    item.SubItems.Add("Pasif");
                }
                materialListView1.Items.Add(item);
            }
        }
        #endregion
        #region PersonListView refresh
        public async void PersonListViewRefresh()
        {
            materialListView2.Items.Clear();
            var dataList = await _personService.GetAllAsync();
            foreach (var data in dataList.Data)
            {
                var personTitle = await _personService.GetTitleById(data.PersonTitleId);
                ListViewItem item = new ListViewItem(data.Id.ToString());
                item.SubItems.Add(data.FirstName);
                item.SubItems.Add(data.LastName);
                item.SubItems.Add(data.Email);
                item.SubItems.Add(data.PhoneNumber);
                item.SubItems.Add(personTitle);
                if (data.Status is true)
                {
                    item.SubItems.Add("Aktif");
                }
                else
                {
                    item.SubItems.Add("Pasif");
                }
                materialListView2.Items.Add(item);
            }
        }

        #endregion
        #region AppointmentListView refresh
        public async void AppointmentListViewRefresh(int doctorId)
        {
            materialListView3.Items.Clear();
            var dataList = await _appointmentService.GetAllAsync(doctorId);
            foreach (var data in dataList.Data)
            {
                var patientName = await _personService.GetDoctorNameById(data.PatientId);
                var doctorName = await _personService.GetDoctorNameById(data.DoctorId);
                var diseaseName = await _diseaseService.GetDiseaseNameById(data.DiseaseId);
                ListViewItem item = new ListViewItem(data.Id.ToString());
                item.SubItems.Add(patientName);
                item.SubItems.Add(diseaseName);
                item.SubItems.Add(doctorName);
                item.SubItems.Add(data.Date);
                item.SubItems.Add(data.Hour.ToString() + ":" + data.Minute.ToString());
                if (data.isActiveAppointment is true)
                {
                    item.SubItems.Add("Aktif");
                }
                else
                {
                    item.SubItems.Add("Pasif");
                }
                if (data.Status is true)
                {
                    item.SubItems.Add("Aktif");
                }
                else
                {
                    item.SubItems.Add("Pasif");
                }
                materialListView3.Items.Add(item);
            }
        }
        #endregion
        #endregion

        #region Toggle button checked changed
        #region Disease toggle button
        private void isActiveDisease_CheckedChanged(object sender, EventArgs e)
        {
            if (isActiveDisease.Checked is not true)
            {
                isActiveDisease.Text = "Pasif";

            }
            else if (isActiveDisease.Checked is true)
            {
                isActiveDisease.Text = "Aktif";
            }
        }
        #endregion
        #region Person toggle button
        private void isActivePerson_CheckedChanged(object sender, EventArgs e)
        {
            if (isActivePerson.Checked is not true)
            {
                isActivePerson.Text = "Pasif";

            }
            else if (isActivePerson.Checked is true)
            {
                isActivePerson.Text = "Aktif";
            }
        }
        #endregion
        #endregion

        #region  Listview selected index changed
        #region Disease listview
        private void materialListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialListView1.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = materialListView1.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                txtDiseaseName.Text = materialListView1.Items[intselectedindex].SubItems[1].Text;
                if (materialListView1.Items[intselectedindex].SubItems[2].Text is "Aktif")
                    isActiveDisease.Checked = true;
                else if (materialListView1.Items[intselectedindex].SubItems[2].Text is "Pasif")
                    isActiveDisease.Checked = false;
            }
        }
        #endregion
        #region Person ListView
        private async void materialListView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialListView2.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = materialListView2.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                int personId = Convert.ToInt32(materialListView2.Items[intselectedindex].SubItems[0].Text);
                txtName.Text = materialListView2.Items[intselectedindex].SubItems[1].Text;
                txtSurname.Text = materialListView2.Items[intselectedindex].SubItems[2].Text;
                txtEmail.Text = materialListView2.Items[intselectedindex].SubItems[3].Text;
                txtPhone.Text = materialListView2.Items[intselectedindex].SubItems[4].Text;
                cmbPersonelTitle.Text = materialListView2.Items[intselectedindex].SubItems[5].Text;
                if (materialListView2.Items[intselectedindex].SubItems[6].Text is "Aktif")
                    isActivePerson.Checked = true;
                else if (materialListView2.Items[intselectedindex].SubItems[6].Text is "Pasif")
                    isActivePerson.Checked = false;
                if (cmbPersonelTitle.Text is "Doktor")
                {
                    cmbDoctor.Visible = false;
                    var getDiseaseId = await _diseaseDoctorService.GetIdByPersonId(personId);
                    var diseaseText = await _diseaseService.GetDiseaseNameById(getDiseaseId);
                    cmbDisease.SelectedItem = diseaseText;
                }
                else if (cmbPersonelTitle.Text is "Hasta")
                {
                    cmbDoctor.Visible = true;
                    var id = await _appointmentService.GetIdByPatientId(personId);
                    var appointmentData = await _appointmentService.GetByIdAsync<AppointmentListDto>(id);
                    var diseaseText = await _diseaseService.GetDiseaseNameById(appointmentData.Data.DiseaseId);
                    var selectedDoctorName = await _personService.GetDoctorNameById(appointmentData.Data.DoctorId);
                    cmbDisease.SelectedItem = diseaseText;
                    cmbHour.SelectedItem = appointmentData.Data.Hour.ToString();
                    cmbMinute.SelectedItem = appointmentData.Data.Minute.ToString();
                    cmbDoctor.SelectedItem = selectedDoctorName.ToString();
                    cmbHour.Text = appointmentData.Data.Hour.ToString();
                    cmbMinute.Text = appointmentData.Data.Minute.ToString();
                    cmbDoctor.Text = selectedDoctorName;
                    sfCalendar1.SelectedDate = Convert.ToDateTime(appointmentData.Data.Date).Date;

                    CmbDoctorFill(appointmentData.Data.DiseaseId);
                }
            }
        }
        #endregion
        #region Appointment ListView
        private void materialListView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialListView3.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = materialListView3.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                int personId = Convert.ToInt32(materialListView3.Items[intselectedindex].SubItems[0].Text);
                lblAppointmentPatientNameSurname.Text = materialListView3.Items[intselectedindex].SubItems[1].Text;
                lblAppointmentDoctorName.Text = materialListView3.Items[intselectedindex].SubItems[3].Text;
                lblAppointmentDate.Text = materialListView3.Items[intselectedindex].SubItems[4].Text;
                lblAppointmentTime.Text = materialListView3.Items[intselectedindex].SubItems[5].Text;
                cmbPersonelTitle.Text = materialListView3.Items[intselectedindex].SubItems[5].Text;
                if (materialListView3.Items[intselectedindex].SubItems[6].Text is "Aktif")
                    cbAppointmentIsActive.Checked = true;
                else if (materialListView3.Items[intselectedindex].SubItems[6].Text is "Pasif")
                    cbAppointmentIsActive.Checked = false;
            }
        }
        #endregion
        #endregion

        #region Kaydetme iþlemleri
        #region Disease Create Button
        private async void btnDisease_Click(object sender, EventArgs e)
        {
            DiseaseCreateDto createDto = new DiseaseCreateDto();
            createDto.Name = txtDiseaseName.Text;
            if (isActiveDisease.Checked is true)
                createDto.Status = true;
            else
                createDto.Status = false;
            var validationResult = _diseaseCreateValidator.Validate(createDto);
            if (validationResult.IsValid)
            {
                var dto = _mapper.Map<DiseaseCreateDto>(createDto);
                var response = await _diseaseService.CreateAsync(dto);
                DiseaseListViewRefresh();
                DiseaseCounter();
                MessageBox.Show(response.Message);
            }

        }
        #endregion
        #region Person create button
        private async void btnPerson_Click(object sender, EventArgs e)
        {
            var selectedItem = cmbPersonelTitle.SelectedItem;
            PersonCreateDto createDto = new PersonCreateDto();
            createDto.FirstName = txtName.Text;
            createDto.LastName = txtSurname.Text;
            createDto.Email = txtEmail.Text;
            createDto.PhoneNumber = txtPhone.Text;
            createDto.PersonTitleId = await _personService.GetTitleId(selectedItem.ToString());
            if (isActivePerson.Checked is true)
                createDto.Status = true;
            else
                createDto.Status = false;
            var validationResult = _personCreateValidator.Validate(createDto);
            if (validationResult.IsValid)
            {
                var dto = _mapper.Map<PersonCreateDto>(createDto);
                var response = await _personService.CreateAsync(dto);
                PersonListViewRefresh();
                DiseaseCounter();
            }
            if (selectedItem is "Sekreter")
            {
                MessageBox.Show($"{createDto.FirstName} {createDto.LastName}\n" +
                        $"Sekreter baþarýyla kaydedildi.",
                        "Sekreter Kayýt Ýþlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (selectedItem is "Doktor")
            {
                var diseaseSelectedItem = cmbDisease.SelectedItem;
                DiseaseDoctorCreateDto diseaseDoctorCreateDto = new DiseaseDoctorCreateDto();
                diseaseDoctorCreateDto.PersonId = await _personService.GetDoctorOrPatientId(txtName.Text, txtSurname.Text, txtPhone.Text, txtEmail.Text);
                diseaseDoctorCreateDto.DiseaseId = await _diseaseService.GetDiseaseId(diseaseSelectedItem.ToString());
                var diseaseDoctorValidationResult = _diseaseDoctorCreateValidator.Validate(diseaseDoctorCreateDto);
                if (diseaseDoctorValidationResult.IsValid)
                {
                    var diseaseDto = _mapper.Map<DiseaseDoctorCreateDto>(diseaseDoctorCreateDto);
                    var response = await _diseaseDoctorService.CreateAsync(diseaseDto);
                    MessageBox.Show($"{createDto.FirstName} {createDto.LastName}\n" +
                        $"Doktor {cmbDisease.SelectedItem} branþýna baþarýyla kaydedildi.",
                        "Doktor Kayýt Ýþlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (selectedItem is "Hasta")
            {
                var diseaseSelectedItem = cmbDisease.SelectedItem;
                var selectedDoctorItem = cmbDoctor.SelectedItem;
                DiseasePatientCreateDto diseasePatientCreateDto = new DiseasePatientCreateDto();
                diseasePatientCreateDto.PersonId = await _personService.GetDoctorOrPatientId(txtName.Text, txtSurname.Text, txtPhone.Text, txtEmail.Text);
                diseasePatientCreateDto.DiseaseId = await _diseaseService.GetDiseaseId(diseaseSelectedItem.ToString());
                var diseasePatientValidationResult = _diseasePatientCreateValidator.Validate(diseasePatientCreateDto);
                if (diseasePatientValidationResult.IsValid)
                {
                    var diseasePatientDto = _mapper.Map<DiseasePatientCreateDto>(diseasePatientCreateDto);
                    var response = await _diseasePatientService.CreateAsync(diseasePatientDto);
                }
                AppointmentCreateDto appointmentCreateDto = new AppointmentCreateDto();
                appointmentCreateDto.DiseaseId = await _diseaseService.GetDiseaseId(diseaseSelectedItem.ToString());
                appointmentCreateDto.PatientId = diseasePatientCreateDto.PersonId;
                appointmentCreateDto.DoctorId = await _personService.GetSelectedDoctorId(selectedDoctorItem.ToString());
                appointmentCreateDto.Date = sfCalendar1.SelectedDate.Value.ToShortDateString();
                appointmentCreateDto.Hour = Convert.ToInt32(cmbHour.SelectedItem);
                appointmentCreateDto.Minute = Convert.ToInt32(cmbMinute.SelectedItem);
                appointmentCreateDto.Status = true;
                var appointmentValidationResult = _appointmentCreateValidator.Validate(appointmentCreateDto);
                if (appointmentValidationResult.IsValid)
                {
                    var appointmentDto = _mapper.Map<AppointmentCreateDto>(appointmentCreateDto);
                    var response = await _appointmentService.CreateAsync(appointmentDto);
                    MessageBox.Show($"{createDto.FirstName} {createDto.LastName}\n randevu kaydý oluþturuldu. \n" +
                        $" Hastalýðý {cmbDisease.SelectedItem} \n" +
                        $"Tarih {appointmentCreateDto.Date}\n" +
                        $"Randevu saati {appointmentCreateDto.Hour.ToString("00")}:{appointmentCreateDto.Minute.ToString("00")}",
                        "Hasta ve Randevu Kayýt Ýþlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion
        #endregion

        #region Güncelleme iþlemleri
        #region Disease Update
        private async void btnUpdateDisease_ClickAsync(object sender, EventArgs e)
        {
            DiseaseUpdateDto updateDto = new DiseaseUpdateDto();
            updateDto.Id = Convert.ToInt32(materialListView1.SelectedItems[0].SubItems[0].Text);
            updateDto.Name = txtDiseaseName.Text;
            if (isActiveDisease.Checked is true)
                updateDto.Status = true;
            else
                updateDto.Status = false;
            var validationResult = _diseaseUpdateValidator.Validate(updateDto);
            if (validationResult.IsValid)
            {
                var dto = _mapper.Map<DiseaseUpdateDto>(updateDto);
                var response = await _diseaseService.UpdateAsync(dto);
                DiseaseListViewRefresh();
                DiseaseCounter();
                MessageBox.Show(response.Message);
            }
        }
        #endregion
        #region Person Update
        private async void btnUpdatePerson_Click(object sender, EventArgs e)
        {
            var selectedItem = cmbPersonelTitle.SelectedItem;
            var selectedDiseaseItem = cmbDisease.SelectedItem;
            int personId = Convert.ToInt32(materialListView2.SelectedItems[0].SubItems[0].Text);
            PersonUpdateDto updateDto = new PersonUpdateDto();
            updateDto.Id = personId;
            updateDto.FirstName = txtName.Text;
            updateDto.LastName = txtSurname.Text;
            updateDto.Email = txtEmail.Text;
            updateDto.PhoneNumber = txtPhone.Text;
            updateDto.PersonTitleId = await _personService.GetTitleId(selectedItem.ToString());
            if (isActivePerson.Checked is true)
                updateDto.Status = true;
            else
                updateDto.Status = false;
            var validationResult = _personUpdateValidator.Validate(updateDto);
            if (validationResult.IsValid)
            {
                var dto = _mapper.Map<PersonUpdateDto>(updateDto);
                var response = await _personService.UpdateAsync(dto);
                PersonListViewRefresh();
                DiseaseCounter();
            }
            if (cmbPersonelTitle.Text is "Doktor")
            {
                var selectedDiseaseId = await _diseaseService.GetDiseaseId(selectedDiseaseItem.ToString());
                DiseaseDoctorUpdateDto diseaseDoctorUpdateDto = new DiseaseDoctorUpdateDto();
                diseaseDoctorUpdateDto.Id = await _diseaseDoctorService.GetIdByDoctorId(personId);
                diseaseDoctorUpdateDto.PersonId = personId;
                diseaseDoctorUpdateDto.DiseaseId = selectedDiseaseId;
                var diseaseValidationResult = _diseaseDoctorUpdateValidator.Validate(diseaseDoctorUpdateDto);
                if (diseaseValidationResult.IsValid)
                {
                    var dto = _mapper.Map<DiseaseDoctorUpdateDto>(diseaseDoctorUpdateDto);
                    var response = await _diseaseDoctorService.UpdateAsync(dto);
                    PersonListViewRefresh();
                    DiseaseCounter();
                }
            }
            else if (cmbPersonelTitle.Text is "Hasta")
            {
                var diseaseSelectedItem = cmbDisease.SelectedItem;
                var selectedDoctorItem = cmbDoctor.SelectedItem;
                DiseasePatientUpdateDto diseasePatientUpdateDto = new DiseasePatientUpdateDto();
                var diseasePatientId = await _diseasePatientService.GetIdByPatientId(personId);
                diseasePatientUpdateDto.Id = diseasePatientId;
                diseasePatientUpdateDto.PersonId = await _personService.GetDoctorOrPatientId(txtName.Text, txtSurname.Text, txtPhone.Text, txtEmail.Text);
                diseasePatientUpdateDto.DiseaseId = await _diseaseService.GetDiseaseId(diseaseSelectedItem.ToString());
                var diseasePatientValidationResult = _diseasePatientUpdateValidator.Validate(diseasePatientUpdateDto);
                if (diseasePatientValidationResult.IsValid)
                {
                    var diseasePatientDto = _mapper.Map<DiseasePatientUpdateDto>(diseasePatientUpdateDto);
                    var response = await _diseasePatientService.UpdateAsync(diseasePatientDto);
                }
                AppointmentUpdateDto appointmentUpdateDto = new AppointmentUpdateDto();
                var id = await _appointmentService.GetIdByPatientId(personId);
                appointmentUpdateDto.Id = id;
                appointmentUpdateDto.DiseaseId = await _diseaseService.GetDiseaseId(diseaseSelectedItem.ToString());
                appointmentUpdateDto.PatientId = diseasePatientUpdateDto.PersonId;
                appointmentUpdateDto.DoctorId = await _personService.GetSelectedDoctorId(selectedDoctorItem.ToString());
                appointmentUpdateDto.Date = sfCalendar1.SelectedDate.Value.ToShortDateString();
                appointmentUpdateDto.Hour = Convert.ToInt32(cmbHour.SelectedItem);
                appointmentUpdateDto.Minute = Convert.ToInt32(cmbMinute.SelectedItem);
                appointmentUpdateDto.Status = true;
                var appointmentValidationResult = _appointmentUpdateValidator.Validate(appointmentUpdateDto);
                if (appointmentValidationResult.IsValid)
                {
                    var appointmentDto = _mapper.Map<AppointmentUpdateDto>(appointmentUpdateDto);
                    var response = await _appointmentService.UpdateAsync(appointmentDto);
                    MessageBox.Show($"{updateDto.FirstName} {updateDto.LastName}\n randevu kaydý güncellendi. \n" +
                        $" Hastalýðý {cmbDisease.SelectedItem} \n" +
                        $"Tarih {appointmentUpdateDto.Date}\n" +
                        $"Randevu saati {appointmentUpdateDto.Hour.ToString("00")}:{appointmentUpdateDto.Minute.ToString("00")}",
                        "Hasta ve Randevu Güncelleme Ýþlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion
        #region Appointment Update
        private async void materialButton2_Click(object sender, EventArgs e)
        {
            AppointmentIsCancelUpdateDto updateDto = new AppointmentIsCancelUpdateDto();
            updateDto.Id = Convert.ToInt32(materialListView3.SelectedItems[0].SubItems[0].Text);
            if (cbAppointmentIsActive.Checked is true)
                updateDto.isActiveAppointment = true;
            else
                updateDto.isActiveAppointment = false;
            var validationResult = _appointmentIsCancelUpdateValidator.Validate(updateDto);
            if (validationResult.IsValid)
            {
                var dto = _mapper.Map<AppointmentIsCancelUpdateDto>(updateDto);
                var response = await _appointmentService.UpdateIsCancelAsync(dto);
                var doctorId = await _personService.GetSelectedDoctorId(cmbAppointmentDoctor.SelectedItem.ToString());
                AppointmentListViewRefresh(doctorId);
                DiseaseCounter();
            }
        }
        #endregion
        #endregion

        #region Silme iþlemleri
        #region Disease Delete
        private async void btnDeleteDisease_Click(object sender, EventArgs e)
        {
            int selectedId = Convert.ToInt32(materialListView1.SelectedItems[0].SubItems[0].Text);
            var deletedData = await _diseaseService.RemoveAsync(selectedId);
            if (deletedData.ResponseType is Common.Enums.ResponseType.Success)
            {
                DiseaseListViewRefresh();
                DiseaseCounter();
                txtDiseaseName.Text = string.Empty;
                isActiveDisease.Checked = false;
            }

        }
        #endregion
        #region Person Delete
        private async void btnDeletePerson_Click(object sender, EventArgs e)
        {
            int selectedId = Convert.ToInt32(materialListView2.SelectedItems[0].SubItems[0].Text);
            var deletedData = await _personService.RemoveAsync(selectedId);
            if (deletedData.ResponseType is Common.Enums.ResponseType.Success)
            {
                PersonListViewRefresh();
                DiseaseCounter();
                txtName.Text = string.Empty;
                txtSurname.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtPhone.Text = string.Empty;
                cmbPersonelTitle.Text = string.Empty;
                isActivePerson.Checked = false;
            }
        }

        #endregion

        #endregion

        private void materialButton3_Click(object sender, EventArgs e)
        {

        }
    }
}