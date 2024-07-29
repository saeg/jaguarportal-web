namespace WebJaguarPortal.Infrastructure
{
    public class RolesUtil
    {
        internal const string Admin = "Admin";
        internal const string AnalyzesList = "AnalyzesList";
        internal const string AnalyzesDetail = "AnalyzesDetail";
        internal const string AnalyzesNew = "AnalyzesNew";
        internal const string AnalyzesDelete = "AnalyzesDelete";
        internal const string ProjectsList = "ProjectsList";
        internal const string ProjectsDetail = "ProjectsDetail";
        internal const string ProjectsNew = "ProjectsNew";
        internal const string ProjectsEdit = "ProjectsEdit";
        internal const string ProjectsDelete = "ProjectsDelete";
        internal const string UsersList = "UsersList";
        internal const string UsersDetail = "UsersDetail";
        internal const string UsersNew = "UsersNew";
        internal const string UsersEdit = "UsersEdit";
        internal const string UsersDelete = "UsersDelete";

        internal static IEnumerable<string> AllRoles()
        {
            var list = new List<string>();

            list.Add(Admin);
            list.Add(AnalyzesList);
            list.Add(AnalyzesDetail);
            list.Add(AnalyzesNew);
            list.Add(AnalyzesDelete);
            list.Add(ProjectsList);
            list.Add(ProjectsDetail);
            list.Add(ProjectsNew);
            list.Add(ProjectsEdit);
            list.Add(ProjectsDelete);
            list.Add(UsersList);
            list.Add(UsersDetail);
            list.Add(UsersNew);
            list.Add(UsersEdit);
            list.Add(UsersDelete);

            return list;
        }
    }
}
