using ContactImport.Models;
using FluentValidation;

namespace ContactImport.BL.Validators;

public class ContactModelValidator : AbstractValidator<ContactModel>
{
    private const string RcRegex = @"\d{9,10}";
    private const string NumberRegex = @"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$";

    public ContactModelValidator()
    {
        RuleFor(x => x.Number1).Matches(NumberRegex);
        RuleFor(x => x.Number2).Matches(NumberRegex);
        RuleFor(x => x.Number3).Matches(NumberRegex);
        RuleFor(x => x.RC).Matches(RcRegex);
    }
}