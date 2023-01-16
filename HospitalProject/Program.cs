using AutoMapper;
using BL.IServices;
using BL.Mappings;
using BL.Services;
using BL.ValidationRules;
using DAL;
using DAL.UnitOfWorks;
using DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OL;

namespace HospitalProject
{
    internal static class Program
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            Application.Run(ServiceProvider.GetRequiredService<Form1>());
        }

        public static IServiceProvider ServiceProvider { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddDbContext<ApplicationDbContext>();
                    services.AddScoped<IUnitOfWork, UnitOfWork>();


                    services.AddTransient<IValidator<PersonCreateDto>, PersonCreateDtoValidator>();
                    services.AddTransient<IValidator<PersonUpdateDto>, PersonUpdateDtoValidator>();
                    services.AddTransient<IValidator<DiseaseCreateDto>, DiseaseCreateDtoValidator>();
                    services.AddTransient<IValidator<DiseaseUpdateDto>, DiseaseUpdateDtoValidator>();
                    services.AddTransient<IValidator<DiseaseDoctorCreateDto>, DiseaseDoctorCreateDtoValidator>();
                    services.AddTransient<IValidator<DiseaseDoctorUpdateDto>, DiseaseDoctorUpdateDtoValidator>();
                    services.AddTransient<IValidator<DiseasePatientCreateDto>, DiseasePatientCreateDtoValidator>();
                    services.AddTransient<IValidator<DiseasePatientUpdateDto>, DiseasePatientUpdateDtoValidator>();
                    services.AddTransient<IValidator<AppointmentCreateDto>, AppointmentCreateDtoValidator>();
                    services.AddTransient<IValidator<AppointmentUpdateDto>, AppointmentUpdateDtoValidator>();
                    services.AddTransient<IValidator<AppointmentIsCancelUpdateDto>, AppointmentIsCancelUpdateDtoValidator>();


                    services.AddTransient<IPersonService, PersonService>();
                    services.AddTransient<IDiseaseService, DiseaseService>();
                    services.AddTransient<IDiseaseDoctorService, DiseaseDoctorService>();
                    services.AddTransient<IDiseasePatientService, DiseasePatientService>();
                    services.AddTransient<IAppointmentService, AppointmentService>();

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile(new MapProfile());
                    });

                    var mapper = config.CreateMapper();

                    services.AddSingleton(mapper);
                    services.AddTransient<Form1>();


                });
        }
    }
}