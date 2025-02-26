using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Templates;

    public static class TemplateErrors
    {

        public static Error TemplateNotFound(Guid templateId) => Error.NotFound(
        "Templates.NotFound",
        $"The template with the Id = '{templateId}' was not found");

        public static Error TemplateChecklistNotFound(Guid userId) => Error.NotFound(
        "Templates.ChecklistNotFound",
        $"A checklist for the specified user '{userId}' does not exist");

        public static  Error TemplateChecklistExists(Guid userId) => Error.Conflict(
            "Templates.ChecklistExists",
            $"A checklist for the specified user '{userId}'exists");

        public static Error TemplateAssignmentExists(Guid checklistId, string dayOfWeek) => Error.Conflict(
         "Templates.TemplateAssignmentExists",
         $"An assignment for {checklistId} exists on {dayOfWeek}");

    public static readonly Error TemplateNameNotUnique = Error.Conflict(
        "WorkoutTemplates.NameNotUnique",
        "The template name has been used before.");

    public static readonly Error TemplateChecklistSameDate = Error.Conflict(
      "WorkoutTemplates.NameNotUnique",
      "The template is already set on this date");

}

