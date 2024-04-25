namespace ZeroHunger.DTOs
{
    public class RequestDTO
    {
        public int ReqId { get; set; }

        public int ResId { get; set; }

        public DateTime ReqTime { get; set; }

        public DateTime PreserveTime { get; set; }

        public int EmpId { get; set; }

        public DateTime? CollectTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public string Status { get; set; } = null!;

        public float TotalCost { get; set; }
    }
}
