using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers;

public class ProjectsController : Controller
{
    // Displays a list of all projects.
    [HttpGet]
    public IActionResult Index()
    {
        var projects = new List<Project>()
        {
            new Project { ProjectId = 1, Name = "Project 1", Description = "First Project" },
        };
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
        var project = new Project
        {
            ProjectId = id,
            Name = "Project " + id,
            Description = "Details of Project " + id
        };
        return View(project);
    }
}