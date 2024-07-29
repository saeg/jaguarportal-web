using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using WebJaguarPortal.Infrastructure;
using WebJaguarPortal.Models;
using WebJaguarPortal.Repository;
using WebJaguarPortal.Repository.Interfaces;

namespace WebJaguarPortal.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> logger;
        private readonly UserRepository userRepo;
        private readonly IRepository<Settings> settingsRepo;
        private readonly EmailService emailService;
        private readonly RenewPasswordRepository renewRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(ILogger<UserService> logger, UserRepository userRepo, IRepository<Settings> settingsRepo,
            EmailService emailService, RenewPasswordRepository renewRepository, LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.userRepo = userRepo;
            this.settingsRepo = settingsRepo;
            this.emailService = emailService;
            this.renewRepository = renewRepository;
            this.linkGenerator = linkGenerator;
            this.httpContextAccessor = httpContextAccessor;
        }

        public User? ValidateUserAndPassaword(string username, string password)
        {
            return userRepo.GetByUsernameAndPassword(username, password);
        }

        public User? ValidateClientCredentials(string clientId, string clientSecret)
        {
            return userRepo.GetByClientIdAndClientSecret(clientId, clientSecret);
        }

        public string GenerateClientSecret(string username)
        {
            User? user = userRepo.GetByUsername(username);
            string secret = Util.GenerateKey(64);
            if (user != null)
            {
                user.ClientSecret = Util.GenerateHash(secret);
            }

            userRepo.SaveChanges();
            return secret;
        }

        public void AddInitialUser()
        {
            if (userRepo.Count() == 0)
            {
                userRepo.Add(new User()
                {
                    Username = "admin",
                    Password = Util.GenerateHash("admin"),
                    ClientId = Util.GenerateKey(32),
                    ClientSecret = String.Empty,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Email = String.Empty,
                    FirstName = "Administrator",
                    LastName = String.Empty,
                    Roles = new UserRoles()
                    {
                        IsAdmin = true,
                        AnalyzesPermission = new UserPermissions(),
                        ProjectsPermission = new UserPermissions(),
                        UsersPermission = new UserPermissions()
                    }
                });
                userRepo.SaveChanges();
            }
        }

        public bool ValidateEntropyPassword(string password)
        {
            var rules = new[]
            {
                ".{1,}", // Password is one or more characters long
                ".{4,}", // Password is four or more characters long
                ".{8,}", // Password is eight or more characters long
                ".{10,}", // Password is ten or more characters long
                "\\d",  // Password contains numbers
                "[a-z].*?[A-Z]|[A-Z].*?[a-z]", // Password is mixed case
                "[!@#$%^&*?_~-£() ]" // Password has special characters
            }
            .Select(rule => new Regex(rule));

            var matches = rules.Select(regex => regex.Match(password));
            int score = matches.Sum(m => m.Success ? 1 : 0);

            int? MinimumScore = settingsRepo.GetAll()?.FirstOrDefault()?.EntropyLevelPassword;

            return score >= (MinimumScore ?? 3);
        }

        public void UpdatePersonalData(string username, string lastName, string firstName, string email)
        {
            User? user = userRepo.GetByUsername(username);

            if (user != null)
            {
                user.LastName = lastName;
                user.FirstName = firstName;
                user.Email = email;
                userRepo.SaveChanges();
            }
        }

        internal void ChangePassword(string name, string password)
        {
            User? user = userRepo.GetByUsername(name);

            if (user != null)
            {
                user.Password = Util.GenerateHash(password);
                userRepo.SaveChanges();
            }
        }

        internal User? GetByUsername(string name)
        {
            return userRepo.GetByUsername(name);
        }
        internal User? GetByEmail(string email)
        {
            return userRepo.GetByEmail(email);
        }
        internal User? GetByUsernameOrEmail(string userNameOrEmail)
        {
            return userRepo.GetByUsernameOrEmail(userNameOrEmail);
        }


        public void Add(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.ClientId = Util.GenerateKey(32);
            user.Password = string.Empty;
            user.Username = user.Username.ToLowerInvariant();
            userRepo.Add(user);
            userRepo.SaveChanges();

            SendFirstLogin(user);
        }

        public void Update(User user)
        {
            User? userOld = userRepo.GetById(user.Id);
            if (userOld != null)
            {
                userOld.UpdatedAt = DateTime.UtcNow;
                userOld.FirstName = user.FirstName;
                userOld.LastName = user.LastName;
                userOld.Email = user.Email;
                userOld.Roles = user.Roles;
                userRepo.SaveChanges();
            }
        }

        public void Delete(long id)
        {
            User? user = userRepo.GetById(id);

            if (user != null)
            {
                userRepo.Delete(user);
                userRepo.SaveChanges();
            }
        }

        public User? GetById(long id)
        {
            return userRepo.GetById(id);
        }

        internal IEnumerable<User> GetAll()
        {
            return userRepo.GetAll();
        }

        private IEnumerable<string> GetUserRoles(long id)
        {
            var roles = new List<string>();
            User? user = userRepo.GetById(id);

            if (user != null && user.Roles != null)
            {
                if (user.Roles.IsAdmin)
                {
                    roles.AddRange(RolesUtil.AllRoles());
                }
                else
                {
                    if (user.Roles.AnalyzesPermission != null)
                    {
                        if (user.Roles.AnalyzesPermission.List)
                            roles.Add(RolesUtil.AnalyzesList);
                        if (user.Roles.AnalyzesPermission.Detail)
                            roles.Add(RolesUtil.AnalyzesDetail);
                        if (user.Roles.AnalyzesPermission.New)
                            roles.Add(RolesUtil.AnalyzesNew);
                        if (user.Roles.AnalyzesPermission.Delete)
                            roles.Add(RolesUtil.AnalyzesDelete);
                    }

                    if (user.Roles.ProjectsPermission != null)
                    {
                        if (user.Roles.ProjectsPermission.List)
                            roles.Add(RolesUtil.ProjectsList);
                        if (user.Roles.ProjectsPermission.Detail)
                            roles.Add(RolesUtil.ProjectsDetail);
                        if (user.Roles.ProjectsPermission.New)
                            roles.Add(RolesUtil.ProjectsNew);
                        if (user.Roles.ProjectsPermission.Edit)
                            roles.Add(RolesUtil.ProjectsEdit);
                        if (user.Roles.ProjectsPermission.Delete)
                            roles.Add(RolesUtil.ProjectsDelete);
                    }

                    if (user.Roles.UsersPermission != null)
                    {
                        if (user.Roles.UsersPermission.List)
                            roles.Add(RolesUtil.UsersList);
                        if (user.Roles.UsersPermission.Detail)
                            roles.Add(RolesUtil.UsersDetail);
                        if (user.Roles.UsersPermission.New)
                            roles.Add(RolesUtil.UsersNew);
                        if (user.Roles.UsersPermission.Edit)
                            roles.Add(RolesUtil.UsersEdit);
                        if (user.Roles.UsersPermission.Delete)
                            roles.Add(RolesUtil.UsersDelete);
                    }
                }
            }

            return roles;
        }

        internal IEnumerable<Claim> GenerateClaims(User user)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.GivenName, user.FirstName),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

            foreach (var item in GetUserRoles(user.Id))
                claims.Add(new Claim(ClaimTypes.Role, item));

            return claims;
        }

        internal void SendForgotPassword(User user)
        {
            string key = Guid.NewGuid().ToString();

            renewRepository.Add(new RenewPassword()
            {
                CreatedAt = DateTime.UtcNow,
                Key = key,
                Username = user.Username
            });
            renewRepository.SaveChanges();

            string? url = linkGenerator.GetUriByAction(httpContextAccessor.HttpContext, "ForgotPassword", "Login", new { id = key });
            string? urlNewRequest = linkGenerator.GetUriByAction(httpContextAccessor.HttpContext, "Index", "Login");

            StringBuilder sb = new();

            sb.AppendLine($"Hello {user.FirstName} {user.LastName},");
            sb.AppendLine();
            sb.AppendLine($"Have you made a password renewal request on the Jaguar Portal system? If so, <a href='{url}'>click here</a> or copy the link below and paste it into your browser to set a new password.");
            sb.AppendLine();
            sb.AppendLine($"{url}");
            sb.AppendLine();
            sb.AppendLine($"Link valid for 15 minutes, but don't worry, if you can't access it in time, you can make a new request, for that, <a href=\"{urlNewRequest}\">click here</a> or copy the link below and paste it into your browser.");
            sb.AppendLine();
            sb.AppendLine($"{urlNewRequest}");
            sb.AppendLine();

            string message = sb.ToString();
            string destination = user.Email;
            string subject = "Jaguar Portal - Fogot password";

            emailService.SendMail(message, subject, destination);
        }

        internal void SendFirstLogin(User user)
        {
            string key = Guid.NewGuid().ToString();

            renewRepository.Add(new RenewPassword()
            {
                CreatedAt = DateTime.UtcNow,
                Key = key,
                Username = user.Username
            });
            renewRepository.SaveChanges();

            string? url = linkGenerator.GetUriByAction(httpContextAccessor.HttpContext, "NewPassword", "Login", new { id = key });
            string? urlNewRequest = linkGenerator.GetUriByAction(httpContextAccessor.HttpContext, "Index", "Login");

            StringBuilder sb = new();

            sb.AppendLine($"Hello {user.FirstName} {user.LastName}, welcome to Portal Jaguar!");
            sb.AppendLine();
            sb.AppendLine($"<a href=\"{url}\">Click here</a> or copy the link below and paste it into your browser to define your access password.");
            sb.AppendLine();
            sb.AppendLine($"{url}");
            sb.AppendLine();
            sb.AppendLine($"This link is valid for 15 minutes, but don't worry, if you can't access it in time, you can make a new request, for that, <a href=\"{urlNewRequest}\">click here</a> or copy the link below and paste it into your browser.");
            sb.AppendLine();
            sb.AppendLine($"{urlNewRequest}");
            sb.AppendLine();

            string message = sb.ToString();
            string destination = user.Email;
            string subject = "Jaguar Portal - First login";

            emailService.SendMail(message, subject, destination);
        }

        internal bool ValidateForgotPasswordRequest(string key, out Models.RenewPassword? request)
        {
            request = renewRepository.GetByKey(key);

            return request != null &&
                   DateTime.UtcNow.AddMinutes(-15) <= request.CreatedAt;
        }

        internal void DeleteForgotPasswordByKey(string key)
        {
            var renew = renewRepository.GetByKey(key);
            if (renew != null)
                renewRepository.Delete(renew);
            renewRepository.SaveChanges();
        }

        internal bool VerifyUniqueEmail(string email, long? id = null)
        {
            return userRepo.VerifyUniqueEmail(email, id);
        }
        internal bool VerifyUniqueUsername(string username)
        {
            return userRepo.VerifyUniqueUsername(username);
        }

        internal bool VerifyUniqueAdmin(long id)
        {
            return userRepo.VerifyUniqueAdmin(id);
        }
        internal bool VerifyDeleteUniqueAdmin(long id)
        {
            return userRepo.VerifyDeleteUniqueAdmin(id);
        }
    }
}
