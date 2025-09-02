using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using Repository.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Controllers;
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepo _userRepo;

    public UserController(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    /// <summary>
    /// Retrieves all users from the repository.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch all users from the underlying data
    /// source. The result is returned with an HTTP 200 OK status code.</remarks>
    /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="IEnumerable{T}"/> of <see cref="User"/> objects.
    /// Returns an empty collection if no users are found.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await _userRepo.GetAllAsync();
        return Ok(users);
    }
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch the user from the repository.  If the
    /// user does not exist, a 404 Not Found response is returned.</remarks>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the user if found; otherwise, a <see cref="NotFoundResult"/>.</returns>

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// Creates a new user and adds it to the repository.
    /// </summary>
    /// <param name="user">The user to be created. Must not be <see langword="null"/>.</param>
    /// <returns>An <see cref="ActionResult"/> that represents the result of the operation.  Returns a <see
    /// cref="CreatedAtActionResult"/> containing the created user and a location header  pointing to the <c>GetById</c>
    /// action.</returns>
    [HttpPost]
    public async Task<ActionResult> Create(User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _userRepo.AddAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
    /// <summary>
    /// Updates the details of an existing user.
    /// </summary>
    /// <remarks>This method updates the user record in the repository with the provided details. Ensure that
    /// the <paramref name="user"/> object contains valid data before calling this method.</remarks>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="user">The updated user details. The <see cref="User.Id"/> property must match the <paramref name="id"/> parameter.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation. Returns <see cref="BadRequestResult"/> if
    /// the <paramref name="id"/> does not match the <see cref="User.Id"/>. Returns <see cref="NoContentResult"/> if the
    /// update is successful.</returns>

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, User user)
    {
        if (id != user.Id || !ModelState.IsValid)
            return BadRequest();
       

        await _userRepo.UpdateAsync(user);
        return NoContent();
    }

    /// <summary>
    /// Deletes the user with the specified identifier.
    /// </summary>
    /// <remarks>This operation removes the user from the system. If the user does not exist, the operation 
    /// completes without error, but no changes are made.</remarks>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _userRepo.DeleteAsync(id);
        return NoContent();
    }
}