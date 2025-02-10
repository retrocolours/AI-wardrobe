namespace AI_Wardrobe.Models
{
public class PayPalConfirmationModel
{
public required string TransactionId { get; set; }
public required string Amount { get; set; }
public required string PayerName { get; set; }
}
}