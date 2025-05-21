using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Data;
using NotesApi.Dto.Note;
using NotesApi.Models;

namespace NotesApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INotesRepository _notesRepository;

    public NotesController(INotesRepository notesRepository)
    {
        _notesRepository = notesRepository;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID not found in token");

        return int.Parse(userIdClaim.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
    {
        var userId = GetUserId();
        var notes = await _notesRepository.GetAllByUserIdAsync(userId);
        return Ok(notes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Note>> GetNote(int id)
    {
        var userId = GetUserId();
        var note = await _notesRepository.GetByIdAsync(id, userId);

        if (note == null)
            return NotFound();

        return Ok(note);
    }

    [HttpPost]
    public async Task<ActionResult<Note>> CreateNote(NoteCreateDto noteDto)
    {
        if (string.IsNullOrWhiteSpace(noteDto.Title))
            return BadRequest("Title is required");

        var userId = GetUserId();
        var note = await _notesRepository.CreateAsync(noteDto, userId);

        return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Note>> UpdateNote(int id, NoteUpdateDto noteDto)
    {
        if (string.IsNullOrWhiteSpace(noteDto.Title))
            return BadRequest("Title is required");

        var userId = GetUserId();
        var note = await _notesRepository.UpdateAsync(id, noteDto, userId);

        if (note == null)
            return NotFound();

        return Ok(note);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteNote(int id)
    {
        var userId = GetUserId();
        var result = await _notesRepository.DeleteAsync(id, userId);

        if (!result)
            return NotFound();

        return NoContent();
    }
}