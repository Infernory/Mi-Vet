using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Surveys
{
    public class Survey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public LookUp Status { get; set; }
        public LookUp SurveyType { get; set; }
        public BaseUserProfile CreatedBy { get; set; }
        public List<Question> SurveyQuestions { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
