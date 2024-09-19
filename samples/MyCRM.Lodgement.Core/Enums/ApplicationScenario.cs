﻿using System.ComponentModel;

namespace MyCRM.Lodgement.Common.Models;

public enum LoanApplicationScenario
{
    [Description("NewLoanApplication")]
    NewLoanApplication,
    
    [Description("Refinance")]
    Refinance,
    
    [Description("TopUp")]
    TopUp,
}