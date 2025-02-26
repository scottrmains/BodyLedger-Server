using Application.TemplateChecklists.GetByUserId;

namespace Application.Dashboard.GetByUserId;

    public sealed class DashboardResponse
    {

        public ChecklistResponse? CurrentChecklist { get; set; }
        public ChecklistResponse? PreviousChecklist { get; set; }
         public ChecklistResponse? FutureChecklist { get; set; }

    }



