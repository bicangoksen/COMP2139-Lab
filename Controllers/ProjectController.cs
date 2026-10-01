using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Data;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers;

public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Displays a list of all projects.
    [HttpGet]
    public IActionResult Index()
    {
        var projects = _context.Projects.ToList();
        if (projects == null)
        {
            return NotFound();
        }
        return View(projects);
    }

    // Displays the Create Project form.
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Handles the submission of the Create Project form.
    [HttpPost]
    public IActionResult Create(Project project)
    {
        return RedirectToAction("Index");
    }

    // Displays the details of a specific project.
    [HttpGet]
    public IActionResult Details(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }
}