namespace Nozomi.Data.Commands.Sources
{
    public class CreateSourceCommand : SourceCommand
    {
        public CreateSourceCommand(string abbreviation, string name, string apiDocsUrl)
        {
            Abbreviation = abbreviation;
            Name = name;
            APIDocsURL = apiDocsUrl;
        }
        
        public override bool IsValid()
        {
//            ValidationResult = new CreateRequestValidation().Validate(this);
//            return ValidationResult.IsValid;
            return true;
        }
    }
}