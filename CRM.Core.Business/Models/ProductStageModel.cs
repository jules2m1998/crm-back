namespace CRM.Core.Business.Models;

public static class ProductStageModel
{
    public class In
    {
        public Guid ProductId { get; set; }
        public int StageLevel { get; set; } = 0; 
        public string Name { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public bool? IsActivated { get; set; } = true;
    }

    public class Out: BaseModelOut
    {
        public string Name { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;
        public int StageLevel { get; set; } = 0;
        public string Question { get; set; } = string.Empty;

        public ICollection<StageResponseModel.Out> Responses { get; set; } = new List<StageResponseModel.Out>();
    }
}
