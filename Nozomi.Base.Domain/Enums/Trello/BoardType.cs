using System.ComponentModel;

namespace Nozomi.Data.Enums.Trello
{
    public enum BoardType
    {
        // To allow more opened boards, separate different boardIds
        // with ",". E.g. 5e2c209ad3384a49871082bd,5e2c209ad3384a49871082bf
        [Description("5e2c209ad3384a49871082bd")]
        PublicBoards = 0
    }
}