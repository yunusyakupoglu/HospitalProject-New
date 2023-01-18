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
            SetFullControlPermissionsToEveryone(AppDomain.CurrentDomain.BaseDirectory);
            InitializeComponent();
            _personService = personService;
        }
        static void SetFullControlPermissionsToEveryone(string path)
        {
            const FileSystemRights rights = FileSystemRights.FullControl;

            var allUsers = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

            // Add Access Rule to the actual directory itself
            var accessRule = new FileSystemAccessRule(
                allUsers,
                rights,
                InheritanceFlags.None,
                PropagationFlags.NoPropagateInherit,
                AccessControlType.Allow);

            var info = new DirectoryInfo(path);
            var security = info.GetAccessControl(AccessControlSections.Access);

            bool result;
            security.ModifyAccessRule(AccessControlModification.Set, accessRule, out result);

            if (!result)
            {
                MessageBox.Show("Failed to give full-control permission to all users for path " + path);
            }

            // add inheritance
            var inheritedAccessRule = new FileSystemAccessRule(
                allUsers,
                rights,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.InheritOnly,
                AccessControlType.Allow);

            bool inheritedResult;
            security.ModifyAccessRule(AccessControlModification.Add, inheritedAccessRule, out inheritedResult);

            if (!inheritedResult)
            {
                MessageBox.Show("Failed to give full-control permission inheritance to all users for " + path);
            }

            info.SetAccessControl(security);
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
            }

        }
    }
}
