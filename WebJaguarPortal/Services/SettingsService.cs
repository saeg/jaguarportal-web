using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using WebJaguarPortal.Models;
using WebJaguarPortal.Repository.Interfaces;

namespace WebJaguarPortal.Services
{
    public class SettingsService
    {
        private readonly IRepository<Settings> repoSettings;

        public SettingsService(IRepository<Settings> repository)
        {
            this.repoSettings = repository;
        }


        public void Update(Settings setting)
        {
            var settingsOld = Get();
            if (settingsOld == null)
                return;

            settingsOld.SmtpFrom = setting.SmtpFrom;
            settingsOld.SmtpPort = setting.SmtpPort;
            settingsOld.SmtpUsername = setting.SmtpUsername;
            settingsOld.SmtpAddress = setting.SmtpAddress;
            settingsOld.SmtpUseSSL = setting.SmtpUseSSL;
            if (!string.IsNullOrWhiteSpace(setting.SmtpPassword))
                settingsOld.SmtpPassword = setting.SmtpPassword;
            settingsOld.EntropyLevelPassword = setting.EntropyLevelPassword;
            settingsOld.UpdatedAt = DateTime.UtcNow;

            repoSettings.Update(settingsOld);
            repoSettings.SaveChanges();
        }

        public Settings Get()
        {
            return repoSettings.GetAll().First();
        }

        public void AddInitialSettings()
        {
            if (repoSettings.Count() == 0)
            {
                repoSettings.Add(new Settings()
                {
                    UpdatedAt = DateTime.UtcNow,
                    JWTSigningKey = Util.GenerateBase64(64),
                    EntropyLevelPassword = 3,
                });
                repoSettings.SaveChanges();
            }
        }
    }
}
