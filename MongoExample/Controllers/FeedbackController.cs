using System;
using Microsoft.AspNetCore.Mvc;
using MongoExample.Services;
using MongoExample.Models;

namespace MongoExample.Controllers;

[Controller]
[Route("api/[controller]")]

public class FeedbackController: Controller {

    private readonly MongoDBService _mongoDBService;

    public FeedbackController(MongoDBService mongoDBService){
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public async Task<List<Feedback>> Get() {
        return await _mongoDBService.GetAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Feedback feedback) {
        await _mongoDBService.CreateAsync(feedback);
        return CreatedAtAction(nameof(Get), new {id = feedback.Id}, feedback);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AddToFeedback(string id, [FromBody] string feedback) {
        await _mongoDBService.AddToFeedbackAsync(id, feedback);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) {
        await _mongoDBService.DeleteAsync(id);
        return NoContent();
    }

}