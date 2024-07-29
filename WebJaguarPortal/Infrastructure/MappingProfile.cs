using AutoMapper;

namespace WebJaguarPortal.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Project
            CreateMap<Models.Project, ViewModels.ProjectGridViewModel>();
            CreateMap<Models.Project, ViewModels.ProjectAddViewModel>();
            CreateMap<Models.Project, ViewModels.ProjectDeleteViewModel>();
            CreateMap<Models.Project, ViewModels.ProjectEditViewModel>();
            CreateMap<Models.Project, ViewModels.ProjectDetailViewModel>();

            CreateMap<ViewModels.ProjectAddViewModel, Models.Project>();
            CreateMap<ViewModels.ProjectDeleteViewModel, Models.Project>();
            CreateMap<ViewModels.ProjectEditViewModel, Models.Project>();
            #endregion

            #region User
            CreateMap<Models.User, ViewModels.UserGridViewModel>();
            CreateMap<Models.User, ViewModels.UserAddViewModel>();
            CreateMap<Models.User, ViewModels.UserDeleteViewModel>();
            CreateMap<Models.User, ViewModels.UserEditViewModel>();
            CreateMap<Models.User, ViewModels.UserDetailViewModel>();
            CreateMap<Models.UserRoles, ViewModels.UserRolesViewModel>();
            CreateMap<Models.UserPermissions, ViewModels.UserPermissionsViewModel>();

            CreateMap<ViewModels.UserAddViewModel, Models.User>();
            CreateMap<ViewModels.UserDeleteViewModel, Models.User>();
            CreateMap<ViewModels.UserEditViewModel, Models.User>();
            CreateMap<ViewModels.UserRolesViewModel, Models.UserRoles>();
            CreateMap<ViewModels.UserPermissionsViewModel, Models.UserPermissions>();
            #endregion

            #region Analyzes
            CreateMap<Models.LineAnalysis, ViewModels.LineAnalysisViewModel>();
            #endregion

            #region Settings
            CreateMap<Models.Settings, ViewModels.SettingsViewModel>();
            CreateMap<ViewModels.SettingsViewModel, Models.Settings>();
            #endregion
        }
    }
}
