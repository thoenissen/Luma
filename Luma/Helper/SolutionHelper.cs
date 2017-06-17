using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace Seth.Luma.Helper
{
    public static class SolutionHelper
    {
        /// <summary>
        /// Returns all projects of the solution
        /// </summary>
        /// <param name="solution">Solution</param>
        /// <returns>Projects</returns>
        public static IEnumerable<Project> GetAllProjects(Solution solution)
        {
            IEnumerable< Project> ReadProject(Project project)
            {
                // Solution Folder
                if (project.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                {
                    foreach (var projectItem in project.ProjectItems.OfType<ProjectItem>())
                    {
                        if (projectItem.Object is Project innerObject)
                        {
                            foreach (var innerProject in ReadProject(innerObject))
                            {
                                yield return innerProject;
                            }
                        }
                    }
                }
                else
                {
                    yield return project;
                }
            }

            foreach (var project in solution.Projects.OfType<Project>())
            {
                foreach (var innerProject in ReadProject(project))
                {
                    yield return innerProject;
                }
            }
        }
    }
}
