﻿namespace Flash.Configuration.WebApi_AOT.Models;

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);