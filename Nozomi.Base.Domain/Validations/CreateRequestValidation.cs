using Nozomi.Data.Commands;

namespace Nozomi.Data.Validations
{
    public class CreateRequestValidation : RequestValidation<CreateRequestCommand>
    {
        public CreateRequestValidation()
        {
            ValidateDataPath();
        }
    }
}