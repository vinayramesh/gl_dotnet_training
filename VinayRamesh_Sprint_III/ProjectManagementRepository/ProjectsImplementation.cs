using ProjectManagementBusiness;
using ProjectManagementBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementRepository
{
    public class ProjectsImplementation : IProjectsRepository
    {
        private readonly AppDBContext _context;
        public ProjectsImplementation(AppDBContext context)
        {
            _context = context;
        }

        public Projects AddProject(Projects project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }

        public Projects UpdateProject(Projects project)
        {
            _context.Projects.Update(project);
            _context.SaveChanges();
            return project;
        }

        public Projects DeleteProject(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ID == id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
                return project;
            }
            return null;
        }

        public List<Projects> GetAllProjects()
        {
            return _context.Projects.ToList();
        }

        public Projects GetProjectById(int id)
        {
            return _context.Projects.FirstOrDefault(p => p.ID == id);
        }

        public Projects GetProjectByName(string projectName)
        {
            return _context.Projects.FirstOrDefault(p => p.Name == projectName);
        }
    }
}
