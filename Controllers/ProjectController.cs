using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers;

// Controller responsible for managing Project CRUD operations in the ProjectManagement application.
public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;

    // Initializes a new instance of the ProjectsController class with the given database context.
    public ProjectsController(ApplicationDbContext context)
    {
        // Assign the injected database context to the private field
        _context = context;
    }
    
    // GET: Projects/Index - Retrieves and displays all projects.
    [HttpGet]
    public IActionResult Index() 
    {         
        // Retrieve all project records from the database (Lab#3)
        var projects = _context.Projects.ToList();
        
        // Pass the list of projects to the Index view for rendering
        return View(projects); 
    } 

    // GET: Projects/Create - Renders the create project view form.
    [HttpGet]
    public IActionResult Create()
    {
        // Return the empty creation form view to the client
        return View();
    }
    
    // POST: Projects/Create - Processes new project submission and saves it to the database.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Project project) 
    {
        // Check if the submitted form data passes model validation rules
        if (ModelState.IsValid)
        {
            // PostgreSQL (timestamp with time zone) only accepts UTC dates, so mark the form dates as UTC
            project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
            project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);

            // Add the new project entity to the database context
            _context.Add(project);
            
            // Persist changes to the underlying database
            _context.SaveChanges();
            
            // Redirect the user back to the Index action to view the project list
            return RedirectToAction("Index");
        }
        
        // If validation fails, re-render the create form with validation errors
        return View(project);
    } 
    
    // GET: Projects/Details/{id} - Displays details for a specific project.
    [HttpGet]
    public IActionResult Details(int id) 
    {   
        // Query the database for the first project matching the specified ID
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        
        // Return a 404 Not Found result if the project does not exist
        if (project == null)
        {
            return NotFound();
        }
        
        // Pass the found project model to the Details view
        return View(project); 
    }

    // GET: Projects/Edit/{id} - Renders the edit view for an existing project.
    [HttpGet]
    public IActionResult Edit(int id)
    {
        // Look up the project by its primary key in the database
        var project = _context.Projects.Find(id);
        
        // Return 404 Not Found if the project does not exist
        if (project == null)
        {
            return NotFound();
        }
        
        // Pass the project model to the Edit view for modification
        return View(project);
    }
    
    // Lab4 - Part3 - #2 
    // POST: Projects/Edit/{id} - Updates an existing project in the database.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectId","Name","Description","StartDate","EndDate","Status")] Project project) 
    { 
        // Verify that the route ID matches the submitted project ID
        if (id != project.ProjectId) 
        { 
            return NotFound(); 
        } 

        // Check if the submitted form data is valid
        if (ModelState.IsValid)
        { 
            try 
            { 
                // PostgreSQL (timestamp with time zone) only accepts UTC dates, so mark the form dates as UTC
                project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
                project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);

                // Mark the project entity as modified and save updates to the database
                _context.Projects.Update(project); 
                _context.SaveChanges(); 
            } 
            catch (DbUpdateConcurrencyException) 
            { 
                // Handle concurrency conflicts by checking if the project still exists
                if (!ProjectExists(project.ProjectId)) 
                { 
                    return NotFound(); 
                } 
                else 
                { 
                    throw; 
                } 
            } 
            
            // Redirect to Index upon successful update
            return RedirectToAction("Index"); 
        } 
        
        // Return view with model if validation fails
        return View(project); 
    } 

    // Helper method to check if a project exists in the database by its ID.
    private bool ProjectExists(int id) 
    { 
        // Check if any project record matches the given ID
        return _context.Projects.Any(e => e.ProjectId == id); 
    }

    // GET: Projects/Delete/{id} - Renders the delete confirmation view for a specific project.
    [HttpGet]
    public IActionResult Delete(int id)
    {
        // Retrieve the project to be deleted by ID
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        
        // Return 404 Not Found if the project does not exist
        if (project == null)
        {
            return NotFound();
        }
        
        // Pass the project model to the delete confirmation view
        return View(project);
    }
    
    // POST: Projects/DeleteConfirmed/{id} - Confirms and executes the deletion of a project.
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        // Find the project record in the database by ID
        var project = _context.Projects.Find(id);
        
        // If found, remove it from the context and save database changes
        if (project != null)
        {   
            // Remove the project entity from the database context then save changes to the database
            _context.Projects.Remove(project);
            _context.SaveChanges();

            // Redirect back to the Index action after successful deletion
            return RedirectToAction("Index");
        }
        
        // Return 404 Not Found if the project was not found
        return NotFound();
    }
}