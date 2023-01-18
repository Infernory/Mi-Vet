using Sabio.Models.Domain.File;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain.Schedules;
using Sabio.Models.Domain.Surveys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.SurveyQuestions
{
    public class SurveyQuestion
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Question { get; set; }
        public string HelpText { get; set; }
        public bool IsRequired { get; set; }
        public bool IsMultipleAllowed { get; set; }
        public LookUp QuestionTypeId { get; set; }
        public Survey Survey { get; set; }
        public LookUp SurveyStatusId { get; set; }
        public int SortOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
