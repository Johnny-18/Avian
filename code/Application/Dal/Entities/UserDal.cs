using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Application.Dal.Entities;

public class UserDal
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;
}