﻿using System;
using Application.Domain.ValueObjects;

namespace Application.Dal.Entities;

public class TicketDal
{
    public Guid Id { get; set; }
    
    public int SeatNumber { get; set; }
    
    public decimal Price { get; set; }
    
    public TicketType Type { get; set; }
    
    public Guid? UserId { get; set; }
    
    public Guid PlaneId { get; set; }
}