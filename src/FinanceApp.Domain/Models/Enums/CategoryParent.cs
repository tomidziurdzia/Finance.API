namespace FinanceApp.Domain.Models.Enums
{
    public enum CategoryParent
    {
        Others,
        Home,
        VehicleAndTransport,
        Shopping,
        Travel,
        OtherExpenses,
        Food,
        EducationHealthAndSports,
        Salary,
        OtherIncome,
        Investment,
        Savings,
        Taxes,
        Fee,
        Transfer,
        DiningOut
    }

    public static class CategoryParentExtensions
    {
        public static string ToFriendlyString(this CategoryParent me)
        {
            switch (me)
            {
                case CategoryParent.Others:
                    return "Others";
                case CategoryParent.Home:
                    return "Home";
                case CategoryParent.VehicleAndTransport:
                    return "Vehicle And Transport";
                case CategoryParent.Shopping:
                    return "Shopping";
                case CategoryParent.Travel:
                    return "Travel";
                case CategoryParent.OtherExpenses:
                    return "Other Expenses";
                case CategoryParent.Food:
                    return "Food";
                case CategoryParent.EducationHealthAndSports:
                    return "Education Health And Sports";
                case CategoryParent.Salary:
                    return "Salary";
                case CategoryParent.OtherIncome:
                    return "Other Income";
                case CategoryParent.Investment:
                    return "Investment";
                case CategoryParent.Savings:
                    return "Savings";
                case CategoryParent.Transfer:
                    return "Transfer";
                case CategoryParent.Taxes:
                    return "Taxes";
                case CategoryParent.Fee:
                    return "Fee";
                case CategoryParent.DiningOut:
                    return "DiningOut";
                default:
                    return "Others";
            }
        }
    }
}