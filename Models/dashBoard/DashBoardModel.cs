namespace MasterInvoice.Models.dashBoard
{
    public class DashBoardModel
    {
        /// <summary>
        /// valor total das notas emitidas
        /// </summary>
        // [Column(TypeName = "decimal(10, 2)")]
        public double Issued { get; set; }
        /// <summary>
        /// valor total das notas sem cobranca
        /// </summary>
        public double NoCharge { get; set; }
        /// <summary>
        /// valor total notas vencidas
        /// </summary>
        public double LatePayment { get; set; }
        /// <summary>
        /// valor total notas a vencer
        /// </summary>
        public double DuePayment { get; set; }
        /// <summary>
        /// valor total notas pagas
        /// </summary>
        public double PaymentMade { get; set; }
        /// <summary>
        /// inadimplência mês a mês
        /// </summary>
        public IList<GraficModel> ValuesDelinquency { get; set; }
        /// <summary>
        ///  receita recebida mês a mês
        /// </summary>
        public IList<GraficModel> ValuesPastDue { get; set; }
    }
}
