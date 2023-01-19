using AutoMapper;
using BL.IServices;
using DAL.UnitOfWorks;
using DTOs;
using FluentValidation;
using MaterialSkin.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalProject
{
    public partial class Form2 : MaterialForm
    {
        private readonly IPersonService _personService;
        public Form2(IPersonService personService)
        {
            InitializeComponent();
            _personService = personService;
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            var titleId = await _personService.Login(txtName.Text, txtLastName.Text);
            using (var form1 = Program.ServiceProvider.GetRequiredService<Form1>())
            {
                form1.frm2Data = titleId;
                form1.ShowDialog();
                this.Close();
            }

        }
    }
}
