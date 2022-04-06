using FluentValidation;

namespace SpaceTestProject.Application.WatchListItems.Commands.Add
{
    public class AddWatchListItemCommandValidator : AbstractValidator<AddWatchListItemCommand>
    {
        public AddWatchListItemCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("User id must be greater that 0");
            RuleFor(x => x.TitleId).Must(x => x.StartsWith("tt")).WithMessage("Title id must start with tt");
        }
    }
}
