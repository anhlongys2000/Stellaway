﻿using Stellaway.DTOs.Payments;

namespace Stellaway.Services;

public interface IMomoPaymentService
{
    public Task<string> CreatePaymentAsync(MomoPayment payment);
}